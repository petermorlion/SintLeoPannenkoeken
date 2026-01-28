using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Blazor.Client.Auth;
using SintLeoPannenkoeken.Blazor.Client.Server;

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

        [HttpGet]
        [Route("{scoutsjaarBegin:int}/verkooppertak")]
        public async Task<IActionResult> GetVerkoopPerTakRapport(int scoutsjaarBegin)
        {
            var result = await _serverData.GetVerkoopPerTakRapport(scoutsjaarBegin);
            return Ok(result);
        }

        [HttpGet]
        [Route("{scoutsjaarBegin:int}/verkoopperlid")]
        public async Task<IActionResult> GetVerkoopPerLidRapport(int scoutsjaarBegin)
        {
            var result = await _serverData.GetVerkoopPerLidRapport(scoutsjaarBegin);
            return Ok(result);
        }

        [HttpGet]
        [Route("{scoutsjaarBegin:int}/ingavetotalen")]
        public async Task<IActionResult> GetIngaveTotalen(int scoutsjaarBegin)
        {
            var result = await _serverData.GetIngaveTotalen(scoutsjaarBegin);
            return Ok(result);
        }

        [HttpGet]
        [Route("{scoutsjaarBegin:int}/verkoopverdelingperpostcode")]
        public async Task<IActionResult> GetVerkoopVerdelingPerPostcode(int scoutsjaarBegin)
        {
            var result = await _serverData.GetVerkoopVerdelingPerPostcode(scoutsjaarBegin);
            return Ok(result);
        }
    }
}
