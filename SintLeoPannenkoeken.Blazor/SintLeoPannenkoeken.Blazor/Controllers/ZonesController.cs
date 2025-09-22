using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Blazor.Client.Auth;
using SintLeoPannenkoeken.Blazor.Client.Server;

namespace SintLeoPannenkoeken.Blazor.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = $"{Roles.RolesForBeheer}")]
    [ApiController]
    public class ZonesController : ControllerBase
    {
        private readonly ILogger<ZonesController> _logger;
        private readonly IServerData _serverData;

        public ZonesController(ILogger<ZonesController> logger, IServerData serverData)
        {
            _logger = logger;
            _serverData = serverData;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var zoneDtos = await _serverData.GetZones();
            return Ok(zoneDtos);
        }
    }
}
