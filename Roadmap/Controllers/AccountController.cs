using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roadmap.Helpers;
using Roadmap.Security;
using Roadmap.Services;

namespace Roadmap.Controllers;

public class AccountController(UserService userService)
    : Controller
{
    [AllowAnonymous]
    public ActionResult Login() => Challenge(new AuthenticationProperties { RedirectUri = "/" },
        Constants.AuthenticationScheme.Oidc);

    [AllowAnonymous]
    public IActionResult Logout() =>
        SignOut(CookieAuthenticationDefaults.AuthenticationScheme, Constants.AuthenticationScheme.Oidc);

    public IActionResult Profile() => View(userService.GetUser(User.GetId()));
}