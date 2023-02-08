using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.ViewModels.Bestellingen;
using SintLeoPannenkoeken.ViewModels.StreefCijfers;

namespace SintLeoPannenkoeken.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreefCijfersController : ControllerBase
    {
        private readonly ILogger<StreefCijfersController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public StreefCijfersController(ILogger<StreefCijfersController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("{jaar:int}")]
        public IActionResult Get(int jaar)
        {
            var streefCijfers = _dbContext.Scoutsjaren
                .Include(scoutsjaar => scoutsjaar.StreefCijfers)
                .ThenInclude(streefCijfer => streefCijfer.Tak)
                .SingleOrDefault(scoutsjaar => scoutsjaar.Begin == jaar)
                ?.StreefCijfers
                ?.ToList();

            var streefCijferViewModels = streefCijfers == null
                ? new List<StreefCijferViewModel>()
                : streefCijfers.Select(streefCijfer => new StreefCijferViewModel(streefCijfer)).ToList();

            var streefCijfersViewModel = new StreefCijfersViewModel(streefCijferViewModels);

            return Ok(streefCijfersViewModel);
        }

        [HttpPut]
        [Route("{jaar:int}")]
        public IActionResult Put(int jaar, [FromBody] CreateStreefCijferViewModel createStreefCijferViewModel)
        {
            var scoutsjaar = _dbContext.Scoutsjaren.Include(s => s.StreefCijfers).SingleOrDefault(s => s.Begin == jaar);
            if (scoutsjaar == null)
            {
                return BadRequest("Onbekend scoutsjaar");
            }

            var streefCijfer = scoutsjaar.StreefCijfers.SingleOrDefault(s => s.TakId == createStreefCijferViewModel.TakId);

            if (streefCijfer == null)
            {
                streefCijfer = new StreefCijfer();
                scoutsjaar.StreefCijfers.Add(streefCijfer);
            }

            streefCijfer.TakId = createStreefCijferViewModel.TakId;
            streefCijfer.Aantal = createStreefCijferViewModel.Aantal;

            _dbContext.SaveChanges();

            streefCijfer = _dbContext.StreefCijfers
                .Include(s => s.Tak)
                .Single(s => s.Id == streefCijfer.Id);

            var streefCijferViewModel = new StreefCijferViewModel(streefCijfer);
            return Created($"/api/streefCijfers/{streefCijfer.Id}", streefCijferViewModel);
        }
    }
}
