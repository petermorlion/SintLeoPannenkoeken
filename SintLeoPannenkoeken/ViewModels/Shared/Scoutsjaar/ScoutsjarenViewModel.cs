using SintLeoPannenkoeken.Models;
using System.Collections.Generic;

namespace SintLeoPannenkoeken.ViewModels.Shared.Scoutsjaar
{
    public class ScoutsjarenViewModel
    {
        private readonly IList<ScoutsjaarViewModel> _scoutsjarenViewModels;
        private readonly int _selectedScoutsjaar;

        public ScoutsjarenViewModel(IList<Models.Scoutsjaar> scoutsjaren, int selectedScoutsjaar)
        {
            _scoutsjarenViewModels = scoutsjaren.Select(x => new ScoutsjaarViewModel(x)).ToList();
            _selectedScoutsjaar = selectedScoutsjaar;
        }

        public IList<ScoutsjaarViewModel> ScoutsjarenViewModels => _scoutsjarenViewModels;
        public int SelectedScoutsjaar => _selectedScoutsjaar;
    }
}
