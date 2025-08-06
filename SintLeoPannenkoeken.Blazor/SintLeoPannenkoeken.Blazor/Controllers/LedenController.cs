using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Blazor.Client.Auth;
using SintLeoPannenkoeken.Blazor.Client.Server;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;

namespace SintLeoPannenkoeken.Blazor.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = $"{Roles.RolesForLeden}")]
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
        public async Task<IActionResult> Get()
        {
            var lidDtos = await _serverData.GetLeden();
            return Ok(lidDtos);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLid([FromBody] LidDto lidDto)
        {
            var result = await _serverData.UpdateLid(lidDto);
            return Ok(result);
        }
    }
}
