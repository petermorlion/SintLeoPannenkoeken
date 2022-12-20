using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;

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

            var leden = _dbContext.Leden.Include(lid => lid.Tak).ToList();
            var straten = _dbContext.Straten.ToList();

            return View(new ViewModels.Bestellingen.IndexViewModel(sj, leden, straten));
        }
    }
}
