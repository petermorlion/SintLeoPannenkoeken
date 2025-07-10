using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Blazor.Client.Server;

namespace SintLeoPannenkoeken.Blazor.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    [ApiController]
    public class LedenController : ControllerBase
    {
        private readonly ILogger<LedenController> _logger;
        private readonly IServerData _server;

        public LedenController(ILogger<LedenController> logger, IServerData server)
        {
            _logger = logger;
            _server = server;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var lidDtos = await _server.GetLeden();
            return Ok(lidDtos);
        }
    }
}
