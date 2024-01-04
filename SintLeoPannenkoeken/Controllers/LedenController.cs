using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.ViewModels.Leden;

namespace SintLeoPannenkoeken.Controllers
{
    [Authorize(Roles = "Admin,FinanciePloeg")]
    public class LedenController : Controller
    {
        private ILogger<LedenController> _logger;
        private ApplicationDbContext _dbContext;

        public LedenController(ILogger<LedenController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var takken = _dbContext.Takken.ToList();
            return View(new IndexViewModel(takken));
        }

        [HttpGet]
        public async Task<IActionResult> Import()
        {
            return View(new List<string>());
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile importFile)
        {
            var results = new List<string>();
            var lineCounter = 1;
            var importedCount = 0;

            var leden = _dbContext.Leden.ToList();
            var takken = _dbContext.Takken.ToList();

            using (var readStream = importFile.OpenReadStream())
            using (var stringReader = new StreamReader(readStream))
            {
                while (!stringReader.EndOfStream)
                {
                    try
                    {
                        var line = await stringReader.ReadLineAsync();
                        if (lineCounter == 1)
                        {
                            continue;
                        }

                        var parts = line.Split(line.Contains(";") ? ';' : ',');
                        var voornaam = parts[0].Transform(To.LowerCase, To.TitleCase);
                        var achternaam = parts[1].Transform(To.LowerCase, To.TitleCase);
                        var functies = parts[2];

                        var functiesParts = functies.Split('_');
                        var takNaam = functiesParts[1].Transform(To.LowerCase, To.TitleCase);
                        var takAfkorting = Tak.GetAfkorting(takNaam);
                        var functie = "";
                        if (functiesParts.Length > 2)
                        {
                            functie = functiesParts[2].Transform(To.LowerCase, To.TitleCase);
                        }

                        var tak = takken.SingleOrDefault(t => t.Afkorting == takAfkorting);
                        if (tak == null)
                        {
                            results.Add($"Fout op lijn {lineCounter}: Onbekende tak ({takNaam}).");
                            continue;
                        }

                        var lid = leden.SingleOrDefault(l => l.Voornaam == voornaam && l.Achternaam == achternaam);
                        if (lid == null)
                        {
                            lid = new Lid(voornaam, achternaam);
                            _dbContext.Leden.Add(lid);
                        }

                        lid.Functie = functie;
                        lid.TakId = tak.Id;

                        importedCount++;
                    } catch (Exception e)
                    {
                        results.Add($"Fout op lijn {lineCounter}: {e.Message}");
                    }
                    finally
                    {
                        lineCounter++;
                    }
                }
            }

            results.Insert(0, $"Er werden {importedCount} leden geïmporteerd.");

            _dbContext.SaveChanges();
                
            return View(results);
        }
    }
}
