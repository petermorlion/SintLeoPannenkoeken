using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Blazor.Client.Auth;
using SintLeoPannenkoeken.Blazor.Client.Server;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;

namespace SintLeoPannenkoeken.Blazor.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = $"{Roles.RolesForBestellingen}")]
    [ApiController]
    public class BestellingenController : ControllerBase
    {
        private readonly ILogger<BestellingenController> _logger;
        private readonly IServerData _serverData;

        public BestellingenController(ILogger<BestellingenController> logger, IServerData serverData)
        {
            _logger = logger;
            _serverData = serverData;
        }

        [HttpGet]
        [Route("{scoutsjaar:int}")]
        public async Task<IActionResult> Get(int scoutsjaar)
        {
            var bestellingeDtos = await _serverData.GetBestellingen(scoutsjaar);
            return Ok(bestellingeDtos);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewBestellingDto bestellingDto)
        {
            var result = await _serverData.CreateBestelling(bestellingDto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateBestellingDto bestellingDto)
        {
            await _serverData.UpdateBestelling(bestellingDto);
            return NoContent();
        }
    }
}
