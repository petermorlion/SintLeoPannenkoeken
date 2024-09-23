using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SintLeoPannenkoeken.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ApiClientsController : Controller
    {
        private ILogger<ApiClientsController> _logger;

        public ApiClientsController(ILogger<ApiClientsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
