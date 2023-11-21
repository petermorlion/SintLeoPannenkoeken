using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.ViewModels.Shared.Scoutsjaar;

namespace SintLeoPannenkoeken.ViewComponents
{
    public class ScoutsjaarViewComponent : ViewComponent
    {
        private ILogger<ScoutsjaarViewComponent> _logger;
        private ApplicationDbContext _dbContext;

        public ScoutsjaarViewComponent(ILogger<ScoutsjaarViewComponent> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? selectedScoutsjaar)
        {
            if (selectedScoutsjaar == null)
            {
                selectedScoutsjaar = _dbContext.Scoutsjaren.OrderByDescending(sj => sj.Begin).First().Begin;
            }
            
            var scoutsjaren = _dbContext.Scoutsjaren.ToList();
            var scoutsjarenViewModel = new ScoutsjarenViewModel(scoutsjaren, (int)selectedScoutsjaar);
            return View("TitleSelector", scoutsjarenViewModel);
        }
    }
}
