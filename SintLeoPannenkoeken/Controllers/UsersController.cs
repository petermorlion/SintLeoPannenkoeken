using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.ViewModels.Users;

namespace SintLeoPannenkoeken.Controllers
{
    public class UsersController : Controller
    {
        private ILogger<UsersController> _logger;
        private ApplicationDbContext _dbContext;

        public UsersController(ILogger<UsersController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var users = _dbContext.Users.ToList();
            return View(new IndexViewModel(users));
        }
    }
}
