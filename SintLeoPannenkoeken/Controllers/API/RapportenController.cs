using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.ViewModels.Rapporten;

namespace SintLeoPannenkoeken.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,FinanciePloeg")]
    [ApiController]
    public class RapportenController : ControllerBase
    {
        private readonly ILogger<RapportenController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public RapportenController(ILogger<RapportenController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("zonetotalen/{jaar:int}/{zoneId:int}")]
        public async Task<IActionResult> Get(int jaar, int zoneId)
        {
            Scoutsjaar sj = _dbContext.Scoutsjaren.Single(s => s.Begin == jaar);
            var bestellingen = await _dbContext.Bestellingen
                .Include(b => b.Straat)
                .Where(b => b.Straat.ZoneId == zoneId && b.ScoutsjaarId == sj.Id)
                .ToListAsync();

            var zone = await _dbContext.Zones.SingleOrDefaultAsync(z => z.Id == zoneId);
            var ronde = await _dbContext.Rondes.Include(r => r.Bestuurder).SingleOrDefaultAsync(r => r.ZoneId == zoneId);

            var viewModel = new ZoneTotalenViewModel
            {
                AantalPakken = bestellingen.Sum(x => x.AantalPakken),
                AantalPakkenGeleverd = bestellingen.Where(x => x.Geleverd).Sum(x => x.AantalPakken),
                AantalPakkenNietGeleverd = bestellingen.Where(x => !x.Geleverd).Sum(x => x.AantalPakken)
            };

            return Ok(viewModel);
        }
    }
}
