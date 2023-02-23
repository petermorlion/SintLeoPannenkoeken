using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
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
    }
}
