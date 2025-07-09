using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;
using SintLeoPannenkoeken.Blazor.Data;

namespace SintLeoPannenkoeken.Blazor.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
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
            var leden = _dbContext
                .Leden
                .Include(lid => lid.Tak)
                .OrderBy(lid => lid.Achternaam)
                .ToList();

            var lidDtos = leden == null 
                ? new List<LidDto>() 
                : leden.Select(lid => new LidDto(
                    lid.Id,
                    lid.Voornaam,
                    lid.Achternaam,
                    lid.Functie,
                    lid.Tak?.Naam ?? "Onbekend"
                )).ToList();

            return Ok(lidDtos);
        }
    }
}
