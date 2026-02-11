using Microsoft.AspNetCore.Mvc;

namespace TravelCleanArch.Web.Controllers.Mvc;

public sealed class HomeController : Controller
{
    [HttpGet("/")]
    public IActionResult Index()
    {
        return View();
    }
}
