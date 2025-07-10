using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Blazor.Client.Auth;
using SintLeoPannenkoeken.Blazor.Client.Server;

namespace SintLeoPannenkoeken.Blazor.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.FinanciePloeg}")]
    [ApiController]
    public class LedenController : ControllerBase
    {
        private readonly ILogger<LedenController> _logger;
        private readonly IServerData _serverData;

        public LedenController(ILogger<LedenController> logger, IServerData serverData)
        {
            _logger = logger;
            _serverData = serverData;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var lidDtos = await _serverData.GetLeden();
            return Ok(lidDtos);
        }
    }
}
