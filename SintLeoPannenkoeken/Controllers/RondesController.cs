using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Filters;
using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.ViewModels.Rondes;

namespace SintLeoPannenkoeken.Controllers
{
    public class RondesController : Controller
    {
        private ILogger<RondesController> _logger;
        private ApplicationDbContext _dbContext;

        public RondesController(ILogger<RondesController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [TypeFilter(typeof(ScoutsjaarRedirectionFilter))]
        public IActionResult Index(int? scoutsjaar)
        {
            Scoutsjaar? sj = _dbContext.Scoutsjaren.SingleOrDefault(s => s.Begin == scoutsjaar);
            var rondes = _dbContext.Rondes
                .Include(ronde => ronde.Bestuurder)
                .Include(ronde => ronde.Zone)
                .ToList();

            var bestellingen = _dbContext.Scoutsjaren
                .Include(sj => sj.Bestellingen)
                .ThenInclude(bestelling => bestelling.Straat)
                .ThenInclude(straat => straat.Zone)
                .Single(sj => sj.Begin == scoutsjaar)
                .Bestellingen
                .ToList();

            var zones = _dbContext.Zones.ToList();

            return View(new IndexViewModel(sj, rondes, bestellingen, zones));
        }
    }
}
