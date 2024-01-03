using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Bestuurders
{
    public class ScoutsjaarViewModel
    {
        private readonly Scoutsjaar _scoutsjaar;

        public ScoutsjaarViewModel(Scoutsjaar scoutsjaar)
        {
            _scoutsjaar = scoutsjaar;
        }

        public int Begin => _scoutsjaar.Begin;
    }
}
