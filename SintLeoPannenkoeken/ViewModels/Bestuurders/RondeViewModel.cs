using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Bestuurders
{
    public class RondeViewModel
    {
        private Ronde _ronde;
        private int _aantalPakken;

        public RondeViewModel(Ronde ronde, int aantalPakken)
        {
            _ronde = ronde;
            _aantalPakken = aantalPakken;
        }

        public string ZoneNaam => _ronde.Zone.Naam;
        public int AantalStraten => _ronde.Zone.Straten.Count();
        public int AantalPakken => _aantalPakken;
    }
}
