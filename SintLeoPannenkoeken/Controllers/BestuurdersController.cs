using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Filters;
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

        [TypeFilter(typeof(ScoutsjaarRedirectionFilter))]
        public IActionResult Index(int? scoutsjaar)
        {
            Scoutsjaar? sj = _dbContext.Scoutsjaren.SingleOrDefault(s => s.Begin == scoutsjaar);
            return View(new IndexViewModel(sj));
        }

        public IActionResult Rondes(int bestuurderId, int scoutsjaar)
        {
            var zones = _dbContext.Zones.ToList();
            var bestuurder = _dbContext.Bestuurders.Single(bestuurder => bestuurder.Id == bestuurderId);
            var bestellingen = _dbContext.Scoutsjaren
                .Include(sj => sj.Bestellingen)
                .ThenInclude(bestelling => bestelling.Straat)
                .ThenInclude(straat => straat.Zone)
                .Single(sj => sj.Begin == scoutsjaar)
                .Bestellingen
                .ToList();

            var viewModel = new BestuurderRondesViewModel(scoutsjaar, bestuurder, zones, bestellingen);

            return View(viewModel);
        }
    }
}
