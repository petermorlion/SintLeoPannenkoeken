using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Blazor.Client.Auth;
using SintLeoPannenkoeken.Blazor.Client.Server;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;

namespace SintLeoPannenkoeken.Blazor.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = $"{Roles.RolesForChauffeurs}")]
    [ApiController]
    public class RondesController : ControllerBase
    {
        private readonly ILogger<RondesController> _logger;
        private readonly IServerData _serverData;

        public RondesController(ILogger<RondesController> logger, IServerData serverData)
        {
            _logger = logger;
            _serverData = serverData;
        }

        [HttpGet]
        [Route("{scoutsjaarBegin:int}")]
        public async Task<IActionResult> GetRondes(int scoutsjaarBegin)
        {
            var result = await _serverData.GetRondes(scoutsjaarBegin);
            return Ok(result);
        }

        // POst



        [HttpDelete]
        [Route("{rondeId:int}")]
        public async Task<IActionResult> Delete(int rondeId)
        {
            await _serverData.DeleteRonde(rondeId);
            return Ok();
        }
    }
}
