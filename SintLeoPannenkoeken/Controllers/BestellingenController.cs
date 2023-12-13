using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Filters;
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

        [TypeFilter(typeof(ScoutsjaarRedirectionFilter))]
        public IActionResult Index(int? scoutsjaar)
        {
            var leden = _dbContext.Leden.Include(lid => lid.Tak).ToList();
            var straten = _dbContext.Straten.Include(straat => straat.Zone).ToList();
            Scoutsjaar? sj = _dbContext.Scoutsjaren.SingleOrDefault(s => s.Begin == scoutsjaar);
            return View(new ViewModels.Bestellingen.IndexViewModel(sj, leden, straten));
        }
    }
}
