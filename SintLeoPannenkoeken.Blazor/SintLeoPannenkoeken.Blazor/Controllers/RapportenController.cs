using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Blazor.Client.Auth;
using SintLeoPannenkoeken.Blazor.Client.Server;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;

namespace SintLeoPannenkoeken.Blazor.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = $"{Roles.RolesForRapporten}")]
    [ApiController]
    public class RapportenController : ControllerBase
    {
        private readonly ILogger<RapportenController> _logger;
        private readonly IServerData _serverData;

        public RapportenController(ILogger<RapportenController> logger, IServerData serverData)
        {
            _logger = logger;
            _serverData = serverData;
        }

        [HttpGet]
        [Route("{scoutsjaarBegin:int}/chaffeurrondedetails/{chauffeurId:int}")]
        public async Task<IActionResult> GetChauffeurRondeDetails(int scoutsjaarBegin, int chauffeurId)
        {
            var result = await _serverData.GetChauffeurRondeDetails(scoutsjaarBegin, chauffeurId);
            return Ok(result);
        }
    }
}
