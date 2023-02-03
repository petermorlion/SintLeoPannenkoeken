using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.ViewModels.Users;

namespace SintLeoPannenkoeken.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(
            ILogger<UsersController> logger, 
            ApplicationDbContext dbContext, 
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            var users = _dbContext.Users.ToList();

            var userViewModels = users == null 
                ? new List<UserViewModel>() 
                : users.Select(user => new UserViewModel(user)).ToList();

            return Ok(userViewModels);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post([FromBody] CreateUserViewModel createUserViewModel)
        {
            var user = new IdentityUser(createUserViewModel.Email);
            user.Email = createUserViewModel.Email;
            user.EmailConfirmed = true;
            // TODO: check for result?
            var userResult = await _userManager.CreateAsync(user, createUserViewModel.Password);
            if (!userResult.Succeeded)
            {
                // TODO: handle error
                return BadRequest(userResult.Errors);
            }

            if (createUserViewModel.Admin)
            {
                var adminRoleExists = await _roleManager.RoleExistsAsync("Admin");
                if (!adminRoleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                var roleResult = await _userManager.AddToRoleAsync(user, "Admin");
                if (!roleResult.Succeeded)
                {
                    // TODO: handle error
                    return BadRequest(roleResult.Errors);
                }
            }

            return Created($"/api/users/{user.Id}", user);
        }
    }
}
