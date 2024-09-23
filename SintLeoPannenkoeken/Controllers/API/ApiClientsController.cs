using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.ViewModels.ApiClients;

namespace SintLeoPannenkoeken.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class ApiClientsController : ControllerBase
    {
        private readonly ILogger<ApiClientsController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApiClientsController(
            ILogger<ApiClientsController> logger, 
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
        public async Task<IActionResult> Get()
        {
            var apiClients = await _dbContext.ApiClients.ToListAsync();

            var apiClientViewModels = apiClients == null
            ? new List<ApiClientViewModel>()
                : apiClients.Select(apiClient => new ApiClientViewModel(apiClient)).ToList();

            return Ok(apiClientViewModels);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post([FromBody] CreateApiClientViewModel createApiClientViewModel)
        {
            var passwordHasher = new PasswordHasher<IdentityUser>();
            // The user parameter isn't used (see https://github.com/dotnet/AspNetCore/blob/main/src/Identity/Extensions.Core/src/PasswordHasher.cs)
            var hashedApiKey = passwordHasher.HashPassword(null, createApiClientViewModel.ApiKey);

            var apiClient = new ApiClient(createApiClientViewModel.Naam, hashedApiKey);

            _dbContext.ApiClients.Add(apiClient);
            await _dbContext.SaveChangesAsync();

            apiClient = _dbContext.ApiClients.Single(a => a.Id == apiClient.Id);
            var apiClientViewModel = new ApiClientViewModel(apiClient);
            return Created($"/api/apiClients/{apiClient.Id}", apiClientViewModel);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete(int id)
        {
            var apiClient = _dbContext.ApiClients.SingleOrDefault(a => a.Id == id);
            if (apiClient == null)
            {
                return NotFound();
            }

            _dbContext.ApiClients.Remove(apiClient);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
