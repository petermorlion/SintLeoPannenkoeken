using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.Controllers
{
    public class RondeController : Controller
    {
        private readonly ILogger<RondeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public RondeController(ILogger<RondeController> logger, ApplicationDbContext dbContext)
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

            var bestuurders = _dbContext.Bestuurders.ToList();
            var zones = _dbContext.Zones.ToList();

            return View(new ViewModels.Rondes.IndexViewModel(sj, bestuurders, zones));
        }
    }
}
