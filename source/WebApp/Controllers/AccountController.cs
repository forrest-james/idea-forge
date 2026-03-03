using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using WebApp.ViewModels.Account;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    [Route("account")]
    public sealed class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(SignInManager<IdentityUser> signInManager) => _signInManager = signInManager;

        [HttpGet("login")]
        public IActionResult Login(string? returnUrl = null) => View(new LoginVm { ReturnUrl = returnUrl ?? "/" });

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVm vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _signInManager.PasswordSignInAsync(
                userName: vm.Email,
                password: vm.Password,
                isPersistent: vm.RememberMe,
                lockoutOnFailure: false);

            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(vm);
            }

            if (!string.IsNullOrWhiteSpace(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl))
                return Redirect(vm.ReturnUrl);

            return Redirect("/");
        }

        [HttpPost("/account/login-test")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginTest([FromServices] SignInManager<IdentityUser> signInManager, [FromServices] UserManager<IdentityUser> userManager, string? returnUrl = null)
        {
            const string testEmail = "testuser@ideaforge.local";
            var user = await userManager.FindByEmailAsync(testEmail);
            if (user == null)
                return RedirectToAction("Login");

            await signInManager.SignInAsync(user, isPersistent: false);

            return LocalRedirect(string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl);
        }

        [Authorize]
        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        [HttpGet("/account/set-password")]
        public IActionResult SetPassword([FromQuery] string userId, [FromQuery] string token)
        {
            return View(new SetPasswordVm { UserId = userId, Token = token });
        }

        [AllowAnonymous]
        [HttpPost("/account/set-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword([FromServices] UserManager<IdentityUser> userManager, SetPasswordVm vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await userManager.FindByIdAsync(vm.UserId);
            if (user is null)
            {
                ModelState.AddModelError("", "Invalid user.");
                return View(vm);
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(vm.Token));            

            var result = await userManager.ResetPasswordAsync(user, decodedToken, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(vm);
            }

            TempData["Success"] = "Password set successfully. You can no log in.";
            return Redirect("/account/login");
        }
    }
}