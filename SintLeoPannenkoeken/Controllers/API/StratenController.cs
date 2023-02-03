using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.ViewModels.Straten;

namespace SintLeoPannenkoeken.Controllers.API
{
    [Route("api/[controller]")]
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

        //[HttpPost]
        //[Route("")]
        //public IActionResult Put([FromBody] CreateLidViewModel createLidViewModel)
        //{
        //    var lid = new Lid(createLidViewModel.Voornaam, createLidViewModel.Achternaam)
        //    {
        //        Functie = createLidViewModel.Functie,
        //        TakId = createLidViewModel.TakId
        //    };

        //    _dbContext.Leden.Add(lid);
        //    _dbContext.SaveChanges();
        //    return Created($"/api/leden/{lid.Id}", lid);
        //}
    }
}
