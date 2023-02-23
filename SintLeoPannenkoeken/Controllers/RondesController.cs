using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
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

        public IActionResult Index(int? scoutsjaar)
        {
            if (scoutsjaar == null)
            {
                var currentScoutsjaar = _dbContext.Scoutsjaren.OrderByDescending(s => s.Begin).First();
                return Redirect($"/rondes?scoutsjaar={currentScoutsjaar.Begin}");
            }

            Scoutsjaar? sj = _dbContext.Scoutsjaren.SingleOrDefault(s => s.Begin == scoutsjaar);
            if (sj == null)
            {
                var currentScoutsjaar = _dbContext.Scoutsjaren.OrderByDescending(s => s.Begin).First();
                return Redirect($"/rondes?scoutsjaar={currentScoutsjaar.Begin}");
            }

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
