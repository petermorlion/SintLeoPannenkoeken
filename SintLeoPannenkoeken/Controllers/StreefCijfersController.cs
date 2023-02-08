using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.ViewModels.StreefCijfers;

namespace SintLeoPannenkoeken.Controllers
{
    public class StreefCijfersController : Controller
    {
        private ILogger<StreefCijfersController> _logger;
        private ApplicationDbContext _dbContext;

        public StreefCijfersController(ILogger<StreefCijfersController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index(int? scoutsjaar)
        {
            if (scoutsjaar == null)
            {
                var currentScoutsjaar = _dbContext.Scoutsjaren.OrderByDescending(s => s.Begin).First();
                return Redirect($"/streefCijfers?scoutsjaar={currentScoutsjaar.Begin}");
            }

            Scoutsjaar? sj = _dbContext.Scoutsjaren.SingleOrDefault(s => s.Begin == scoutsjaar);
            if (sj == null)
            {
                var currentScoutsjaar = _dbContext.Scoutsjaren.OrderByDescending(s => s.Begin).First();
                return Redirect($"/streefCijfers?scoutsjaar={currentScoutsjaar.Begin}");
            }

            var takken = _dbContext.Takken.ToList();
            return View(new IndexViewModel(sj, takken));
        }
    }
}
