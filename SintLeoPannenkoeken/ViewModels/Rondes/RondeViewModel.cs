using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Rondes
{
    public class RondeViewModel
    {
        private Ronde _ronde;
        public RondeViewModel(Ronde ronde)
        {
            _ronde = ronde;
        }

        public string ZoneNaam => _ronde.Zone.Naam;
        public string BestuurderVoornaam => _ronde.Bestuurder.Voornaam;
        public string BestuurderAchternaam => _ronde.Bestuurder.Achternaam;
        public int AantalStraten => _ronde.Zone.Straten.Count();
        public int AantalPakjes => 4;
    }
}
