using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.ViewModels.Straten;

namespace SintLeoPannenkoeken.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,FinanciePloeg")]
    [ApiController]
    public class StratenController : ControllerBase
    {
        private readonly ILogger<StratenController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public StratenController(ILogger<StratenController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            var straten = _dbContext.Straten.Include(straat => straat.Zone).ToList();

            var stratenViewModels = straten == null 
                ? new List<StraatViewModel>() 
                : straten.Select(straat => new StraatViewModel(straat)).ToList();

            return Ok(stratenViewModels);
        }

        [HttpPost]
        [Route("")]
        public IActionResult Put([FromBody] CreateStraatViewModel createStraatViewModel)
        {
            var straat = new Straat(createStraatViewModel.Naam, createStraatViewModel.Gemeente)
            {
                PostNummer = createStraatViewModel.PostNummer,
                Omschrijving = createStraatViewModel.Omschrijving,
                Nummer = createStraatViewModel.Nummer,
                ZoneId = createStraatViewModel.ZoneId
            };

            _dbContext.Straten.Add(straat);
            _dbContext.SaveChanges();

            straat = _dbContext.Straten.Include(s => s.Zone).Single(s => s.Id == straat.Id);
            var straatViewModel = new StraatViewModel(straat);
            return Created($"/api/straten/{straat.Id}", straatViewModel);
        }
    }
}
