using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.ViewModels.Rondes;

namespace SintLeoPannenkoeken.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class RondeController : ControllerBase
    {
        private readonly ILogger<RondeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public RondeController(ILogger<RondeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("{jaar:int}")]
        public IActionResult Get(int jaar)
        {
            var rondes = _dbContext.Scoutsjaren
                .Include(scoutsjaar => scoutsjaar.Rondes)
                .ThenInclude(ronde => ronde.Zone)
                .ThenInclude(zone => zone.Straten)
                .Include(scoutsjaar => scoutsjaar.Rondes)
                .ThenInclude(ronde => ronde.Bestuurder)
                .SingleOrDefault(scoutsjaar => scoutsjaar.Begin == jaar)
                ?.Rondes
                ?.ToList();

            var rondeViewModels = rondes == null 
                ? new List<RondeViewModel>() 
                : rondes.Select(ronde => new RondeViewModel(ronde)).ToList();

            return Ok(rondeViewModels);
        }

        [HttpPost]
        [Route("{jaar:int}")]
        public IActionResult Post(int jaar, [FromBody] CreateRondeViewModel createRondeViewModel)
        {
            var scoutsjaar = _dbContext.Scoutsjaren.SingleOrDefault(s => s.Begin == jaar);
            if (scoutsjaar == null)
            {
                return BadRequest("Onbekend scoutsjaar");
            }

            var ronde = new Ronde
            {
                ZoneId = createRondeViewModel.ZoneId,
                BestuurderId = createRondeViewModel.BestuurderId
            };

            scoutsjaar.Rondes.Add(ronde);
            _dbContext.SaveChanges();
            ronde = _dbContext.Rondes
                .Include(r => r.Bestuurder)
                .Include(r => r.Zone)
                .ThenInclude(z => z.Straten)
                .Single(r => r.Id == ronde.Id);

            var rondeViewModel = new RondeViewModel(ronde);
            return Created($"/api/rondes/{ronde.Id}", rondeViewModel);
        }

        [HttpDelete]
        [Route("{jaar:int}/{id:int}")]
        public IActionResult Delete(int jaar, int id)
        {
            var ronde = new Ronde { Id = id };
            _dbContext.Rondes.Attach(ronde);
            _dbContext.Rondes.Remove(ronde);
            _dbContext.SaveChanges();
            return NoContent();
        }
    }
}
