using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.ViewModels.Scoutsjaren;

namespace SintLeoPannenkoeken.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ScoutsjarenController : Controller
    {
        private ILogger<ScoutsjarenController> _logger;
        private ApplicationDbContext _dbContext;

        public ScoutsjarenController(ILogger<ScoutsjarenController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var scoutsjaren = _dbContext.Scoutsjaren.ToList();
            return View(new IndexViewModel(scoutsjaren));
        }
    }
}
