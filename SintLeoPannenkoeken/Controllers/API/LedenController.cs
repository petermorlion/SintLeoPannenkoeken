using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.ViewModels.Leden;

namespace SintLeoPannenkoeken.Controllers.API
{
    [Route("api/[controller]")]
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
            var leden = _dbContext.Leden.ToList();

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
                Functie = createLidViewModel.Functie
            };

            _dbContext.Leden.Add(lid);
            _dbContext.SaveChanges();
            return Created($"/api/leden/{lid.Id}", lid);
        }
    }
}
