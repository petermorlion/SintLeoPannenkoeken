using Microsoft.AspNetCore.Mvc;

namespace SintLeoPannenkoeken.Controllers
{
    public class BestellingenController : Controller
    {
        private readonly ILogger<BestellingenController> _logger;

        public BestellingenController(ILogger<BestellingenController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
