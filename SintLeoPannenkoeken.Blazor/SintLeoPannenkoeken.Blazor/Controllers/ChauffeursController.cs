using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Blazor.Client.Auth;
using SintLeoPannenkoeken.Blazor.Client.Server;

namespace SintLeoPannenkoeken.Blazor.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = $"{Roles.RolesForGebruikers}")]
    [ApiController]
    public class ChauffeursController : ControllerBase
    {
        private readonly ILogger<ChauffeursController> _logger;
        private readonly IServerData _serverData;

        public ChauffeursController(ILogger<ChauffeursController> logger, IServerData serverData)
        {
            _logger = logger;
            _serverData = serverData;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var chauffeurDtos = await _serverData.GetChauffeurs();
            return Ok(chauffeurDtos);
        }
    }
}
