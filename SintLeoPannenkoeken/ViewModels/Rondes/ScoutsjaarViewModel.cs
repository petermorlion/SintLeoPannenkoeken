using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Rondes
{
    public class ScoutsjaarViewModel
    {
        private readonly Scoutsjaar _scoutsjaar;

        public ScoutsjaarViewModel(Scoutsjaar scoutsjaar)
        {
            _scoutsjaar = scoutsjaar;
        }

        public int Begin => _scoutsjaar.Begin;
        public int Einde => _scoutsjaar.Einde;
    }
}
