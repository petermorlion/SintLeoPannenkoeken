using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.ViewModels.Bestellingen;

namespace SintLeoPannenkoeken.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BestellingenController : ControllerBase
    {
        private readonly ILogger<BestellingenController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public BestellingenController(ILogger<BestellingenController> logger, ApplicationDbContext dbContext    )
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
                .SingleOrDefault(scoutsjaar => scoutsjaar.Begin.Year == jaar)
                ?.Bestellingen
                ?.ToList();

            var bestellingenViewModels = bestellingen == null 
                ? new List<BestellingViewModel>() 
                : bestellingen.Select(bestelling => new BestellingViewModel(bestelling)).ToList();

            return Ok(bestellingenViewModels);
        }
    }
}
