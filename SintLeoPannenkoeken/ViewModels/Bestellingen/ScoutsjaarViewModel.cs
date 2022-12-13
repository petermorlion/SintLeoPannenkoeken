using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Bestellingen
{
    public class ScoutsjaarViewModel
    {
        private readonly Scoutsjaar _scoutsjaar;

        public ScoutsjaarViewModel(Scoutsjaar scoutsjaar)
        {
            _scoutsjaar = scoutsjaar;
        }

        public int ScoutsjaarId => _scoutsjaar.Id;
        public string BeginJaar => _scoutsjaar.Begin.Year.ToString();
        public string EindJaar => _scoutsjaar.Einde.Year.ToString();
    }
}
