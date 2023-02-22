using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Bestuurders
{
    public class BestuurderMetAantallenViewModel
    {
        private readonly Bestuurder _bestuurder;

        public BestuurderMetAantallenViewModel(Bestuurder bestuurder, int aantalPakken, int aantalBestellingen)
        {
            _bestuurder = bestuurder;
            AantalPakken = aantalPakken;
            AantalBestellingen = aantalBestellingen;
            if (_bestuurder == null)
            {
                _bestuurder = new Bestuurder();
            }
        }

        public int Id => _bestuurder.Id;
        public string Achternaam => _bestuurder.Achternaam;
        public string Voornaam => _bestuurder.Voornaam;

        public int AantalPakken { get; }
        public int AantalBestellingen { get; }
    }
}
