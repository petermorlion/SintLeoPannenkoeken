using Microsoft.AspNetCore.Mvc;

namespace SintLeoPannenkoeken.Controllers
{
    public class RapportenController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
