using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Filters;
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

        [TypeFilter(typeof(ScoutsjaarRedirectionFilter))]
        public IActionResult Index(int? scoutsjaar)
        {
            Scoutsjaar? sj = _dbContext.Scoutsjaren.SingleOrDefault(s => s.Begin == scoutsjaar);
            return View(scoutsjaar);
        }

        [TypeFilter(typeof(ScoutsjaarRedirectionFilter))]
        public async Task<IActionResult> ZoneDetails(int? scoutsjaar, int zoneId)
        {
            Scoutsjaar? sj = _dbContext.Scoutsjaren.SingleOrDefault(s => s.Begin == scoutsjaar);
            var bestellingen = await _dbContext.Bestellingen
                .Include(b => b.Straat)
                .Where(b => b.Straat.ZoneId == zoneId && b.ScoutsjaarId == sj.Id)
                .ToListAsync();

            var zone = await _dbContext.Zones.SingleOrDefaultAsync(z => z.Id == zoneId);
            var ronde = await _dbContext.Rondes.Include(r => r.Bestuurder).SingleOrDefaultAsync(r => r.ZoneId == zoneId);

            var viewModel = new ZoneDetailsViewModel
            {
                ScoutsjaarBegin = sj.Begin,
                ZoneNaam = zone != null ? zone.Naam : "",
                Bestuurder = ronde != null && ronde.Bestuurder != null ? $"{ronde.Bestuurder.Voornaam} {ronde.Bestuurder.Achternaam}" : "",
                Gemeente = zone != null ? zone.Gemeente : "",
                PostNummer = zone.PostNummer,
                Bestellingen = bestellingen.Select(b => new BestellingViewModel
                {
                    Naam = b.Naam,
                    Straat = b.Straat.Naam,
                    Nummer = b.Nummer,
                    Bus = b.Bus,
                    Aantal = b.AantalPakken
                }).ToList()
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

        [TypeFilter(typeof(ScoutsjaarRedirectionFilter))]
        public async Task<IActionResult> VerkoopPerTak(int? scoutsjaar)
        {
            Scoutsjaar? sj = _dbContext
                .Scoutsjaren
                .Include(x => x.StreefCijfers)
                .ThenInclude(x => x.Tak)
                .SingleOrDefault(s => s.Begin == scoutsjaar);

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

            var viewModel = new VerkoopPerTakViewModel(verkoopPerTak, streefcijfersPerTak, takken, sj.Begin);
            
            return View(viewModel);
        }

        [TypeFilter(typeof(ScoutsjaarRedirectionFilter))]
        public async Task<IActionResult> VerkoopPerLid(int? scoutsjaar)
        {
            Scoutsjaar? sj = _dbContext
                .Scoutsjaren
                .Include(x => x.StreefCijfers)
                .ThenInclude(x => x.Tak)
                .SingleOrDefault(s => s.Begin == scoutsjaar);
            
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

            return View(new VerkoopPerLidViewModel(verkoopPerLid, sj.Begin));
        }

        [TypeFilter(typeof(ScoutsjaarRedirectionFilter))]
        public async Task<IActionResult> IngaveTotalen(int? scoutsjaar)
        {
            Scoutsjaar? sj = _dbContext
                .Scoutsjaren
                .Include(x => x.StreefCijfers)
                .ThenInclude(x => x.Tak)
                .SingleOrDefault(s => s.Begin == scoutsjaar);
            
            var bestellingen = _dbContext.Scoutsjaren
                .Include(scoutsjaar => scoutsjaar.Bestellingen)
                .ThenInclude(bestelling => bestelling.Tak)
                .SingleOrDefault(scoutsjaar => scoutsjaar.Id == sj.Id)
                ?.Bestellingen
                ?.ToList();

            var bestellingenByDate = bestellingen
                .GroupBy(x => x.IngaveDatum)
                .OrderBy(x => x.Key);

            var ingaveTotalenViewModel = new IngaveTotalenViewModel
            {
                ScoutsjaarBegin = sj.Begin,
                Takken = _dbContext.Takken.OrderBy(x => x.Afkorting).ToList().Select(x => x.Afkorting).ToList(),
                IngaveTotalen = new List<IngaveTotalenRowViewModel>()
            };

            foreach (var bestellingenOnDate in bestellingenByDate)
            {
                var ingaveTotalenRowViewModel = new IngaveTotalenRowViewModel
                {
                    IngaveDatum = bestellingenOnDate.Key.Date,
                    AantalPerTak = new Dictionary<string, int>()
                };

                foreach (var bestelling in bestellingenOnDate)
                {
                    if (ingaveTotalenRowViewModel.AantalPerTak.ContainsKey(bestelling.Tak.Afkorting))
                    {
                        ingaveTotalenRowViewModel.AantalPerTak[bestelling.Tak.Afkorting] += bestelling.AantalPakken;
                    }
                    else
                    {
                        ingaveTotalenRowViewModel.AantalPerTak.Add(bestelling.Tak.Afkorting, bestelling.AantalPakken);
                    }
                }

                ingaveTotalenViewModel.IngaveTotalen.Add(ingaveTotalenRowViewModel);
            }

            return View(ingaveTotalenViewModel);
        }
    }
}
