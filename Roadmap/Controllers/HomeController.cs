using Microsoft.AspNetCore.Mvc;

namespace Roadmap.Controllers;

public class HomeController(ILogger<HomeController> logger) : Controller
{
    public IActionResult Index() => View();
}