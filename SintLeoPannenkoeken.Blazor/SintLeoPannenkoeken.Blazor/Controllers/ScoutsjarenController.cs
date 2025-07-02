using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;
using SintLeoPannenkoeken.Blazor.Data;

namespace SintLeoPannenkoeken.Blazor.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    [ApiController]
    public class ScoutsjarenController : ControllerBase
    {
        private readonly ILogger<ScoutsjarenController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public ScoutsjarenController(ILogger<ScoutsjarenController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            var scoutsjaren = _dbContext.Scoutsjaren.ToList();

            var scoutsjaarViewModels = scoutsjaren == null 
                ? new List<ScoutsjaarDto>() 
                : scoutsjaren.Select(scoutsjaar => new ScoutsjaarDto(scoutsjaar.Begin, scoutsjaar.PannenkoekenPerPak)).ToList();

            return Ok(scoutsjaarViewModels);
        }
    }
}
