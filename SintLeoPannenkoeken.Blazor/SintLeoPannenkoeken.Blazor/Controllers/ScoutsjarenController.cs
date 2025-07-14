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

        [HttpPut]
        [Route("{begin:int}")]
        public async Task<IActionResult> UpdateScoutsjaar(int begin, [FromBody] ScoutsjaarDto scoutsjaarDto)
        {
            if (scoutsjaarDto == null || scoutsjaarDto.Begin != begin)
            {
                return BadRequest("Invalid Scoutsjaar data.");
            }

            await _serverData.UpdateScoutsjaar(scoutsjaarDto);
            return NoContent();
        }
    }
}
