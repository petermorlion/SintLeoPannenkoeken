using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Blazor.Client.Auth;
using SintLeoPannenkoeken.Blazor.Client.Server;

namespace SintLeoPannenkoeken.Blazor.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = $"{Roles.RolesForGebruikers}")]
    [ApiController]
    public class GebruikersController : ControllerBase
    {
        private readonly ILogger<GebruikersController> _logger;
        private readonly IServerData _serverData;

        public GebruikersController(ILogger<GebruikersController> logger, IServerData serverData)
        {
            _logger = logger;
            _serverData = serverData;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var gebruikerDtos = await _serverData.GetGebruikers();
            return Ok(gebruikerDtos);
        }
    }
}
