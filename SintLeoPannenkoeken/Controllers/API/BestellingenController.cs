using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.ViewModels.Bestellingen;

namespace SintLeoPannenkoeken.Controllers.API
{
    [Route("api/[controller]")]
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

            return Ok(bestellingenViewModels);
        }

        [HttpPost]
        [Route("{jaar:int}")]
        public IActionResult Post(int jaar, [FromBody] CreateBestellingViewModel createBestellingViewModel)
        {
            var scoutsjaar = _dbContext.Scoutsjaren.SingleOrDefault(s => s.Begin == jaar);
            if (scoutsjaar == null)
            {
                return BadRequest("Onbekend scoutsjaar");
            }

            var takId = _dbContext.Leden.Single(lid => lid.Id == createBestellingViewModel.LidId).TakId;

            var bestelling = new Bestelling(createBestellingViewModel.Naam, createBestellingViewModel.AantalPakken)
            {
                Telefoon = createBestellingViewModel.Telefoon != null ? createBestellingViewModel.Telefoon : "",
                Opmerkingen = createBestellingViewModel.Opmerkingen != null ? createBestellingViewModel.Opmerkingen : "",
                Betaald = createBestellingViewModel.Betaald,
                LidId = createBestellingViewModel.LidId,
                TakId = takId,
                StraatId = createBestellingViewModel.StraatId,
                Nummer = createBestellingViewModel.Nummer
            };

            scoutsjaar.Bestellingen.Add(bestelling);
            _dbContext.SaveChanges();
            bestelling = _dbContext.Bestellingen
                .Include(b => b.Tak)
                .Include(b => b.Lid)
                .Include(b => b.Straat)
                .Single(b => b.Id == bestelling.Id);

            var bestellingViewModel = new BestellingViewModel(bestelling);
            return Created($"/api/bestellingen/{bestelling.Id}", bestellingViewModel);
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
    }
}
