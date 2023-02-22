using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.ViewModels.Bestuurders;

namespace SintLeoPannenkoeken.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BestuurdersController : ControllerBase
    {
        private readonly ILogger<BestuurdersController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public BestuurdersController(ILogger<BestuurdersController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            var bestuurders = _dbContext.Bestuurders.ToList();

            var bestuurderViewModels = bestuurders == null 
                ? new List<BestuurderViewModel>() 
                : bestuurders.Select(bestuurder => new BestuurderViewModel(bestuurder)).ToList();

            return Ok(bestuurderViewModels);
        }

        [HttpPost]
        [Route("")]
        public IActionResult Post([FromBody] CreateBestuurderViewModel createBestuurderViewModel)
        {
            var bestuurder = new Bestuurder
            {
                Voornaam = createBestuurderViewModel.Voornaam,
                Achternaam = createBestuurderViewModel.Achternaam
            };

            _dbContext.Bestuurders.Add(bestuurder);
            _dbContext.SaveChanges();

            bestuurder = _dbContext.Bestuurders.Single(b => b.Id == bestuurder.Id);
            var bestuurderViewModel = new BestuurderViewModel(bestuurder);
            return Created($"/api/bestuurders/{bestuurder.Id}", bestuurderViewModel);
        }

        [HttpGet]
        [Route("{id:int}/rondes/{scoutsjaar:int}")]
        public IActionResult BestuurderRondes(int id, int scoutsjaar)
        {
            var rondes = _dbContext.Scoutsjaren
                .Where(sj => sj.Begin == scoutsjaar)
                .SelectMany(scoutsjaar => scoutsjaar.Rondes)
                .Include(ronde => ronde.Zone)
                .ThenInclude(zone => zone.Straten)
                .Include(ronde => ronde.Bestuurder)
                .Where(ronde => ronde.BestuurderId == id)
                .ToList();

            var zoneIds = rondes.Select(ronde => ronde.ZoneId).ToList();

            var bestellingen = _dbContext.Scoutsjaren
                .Where(sj => sj.Begin == scoutsjaar)
                .SelectMany(scoutsjaar => scoutsjaar.Bestellingen)
                .Include(bestelling => bestelling.Straat)
                .Where(bestelling => zoneIds.Contains(bestelling.Straat.ZoneId));

            var rondeViewModels = rondes.Select(ronde =>
            {
                var aantalPakken = bestellingen
                    .Where(bestelling => bestelling.Straat.ZoneId == ronde.ZoneId)
                    .Select(bestelling => bestelling.AantalPakken)
                    .Sum();

                return new RondeViewModel(ronde, aantalPakken);
            });

            return Ok(rondeViewModels);
        }

        [HttpPost]
        [Route("{id:int}/rondes/{scoutsjaar:int}")]
        public IActionResult BestuurderRondes(int id, int scoutsjaar, [FromBody] AddZoneToBestuurderViewModel addZoneToBestuurderViewModel)
        {
            var scoutsjaarModel = _dbContext.Scoutsjaren.Single(sj => sj.Begin == scoutsjaar);
            var scoutsjaarId = scoutsjaarModel.Id;
            var ronde = new Ronde
            {
                BestuurderId = id,
                ZoneId = addZoneToBestuurderViewModel.ZoneId,
                ScoutsjaarId = scoutsjaarId
            };

            var existing = _dbContext.Rondes
                .Include(ronde => ronde.Bestuurder)
                .SingleOrDefault(ronde => ronde.ScoutsjaarId == scoutsjaarId 
                    && ronde.ZoneId == addZoneToBestuurderViewModel.ZoneId);
            if (existing != null)
            {
                return BadRequest("Zone al toegekend aan " + existing.Bestuurder.Achternaam + " " + existing.Bestuurder.Voornaam);
            }

            _dbContext.Rondes.Add(ronde);
            _dbContext.SaveChanges();

            ronde = _dbContext.Rondes
                .Include(ronde => ronde.Zone)
                .ThenInclude(zone => zone.Straten)
                .Include(ronde => ronde.Bestuurder)
                .Single(r => r.Id == ronde.Id);

            var bestellingen = _dbContext.Scoutsjaren
                .Where(sj => sj.Begin == scoutsjaar)
                .SelectMany(scoutsjaar => scoutsjaar.Bestellingen)
                .Include(bestelling => bestelling.Straat)
                .Where(bestelling => bestelling.Straat.ZoneId == addZoneToBestuurderViewModel.ZoneId);

            var aantalPakken = bestellingen
                    .Where(bestelling => bestelling.Straat.ZoneId == ronde.ZoneId)
                    .Select(bestelling => bestelling.AantalPakken)
                    .Sum();

            var rondeViewModel = new RondeViewModel(ronde, aantalPakken);
            return Created($"/api/bestuurders/{id}/rondes/{scoutsjaar}/{ronde.Id}", rondeViewModel);
        }
    }
}
