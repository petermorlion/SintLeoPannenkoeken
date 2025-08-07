using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Blazor.Client.Server;

namespace SintLeoPannenkoeken.Blazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TakkenController : ControllerBase
    {
        private readonly ILogger<TakkenController> _logger;
        private readonly IServerData _serverData;

        public TakkenController(ILogger<TakkenController> logger, IServerData serverData)
        {
            _logger = logger;
            _serverData = serverData;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var takDtos = await _serverData.GetTakken();
            return Ok(takDtos);
        }
    }
}
