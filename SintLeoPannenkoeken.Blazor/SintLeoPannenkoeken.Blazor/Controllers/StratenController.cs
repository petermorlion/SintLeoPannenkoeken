using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Blazor.Client.Auth;
using SintLeoPannenkoeken.Blazor.Client.Server;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;

namespace SintLeoPannenkoeken.Blazor.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.RolesForScoutsjaren)]
    [ApiController]
    public class StratenController : ControllerBase
    {
        private readonly ILogger<StratenController> _logger;
        private readonly IServerData _serverData;

        public StratenController(ILogger<StratenController> logger, IServerData serverData)
        {
            _logger = logger;
            _serverData = serverData;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStraat([FromBody] StraatDto straatDto)
        {
            var result = await _serverData.CreateStraat(straatDto);
            return Ok(result);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var straatDtos = await _serverData.GetStraten();
            return Ok(straatDtos);
        }
    }
}
