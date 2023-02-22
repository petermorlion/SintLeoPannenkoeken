using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;
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

        public IActionResult Index(int? scoutsjaar)
        {
            if (scoutsjaar == null)
            {
                var currentScoutsjaar = _dbContext.Scoutsjaren.OrderByDescending(s => s.Begin).First();
                return Redirect($"/bestuurders?scoutsjaar={currentScoutsjaar.Begin}");
            }

            Scoutsjaar? sj = _dbContext.Scoutsjaren.SingleOrDefault(s => s.Begin == scoutsjaar);
            if (sj == null)
            {
                var currentScoutsjaar = _dbContext.Scoutsjaren.OrderByDescending(s => s.Begin).First();
                return Redirect($"/bestuurders?scoutsjaar={currentScoutsjaar.Begin}");
            }

            return View(new IndexViewModel(sj));
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
