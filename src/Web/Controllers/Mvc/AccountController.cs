using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelCleanArch.Web.Models.Account;
using TravelCleanArch.Application.Abstractions.Identity;

namespace TravelCleanArch.Web.Controllers.Mvc;

[AllowAnonymous]
[Route("account")]
public sealed class AccountController(IInteractiveAuthService interactiveAuthService) : Controller
{
    [HttpGet("login")]
    public IActionResult Login(string? returnUrl = null)
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(model);

        var signIn = await interactiveAuthService.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, ct);
        if (!signIn.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, signIn.Error?.Message ?? "Unable to sign in.");
            return View(model);
        }

        if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            return Redirect(model.ReturnUrl);

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpGet("register")]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost("register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(model);

        var register = await interactiveAuthService.RegisterCustomerAsync(model.Email, model.Password, model.FullName, ct);
        if (!register.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, register.Error?.Message ?? "Unable to register.");
            return View(model);
        }

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [Authorize(AuthenticationSchemes = IdentityConstants.ApplicationScheme)]
    [HttpPost("logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        await interactiveAuthService.SignOutAsync(ct);
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpGet("access-denied")]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
