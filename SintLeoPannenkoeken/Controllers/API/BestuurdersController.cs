using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.ViewModels.Bestuurders;

namespace SintLeoPannenkoeken.Controllers.API
{
    [Route("api/[controller]")]
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
        [Route("")]
        public IActionResult Get()
        {
            var bestuurders = _dbContext.Bestuurders.ToList();

            var bestuurderViewModels = bestuurders == null 
                ? new List<BestuurderViewModel>() 
                : bestuurders.Select(bestuurder => new BestuurderViewModel(bestuurder)).ToList();

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
    }
}
