using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roadmap.Security;

namespace Roadmap.Controllers;

public class AccountController : Controller
{
    [AllowAnonymous]
    public ActionResult Login() => Challenge(new AuthenticationProperties { RedirectUri = "/" },
        Constants.AuthenticationScheme.Oidc);

    public IActionResult Logout() =>
        SignOut(CookieAuthenticationDefaults.AuthenticationScheme, Constants.AuthenticationScheme.Oidc);

    public IActionResult Profile() => View();
}