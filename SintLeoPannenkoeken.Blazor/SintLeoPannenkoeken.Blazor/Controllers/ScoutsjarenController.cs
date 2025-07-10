using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Blazor.Client.Server;

namespace SintLeoPannenkoeken.Blazor.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    [ApiController]
    public class ScoutsjarenController : ControllerBase
    {
        private readonly ILogger<ScoutsjarenController> _logger;
        private readonly IServerData _serverData;

        public ScoutsjarenController(ILogger<ScoutsjarenController> logger, IServerData serverData)
        {
            _logger = logger;
            _serverData = serverData;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var scoutsjaarDtos = await _serverData.GetScoutsjaren();
            return Ok(scoutsjaarDtos);
        }
    }
}
