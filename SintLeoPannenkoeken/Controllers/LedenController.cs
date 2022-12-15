using Microsoft.AspNetCore.Mvc;

namespace SintLeoPannenkoeken.Controllers
{
    public class LedenController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
