using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Shared.Scoutsjaar
{
    public class ScoutsjaarViewModel
    {
        private readonly Models.Scoutsjaar _scoutsjaar;

        public ScoutsjaarViewModel(Models.Scoutsjaar scoutsjaar)
        {
            _scoutsjaar = scoutsjaar;
        }

        public int Id => _scoutsjaar.Id;
        public int Begin => _scoutsjaar.Begin;
        public int Einde => _scoutsjaar.Einde;
    }
}
