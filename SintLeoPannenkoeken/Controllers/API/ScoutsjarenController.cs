using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.ViewModels.Scoutsjaren;
using SintLeoPannenkoeken.ViewModels.Users;
using System.IO;

namespace SintLeoPannenkoeken.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class ScoutsjarenController : ControllerBase
    {
        private readonly ILogger<ScoutsjarenController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public ScoutsjarenController(ILogger<ScoutsjarenController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            var scoutsjaren = _dbContext.Scoutsjaren.ToList();

            var scoutsjaarViewModels = scoutsjaren == null 
                ? new List<ScoutsjaarViewModel>() 
                : scoutsjaren.Select(scoutsjaar => new ScoutsjaarViewModel(scoutsjaar)).ToList();

            return Ok(scoutsjaarViewModels);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post([FromBody] CreateScoutsjaarViewModel createScoutsjaarViewModel)
        {
            var scoutsjaar = new Scoutsjaar(createScoutsjaarViewModel.Begin, createScoutsjaarViewModel.PannenkoekenPerPak);
            _dbContext.Scoutsjaren.Add(scoutsjaar);
            _dbContext.SaveChanges();

            scoutsjaar = _dbContext.Scoutsjaren.Single(s => s.Id == scoutsjaar.Id);
            var scoutsjaarViewModel = new ScoutsjaarViewModel(scoutsjaar);
            return Created($"/api/scoutsjaren/{scoutsjaar.Id}", scoutsjaarViewModel);
        }
    }
}
