using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.ViewModels.Bestellingen;

namespace SintLeoPannenkoeken.Controllers
{
    public class BestellingenController : Controller
    {
        private readonly ILogger<BestellingenController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public BestellingenController(ILogger<BestellingenController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index(int? scoutsjaar)
        {
            if (scoutsjaar == null)
            {
                var currentScoutsjaar = _dbContext.Scoutsjaren.OrderByDescending(s => s.Begin).First();
                return Redirect($"/bestellingen?scoutsjaar={currentScoutsjaar.Begin}");
            }

            Scoutsjaar? sj = _dbContext.Scoutsjaren.SingleOrDefault(s => s.Begin == scoutsjaar);
            if (sj == null)
            {
                var currentScoutsjaar = _dbContext.Scoutsjaren.OrderByDescending(s => s.Begin).First();
                return Redirect($"/bestellingen?scoutsjaar={currentScoutsjaar.Begin}");
            }

            return View(new IndexViewModel(sj));
        }
    }
}
