using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.ViewModels.Bestellingen;

namespace SintLeoPannenkoeken.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,FinanciePloeg,ApiClient")]
    [ApiController]
    public class BestellingenController : ControllerBase
    {
        private readonly ILogger<BestellingenController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public BestellingenController(ILogger<BestellingenController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("{jaar:int}")]
        public IActionResult Get(int jaar)
        {
            var scoutsjaar = _dbContext.Scoutsjaren.SingleOrDefault(s => s.Begin == jaar);
            if (scoutsjaar == null)
            {
                return BadRequest("Onbekend scoutsjaar");
            }

            var bestellingen = _dbContext.Scoutsjaren
                .Include(scoutsjaar => scoutsjaar.Bestellingen)
                .ThenInclude(bestelling => bestelling.Straat)
                .ThenInclude(straat => straat.Zone)
                .Include(scoutsjaar => scoutsjaar.Bestellingen)
                .ThenInclude(bestelling => bestelling.Lid)
                .ThenInclude(lid => lid.Tak)
                .SingleOrDefault(scoutsjaar => scoutsjaar.Begin == jaar)
                ?.Bestellingen
                ?.ToList();

            var bestellingenViewModels = bestellingen == null 
                ? new List<BestellingViewModel>() 
                : bestellingen.Select(bestelling => new BestellingViewModel(bestelling)).ToList();

            var bestellingenViewModel = new BestellingenViewModel(bestellingenViewModels, scoutsjaar.PannenkoekenPerPak);

            return Ok(bestellingenViewModel);
        }

        [HttpGet]
        [Route("{jaar:int}/{id:int}")]
        public IActionResult GetBestelling(int jaar, int id)
        {
            var scoutsjaar = _dbContext.Scoutsjaren.SingleOrDefault(s => s.Begin == jaar);
            if (scoutsjaar == null)
            {
                return BadRequest("Onbekend scoutsjaar");
            }

            var bestelling = _dbContext.Scoutsjaren
                .Include(scoutsjaar => scoutsjaar.Bestellingen)
                .ThenInclude(bestelling => bestelling.Straat)
                .ThenInclude(straat => straat.Zone)
                .Include(scoutsjaar => scoutsjaar.Bestellingen)
                .ThenInclude(bestelling => bestelling.Lid)
                .ThenInclude(lid => lid.Tak)
                .SingleOrDefault(scoutsjaar => scoutsjaar.Begin == jaar)
                ?.Bestellingen
                .SingleOrDefault(bestelling => bestelling.Id == id);

            if (bestelling == null)
            {
                return BadRequest("Onbekende bestelling");
            }

            var bestellingViewModel = new BestellingViewModel(bestelling);

            return Ok(bestellingViewModel);
        }

        [HttpPost]
        [Route("{jaar:int}")]
        [Authorize(AuthenticationSchemes = "Identity.Application,ApiKey", Roles = "Admin,FinanciePloeg,ApiClient")]
        public IActionResult Post(int jaar, [FromBody] CreateBestellingViewModel createBestellingViewModel)
        {
            var scoutsjaar = _dbContext.Scoutsjaren.SingleOrDefault(s => s.Begin == jaar);
            if (scoutsjaar == null)
            {
                return BadRequest("Onbekend scoutsjaar");
            }

            var takId = _dbContext.Leden.Single(lid => lid.Id == createBestellingViewModel.LidId).TakId;

            var lastBestelling = _dbContext.Bestellingen
                .Where(b => b.ScoutsjaarId == scoutsjaar.Id)
                .OrderByDescending(b => b.Id)
                .FirstOrDefault();

            // I totally realize this could lead to duplicate bestellingNummers, but the chances are slim
            // because only about 2 users (maximum) will be active at the same time.
            var bestellingNummer = lastBestelling != null ? lastBestelling.BestellingNummer + 1 : 1;

            var bestelling = new Bestelling(createBestellingViewModel.Naam, createBestellingViewModel.AantalPakken)
            {
                BestellingNummer = bestellingNummer,
                IngaveDatum = DateTime.Now,
                Telefoon = createBestellingViewModel.Telefoon != null ? createBestellingViewModel.Telefoon : "",
                Opmerkingen = createBestellingViewModel.Opmerkingen != null ? createBestellingViewModel.Opmerkingen : "",
                Betaald = createBestellingViewModel.Betaald,
                Geleverd = createBestellingViewModel.Geleverd,
                LidId = createBestellingViewModel.LidId,
                TakId = takId,
                StraatId = createBestellingViewModel.StraatId,
                Nummer = createBestellingViewModel.Nummer,
                Bus = createBestellingViewModel.Bus
            };

            scoutsjaar.Bestellingen.Add(bestelling);
            _dbContext.SaveChanges();
            bestelling = _dbContext.Bestellingen
                .Include(b => b.Tak)
                .Include(b => b.Lid)
                .Include(b => b.Straat)
                .Single(b => b.Id == bestelling.Id);

            var bestellingViewModel = new BestellingViewModel(bestelling);
            return Created($"/api/bestellingen/{scoutsjaar}/{bestelling.Id}", bestellingViewModel);
        }

        [HttpDelete]
        [Route("{jaar:int}/{id:int}")]
        public IActionResult Delete(int jaar, int id)
        {
            var bestelling = new Bestelling("", 0) { Id = id };
            _dbContext.Bestellingen.Attach(bestelling);
            _dbContext.Bestellingen.Remove(bestelling);
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpPut]
        [Route("{jaar:int}/{bestellingId:int}")]
        public IActionResult Put(int jaar, int bestellingId, [FromBody] UpdateBestellingViewModel updateBestellingViewModel)
        {
            var scoutsjaar = _dbContext.Scoutsjaren.SingleOrDefault(s => s.Begin == jaar);
            if (scoutsjaar == null)
            {
                return BadRequest("Onbekend scoutsjaar");
            }

            var bestelling = _dbContext.Bestellingen.SingleOrDefault(bestelling => bestelling.Id == bestellingId);
            if (bestelling == null)
            {
                return BadRequest("Onbekende bestelling");
            }

            var takId = _dbContext.Leden.Single(lid => lid.Id == updateBestellingViewModel.LidId).TakId;

            bestelling.Naam = updateBestellingViewModel.Naam;
            bestelling.AantalPakken = updateBestellingViewModel.AantalPakken;
            bestelling.Telefoon = updateBestellingViewModel.Telefoon != null ? updateBestellingViewModel.Telefoon : "";
            bestelling.Opmerkingen = updateBestellingViewModel.Opmerkingen != null ? updateBestellingViewModel.Opmerkingen : "";
            bestelling.Betaald = updateBestellingViewModel.Betaald;
            bestelling.Geleverd = updateBestellingViewModel.Geleverd;
            bestelling.LidId = updateBestellingViewModel.LidId;
            bestelling.TakId = takId;
            bestelling.StraatId = updateBestellingViewModel.StraatId;
            bestelling.Nummer = updateBestellingViewModel.Nummer;
            bestelling.Bus = updateBestellingViewModel.Bus;

            _dbContext.SaveChanges();
            bestelling = _dbContext.Bestellingen
                .Include(b => b.Tak)
                .Include(b => b.Lid)
                .Include(b => b.Straat)
                .Single(b => b.Id == bestelling.Id);

            var bestellingViewModel = new BestellingViewModel(bestelling);
            return NoContent();
        }

        [HttpPost]
        [Route("{bestellingId:int}/delivery")]
        public async Task<IActionResult> Delivery(int bestellingId, [FromBody] UpdateGeleverdViewModel viewModel)
        {
            var bestelling = await _dbContext.Bestellingen.SingleOrDefaultAsync(b => b.Id == bestellingId);
            if (bestelling == null)
            {
                return NotFound();
            }

            bestelling.Geleverd = viewModel.Geleverd;
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
