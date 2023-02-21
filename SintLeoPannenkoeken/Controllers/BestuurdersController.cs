using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.ViewModels.Bestuurders;

namespace SintLeoPannenkoeken.Controllers
{
    public class BestuurdersController : Controller
    {
        private ILogger<BestuurdersController> _logger;
        private ApplicationDbContext _dbContext;

        public BestuurdersController(ILogger<BestuurdersController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View(new IndexViewModel());
        }
    }
}
