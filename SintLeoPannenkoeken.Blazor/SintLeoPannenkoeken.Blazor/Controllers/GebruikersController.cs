using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Blazor.Client.Auth;
using SintLeoPannenkoeken.Blazor.Client.Server;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;

namespace SintLeoPannenkoeken.Blazor.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = $"{Roles.RolesForGebruikers}")]
    [ApiController]
    public class GebruikersController : ControllerBase
    {
        private readonly ILogger<GebruikersController> _logger;
        private readonly IServerData _serverData;

        public GebruikersController(ILogger<GebruikersController> logger, IServerData serverData)
        {
            _logger = logger;
            _serverData = serverData;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var gebruikerDtos = await _serverData.GetGebruikers();
            return Ok(gebruikerDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGebruiker([FromBody] NewGebruikerDto gebruikerDto)
        {
            var result = await _serverData.CreateGebruiker(gebruikerDto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGebruiker([FromBody] GebruikerDto gebruikerDto)
        {
            await _serverData.UpdateGebruiker(gebruikerDto);
            return Ok();
        }

        [HttpDelete]
        [Route("{email}")]
        public async Task<IActionResult> DeleteGebruiker([FromRoute] string email)
        {
            await _serverData.DeleteGebruiker(email);
            return Ok();
        }

        [HttpGet]
        [Route("{email}/passwordResetCode")]
        public async Task<IActionResult> GetPasswordResetCode([FromRoute] string email)
        {
            var code = await _serverData.GetPasswordResetCode(email);
            return Ok(code);
        }
    }
}
