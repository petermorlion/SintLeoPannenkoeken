using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.ViewModels.Users;

namespace SintLeoPannenkoeken.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private ILogger<UsersController> _logger;
        private ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(ILogger<UsersController> logger, ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _dbContext.Users.ToList()
                .Select(async user =>
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    return new UserViewModel(user, roles);
                })
                .Select(t => t.Result)
                .OrderBy(u => u.Email)
                .ToList();
            return View(new IndexViewModel(users));
        }
    }
}
