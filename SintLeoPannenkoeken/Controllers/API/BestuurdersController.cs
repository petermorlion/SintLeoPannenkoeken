using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.ViewModels.Bestuurders;

namespace SintLeoPannenkoeken.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,FinanciePloeg")]
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
        [Route("{scoutsjaar:int}")]
        public IActionResult Get(int scoutsjaar)
        {
            var bestuurders = _dbContext.Bestuurders.ToList();
            var scoutsjaarModel = _dbContext.Scoutsjaren.Single(sj => sj.Begin == scoutsjaar);

            var bestellingenPerZone = _dbContext.Scoutsjaren
                .Include(sj => sj.Bestellingen)
                .ThenInclude(b => b.Straat)
                .Single(sj => sj.Begin == scoutsjaar)
                .Bestellingen
                .GroupBy(b => b.Straat.ZoneId)
                .ToDictionary(group => group.Key, group => group.ToList());

            var zonesPerBestuurder = _dbContext.Rondes
                .Where(r => r.ScoutsjaarId == scoutsjaarModel.Id)
                .ToList()
                .GroupBy(r => r.BestuurderId, r => r.ZoneId)
                .ToDictionary(group => group.Key, group => group.ToList());

            var bestuurderViewModels = bestuurders == null 
                ? new List<BestuurderMetAantallenViewModel>() 
                : bestuurders.Select(bestuurder =>
                {
                    var aantalPakken = 0;
                    var aantalBestellingen = 0;
                    var zonesForBestuurder = zonesPerBestuurder.ContainsKey(bestuurder.Id) ? zonesPerBestuurder[bestuurder.Id] : new List<int>();
                    foreach(var zoneId in zonesForBestuurder)
                    {
                        var bestellingenForZone = bestellingenPerZone.ContainsKey(zoneId) ? bestellingenPerZone[zoneId] : new List<Bestelling>();
                        aantalPakken += bestellingenForZone.Sum(x => x.AantalPakken);
                        aantalBestellingen += bestellingenForZone.Count;
                    }

                    return new BestuurderMetAantallenViewModel(bestuurder, aantalPakken, aantalBestellingen);
                }).ToList();

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

        [HttpDelete]
        [Route("{bestuurdersId:int}/rondes/{scoutsjaar:int}/{id:int}")]
        public IActionResult Delete(int jaar, int bestuurdersId, int id)
        {
            var ronde = new Ronde { Id = id };
            _dbContext.Rondes.Attach(ronde);
            _dbContext.Rondes.Remove(ronde);
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete]
        [Route("{bestuurdersId:int}")]
        public async Task<IActionResult> Delete(int bestuurdersId)
        {
            var bestuurder = await _dbContext.Bestuurders.SingleOrDefaultAsync(b => b.Id == bestuurdersId);
            if (bestuurder == null)
            {
                return NotFound();
            }

            _dbContext.Bestuurders.Attach(bestuurder);
            _dbContext.Bestuurders.Remove(bestuurder);
            _dbContext.SaveChanges();
            return NoContent();
        }
    }
}
