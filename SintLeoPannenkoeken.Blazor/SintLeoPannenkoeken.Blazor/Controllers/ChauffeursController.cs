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
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var chauffeurDtos = await _serverData.GetChauffeurs();
            return Ok(chauffeurDtos);
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
        public async Task<IActionResult> UpdateChauffeur(int id)
        {
            await _serverData.DeleteChauffeur(id);
            return Ok();
        }
    }
}
