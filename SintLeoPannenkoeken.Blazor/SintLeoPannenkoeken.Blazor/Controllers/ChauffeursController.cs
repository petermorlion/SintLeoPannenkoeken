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
    public class ChauffeursController : ControllerBase
    {
        private readonly ILogger<ChauffeursController> _logger;
        private readonly IServerData _serverData;

        public ChauffeursController(ILogger<ChauffeursController> logger, IServerData serverData)
        {
            _logger = logger;
            _serverData = serverData;
        }

        [HttpGet]
        [Route("{scoutsjaar:int}")]
        public async Task<IActionResult> Get(int scoutsjaar)
        {
            var chauffeurDtos = await _serverData.GetChauffeurs(scoutsjaar);
            return Ok(chauffeurDtos);
        }

        [HttpGet]
        [Route("{scoutsjaar:int}/{chauffeurId:int}")]
        public async Task<IActionResult> Get(int scoutsjaar, int chauffeurId)
        {
            var chauffeurDto = await _serverData.GetChauffeur(scoutsjaar, chauffeurId);
            return Ok(chauffeurDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChauffeur([FromBody] ChauffeurDto chauffeurDto)
        {
            var result = await _serverData.CreateChauffeur(chauffeurDto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateChauffeur([FromBody] ChauffeurDto chauffeurDto)
        {
            await _serverData.UpdateChauffeur(chauffeurDto);
            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteChauffeur(int id)
        {
            await _serverData.DeleteChauffeur(id);
            return Ok();
        }

        [HttpGet]
        [Route("{chauffeurId:int}/rondes/{scoutsjaarBegin:int}")]
        public async Task<IActionResult> GetRondes(int chauffeurId, int scoutsjaarBegin)
        {
            var result = await _serverData.GetRondesForChauffeur(chauffeurId, scoutsjaarBegin);
            return Ok(result);
        }

        [HttpPost]
        [Route("{chauffeurId:int}/rondes/{scoutsjaarBegin:int}")]
        public async Task<IActionResult> CreateRonde(int chauffeurId, int scoutsjaarBegin, [FromBody] CreateRondeDto rondeDto)
        {
            var result = await _serverData.CreateRonde(chauffeurId, scoutsjaarBegin, rondeDto);
            return Ok(result);
        }
    }
}
