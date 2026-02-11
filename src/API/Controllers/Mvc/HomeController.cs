using Microsoft.AspNetCore.Mvc;

namespace TravelCleanArch.API.Controllers.Mvc;

public sealed class HomeController : Controller
{
    [HttpGet("/")]
    public IActionResult Index()
    {
        return View();
    }
}
