using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.ViewModels.Leden;

namespace SintLeoPannenkoeken.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,FinanciePloeg")]
    [ApiController]
    public class LedenController : ControllerBase
    {
        private readonly ILogger<LedenController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public LedenController(ILogger<LedenController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            var leden = _dbContext.Leden.Include(lid => lid.Tak).ToList();

            var ledenViewModels = leden == null 
                ? new List<LidViewModel>() 
                : leden.Select(lid => new LidViewModel(lid)).ToList();

            return Ok(ledenViewModels);
        }

        [HttpPost]
        [Route("")]
        public IActionResult Put([FromBody] CreateLidViewModel createLidViewModel)
        {
            var lid = new Lid(createLidViewModel.Voornaam, createLidViewModel.Achternaam)
            {
                Functie = createLidViewModel.Functie,
                TakId = createLidViewModel.TakId
            };

            _dbContext.Leden.Add(lid);
            _dbContext.SaveChanges();

            lid = _dbContext.Leden.Include(l => l.Tak).Single(l => l.Id == lid.Id);
            var lidViewModel = new LidViewModel(lid);
            return Created($"/api/leden/{lid.Id}", lidViewModel);
        }
    }
}
