using Application.Common.Interfaces;
using Data.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApp.ViewModels.Users;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("users")]
    public class UsersUiController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAppDbContext _dbContext;
        private readonly IImageStorage _imageStorage;

        public UsersUiController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IAppDbContext dbContext, IImageStorage imageStorage)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _imageStorage = imageStorage;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var roles = await _roleManager.Roles
                .Select(r => r.Name)
                .OrderBy(r => r)
                .ToListAsync(cancellationToken);

            var users = _userManager.Users
                .OrderBy(u => u.Email)
                .ToList();

            var userIds = users.Select(u => u.Id).ToList();

            var submissionCounts = await _dbContext.Submissions
                .AsNoTracking()
                .Where(s => s.CreatedBy != null && userIds.Contains(s.CreatedBy))
                .GroupBy(s => s.CreatedBy)
                .Select(g => new { UserId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.UserId, x => x.Count, cancellationToken);

            var rows = new List<UserRowVm>();
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var role = userRoles.FirstOrDefault() ?? "";

                rows.Add(new UserRowVm(
                    Id: user.Id,
                    Email: user.Email ?? user.UserName ?? "(no email)",
                    Role: role,
                    SubmissionCount: submissionCounts.TryGetValue(user.Id, out var count) ? count : 0
                ));
            }

            return View(new UsersIndexVm { Users = rows, Roles = roles });
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] string email, [FromForm] string role, CancellationToken cancellationToken)
        {
            email = (email ?? "").Trim();

            if (string.IsNullOrWhiteSpace(email))
                throw new InvalidOperationException("Email is required.");

            if (!await _roleManager.RoleExistsAsync(role))
                throw new InvalidOperationException("Role does not exist.");

            var existing = await _userManager.FindByEmailAsync(email);
            if (existing is not null)
                throw new InvalidOperationException("A user with that email already exists.");

            var user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var create = await _userManager.CreateAsync(user);
            if (!create.Succeeded)
                throw new InvalidOperationException(string.Join("; ", create.Errors.Select(e => e.Description)));

            var addRole = await _userManager.AddToRoleAsync(user, role);
            if (!addRole.Succeeded)
                throw new InvalidOperationException(string.Join("; ", addRole.Errors.Select(e => e.Description)));

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("{id}/role")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole([FromRoute] string id, [FromForm] string role, CancellationToken cancellationToken)
        {
            if (!await _roleManager.RoleExistsAsync(role))
                throw new InvalidOperationException("Role does not exist.");

            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Count > 0)
            {
                var remove = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!remove.Succeeded)
                    throw new InvalidOperationException(string.Join("; ", remove.Errors.Select(e => e.Description)));
            }

            var add = await _userManager.AddToRoleAsync(user, role);
            if (!add.Succeeded)
                throw new InvalidOperationException(string.Join("; ", add.Errors.Select(e => e.Description)));

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("{id}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return RedirectToAction(nameof(Index));

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == id)
                throw new InvalidOperationException("You cannot delete your own account.");

            var submissions = await _dbContext.Submissions
                .Include(s => s.Images)
                .Where(s => s.CreatedBy == id)
                .ToListAsync(cancellationToken);

            foreach (var submission in submissions)
            {
                foreach (var image in submission.Images)
                {
                    await _imageStorage.DeleteByAbsoluteUrl(image.Url.Value, cancellationToken);
                }
            }

            if (submissions.Count > 0)
            {
                _dbContext.Submissions.RemoveRange(submissions);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

            return RedirectToAction(nameof(Index));
        }
    }
}