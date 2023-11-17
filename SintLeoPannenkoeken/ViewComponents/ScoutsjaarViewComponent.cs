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

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var scoutsjaren = _dbContext.Scoutsjaren.ToList();
            var selectedScoutsjaarId = scoutsjaren.First().Id;
            var scoutsjarenViewModel = new ScoutsjarenViewModel(scoutsjaren, selectedScoutsjaarId);
            return View("TitleSelector", scoutsjarenViewModel);
        }
    }
}
