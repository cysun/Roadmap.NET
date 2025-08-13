using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Roadmap.Helpers;
using Roadmap.Models;
using Roadmap.Services;

namespace Roadmap.Controllers;

[AllowAnonymous]
public class HomeController(UserService userService, ILogger<HomeController> logger) : Controller
{
    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true && userService.GetUser(User.GetId()) == null)
            userService.AddUser(new User
            {
                Id = User.GetId(),
                FirstName = User.FindFirstValue(ClaimTypes.GivenName),
                LastName = User.FindFirstValue(ClaimTypes.Surname),
                Email = User.FindFirstValue(ClaimTypes.Email)
            });

        return View();
    }

    public IActionResult AccessDenied() => View();

    // Displays a page for non-OK results (e.g. NotFound()) returned by controller.
    public IActionResult Error(int id)
    {
        ViewBag.Code = id;
        switch (id)
        {
            case 404:
                return View("404");
            default:
                return View(new ErrorViewModel
                {
                    Message = $"Sorry, the request cannot be completed. Error code: {id}"
                });
        }
    }

    // Logs unhandled exceptions and displays an error page.
    public IActionResult Exception()
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        logger.LogWarning("Exception {exception} caused by user {user}: \n{stackTrace}",
            feature?.Error.Message, User.Identity?.Name, feature?.Error.StackTrace);

        return View();
    }
}