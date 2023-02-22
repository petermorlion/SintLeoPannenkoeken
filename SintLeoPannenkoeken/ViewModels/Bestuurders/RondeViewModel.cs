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

        public int Id => _ronde.Id;
        public string ZoneNaam => _ronde.Zone.Naam;
        public string ZoneGemeente => _ronde.Zone.Gemeente;
        public string? ZoneKaartNummer => _ronde.Zone.KaartNummer;
        public int AantalStraten => _ronde.Zone.Straten.Count();
        public int AantalPakken => _aantalPakken;
    }
}
