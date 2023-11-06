using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.Models.Views;
using SintLeoPannenkoeken.ViewModels.Rapporten;

namespace SintLeoPannenkoeken.Controllers
{
    public class RapportenController : Controller
    {
        private readonly ILogger<RapportenController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public RapportenController(ILogger<RapportenController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ZoneDetails(string zoneNaam)
        {
            var connection = _dbContext.Database.GetDbConnection();
            var command = new CommandDefinition(
                "SELECT " +
                    "[bestelnummer], " +
                    "[zone_naam], " +
                    "[zone_gemeente], " +
                    "[zone_postnummer], " +
                    "[zone_omschrijving], " +
                    "[KaartNummer], " +
                    "[bestuurder], " +
                    "[bestuurder_id], " +
                    "[huisnummer], " +
                    "[Bus], " +
                    "[Naam], " +
                    "[Opmerkingen], " +
                    "[AantalPakken], " +
                    "[Telefoon], " +
                    "[straatnaam], " +
                    "[Postcode], " +
                    "[straat_gemeente], " +
                    "[straat_omschrijving], " +
                    "[straatnummer] " +
                    "FROM [vw_rondes] " +
                    "WHERE [zone_naam] = @zoneNaam",
                new
                {
                    zoneNaam = zoneNaam
                }
            );

            var vwRondes = await connection.QueryAsync<VwRonde>(command);

            var viewModel = new ZoneDetailsViewModel
            {
                ZoneNaam = zoneNaam,
                VwRondes = vwRondes.ToList()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> BestuurderDetails(int bestuurderId)
        {
            var connection = _dbContext.Database.GetDbConnection();
            var command = new CommandDefinition(
                "SELECT " +
                    "[bestelnummer], " +
                    "[zone_naam], " +
                    "[zone_gemeente], " +
                    "[zone_postnummer], " +
                    "[zone_omschrijving], " +
                    "[KaartNummer], " +
                    "[bestuurder], " +
                    "[bestuurder_id], " +
                    "[huisnummer], " +
                    "[Bus], " +
                    "[Naam], " +
                    "[Opmerkingen], " +
                    "[AantalPakken], " +
                    "[Telefoon], " +
                    "[straatnaam], " +
                    "[Postcode], " +
                    "[straat_gemeente], " +
                    "[straat_omschrijving], " +
                    "[straatnummer] " +
                    "FROM [vw_rondes] " +
                    "WHERE [bestuurder_id] = @bestuurderId",
                new
                {
                    bestuurderId = bestuurderId
                }
            );

            var bestuurder = _dbContext.Bestuurders.Single(b => b.Id == bestuurderId);

            var vwRondes = await connection.QueryAsync<VwRonde>(command);

            var viewModel = new BestuurderDetailsViewModel
            {
                BestuurderNaam = bestuurder.Achternaam + " " + bestuurder.Voornaam,
                VwRondes = vwRondes.ToList()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> VerkoopPerTak(int? scoutsjaar)
        {
            if (scoutsjaar == null)
            {
                var currentScoutsjaar = _dbContext.Scoutsjaren.OrderByDescending(s => s.Begin).First();
                return Redirect($"/rapporten/verkooppertak?scoutsjaar={currentScoutsjaar.Begin}");
            }

            Scoutsjaar? sj = _dbContext
                .Scoutsjaren
                .Include(x => x.StreefCijfers)
                .ThenInclude(x => x.Tak)
                .SingleOrDefault(s => s.Begin == scoutsjaar);
            if (sj == null)
            {
                var currentScoutsjaar = _dbContext.Scoutsjaren.OrderByDescending(s => s.Begin).First();
                return Redirect($"/rapporten/verkooppertak?scoutsjaar={currentScoutsjaar.Begin}");
            }

            var bestellingen = _dbContext.Scoutsjaren
                .Include(scoutsjaar => scoutsjaar.Bestellingen)
                .ThenInclude(bestelling => bestelling.Tak)
                .SingleOrDefault(scoutsjaar => scoutsjaar.Id == sj.Id)
                ?.Bestellingen
                ?.ToList();

            var verkoopPerTak = bestellingen
                .GroupBy(x => x.Tak)
                .ToDictionary(x => x.Key.Id, x => x.Sum(y => y.AantalPakken));

            var streefcijfersPerTak = sj.StreefCijfers.ToDictionary(x => x.Tak.Id, x => x.Aantal);
            var takken = _dbContext.Takken.Include(x => x.Leden).ToList();

            var viewModel = new VerkoopPerTakViewModel(verkoopPerTak, streefcijfersPerTak, takken);
            
            return View(viewModel);
        }

        public async Task<IActionResult> VerkoopPerLid(int? scoutsjaar)
        {
            if (scoutsjaar == null)
            {
                var currentScoutsjaar = _dbContext.Scoutsjaren.OrderByDescending(s => s.Begin).First();
                return Redirect($"/rapporten/verkoopperlid?scoutsjaar={currentScoutsjaar.Begin}");
            }

            Scoutsjaar? sj = _dbContext
                .Scoutsjaren
                .Include(x => x.StreefCijfers)
                .ThenInclude(x => x.Tak)
                .SingleOrDefault(s => s.Begin == scoutsjaar);
            if (sj == null)
            {
                var currentScoutsjaar = _dbContext.Scoutsjaren.OrderByDescending(s => s.Begin).First();
                return Redirect($"/rapporten/verkoopperlid?scoutsjaar={currentScoutsjaar.Begin}");
            }

            var bestellingen = _dbContext.Scoutsjaren
                .Include(scoutsjaar => scoutsjaar.Bestellingen)
                .ThenInclude(bestelling => bestelling.Lid)
                .ThenInclude(lid => lid.Tak)
                .SingleOrDefault(scoutsjaar => scoutsjaar.Id == sj.Id)
                ?.Bestellingen
                ?.ToList();

            var verkoopPerLid = bestellingen
                .GroupBy(x => x.Lid)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.AantalPakken));

            return View(new VerkoopPerLidViewModel(verkoopPerLid));
        }
    }
}
