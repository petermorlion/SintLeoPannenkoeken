using SintLeoPannenkoeken.Models;
using System.Collections.Generic;

namespace SintLeoPannenkoeken.ViewModels.Shared.Scoutsjaar
{
    public class ScoutsjarenViewModel
    {
        private readonly IList<ScoutsjaarViewModel> _scoutsjarenViewModels;
        private readonly int _selectedScoutsjaarId;

        public ScoutsjarenViewModel(IList<Models.Scoutsjaar> scoutsjaren, int selectedScoutsjaarId)
        {
            _scoutsjarenViewModels = scoutsjaren.Select(x => new ScoutsjaarViewModel(x)).ToList();
            _selectedScoutsjaarId = selectedScoutsjaarId;
        }

        public IList<ScoutsjaarViewModel> ScoutsjarenViewModels => _scoutsjarenViewModels;
        public int SelectedScoutsjaarId => _selectedScoutsjaarId;
    }
}
