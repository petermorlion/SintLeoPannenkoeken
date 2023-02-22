using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult Rondes(int bestuurderId, int scoutsjaar)
        {
            var zones = _dbContext.Zones.ToList();
            var bestuurder = _dbContext.Bestuurders.Single(bestuurder => bestuurder.Id == bestuurderId);
            var viewModel = new BestuurderRondesViewModel(scoutsjaar, bestuurder, zones);

            return View(viewModel);
        }
    }
}
