using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Filters;
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

        [TypeFilter(typeof(ScoutsjaarRedirectionFilter))]
        public IActionResult Index(int? scoutsjaar)
        {
            Scoutsjaar? sj = _dbContext.Scoutsjaren.SingleOrDefault(s => s.Begin == scoutsjaar);
            var takken = _dbContext.Takken.ToList();
            return View(new IndexViewModel(sj, takken));
        }
    }
}
