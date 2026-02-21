using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        [Authorize]
        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}