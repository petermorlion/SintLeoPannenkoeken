using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Blazor.Client.Auth;
using SintLeoPannenkoeken.Blazor.Client.Server;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;

namespace SintLeoPannenkoeken.Blazor.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.RolesForStreefcijfers)]
    [ApiController]
    public class StreefcijfersController : ControllerBase
    {
        private readonly ILogger<StreefcijfersController> _logger;
        private readonly IServerData _serverData;

        public StreefcijfersController(ILogger<StreefcijfersController> logger, IServerData serverData)
        {
            _logger = logger;
            _serverData = serverData;
        }

        [HttpGet]
        [Route("{scoutsjaar:int}")]
        public async Task<IActionResult> Get(int scoutsjaar)
        {
            var dtos = await _serverData.GetStreefcijfers(scoutsjaar);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStreefcijfer([FromBody] StreefcijferDto streefcijferDto)
        {
            var result = await _serverData.CreateStreefcijfer(streefcijferDto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStreefcijfer([FromBody] StreefcijferDto streefcijferDto)
        {
            await _serverData.UpdateStreefcijfer(streefcijferDto);
            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateStreefcijfer(int id)
        {
            await _serverData.DeleteStreefcijfer(id);
            return Ok();
        }
    }
}
