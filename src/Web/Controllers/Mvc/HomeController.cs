using Microsoft.AspNetCore.Mvc;

namespace TravelCleanArch.Web.Controllers.Mvc;

public sealed class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
