using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.ViewModels.Leden;

namespace SintLeoPannenkoeken.Controllers
{
    [Authorize(Roles = "Admin,FinanciePloeg")]
    public class LedenController : Controller
    {
        private ILogger<LedenController> _logger;
        private ApplicationDbContext _dbContext;

        public LedenController(ILogger<LedenController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var takken = _dbContext.Takken.ToList();
            return View(new IndexViewModel(takken));
        }
    }
}
