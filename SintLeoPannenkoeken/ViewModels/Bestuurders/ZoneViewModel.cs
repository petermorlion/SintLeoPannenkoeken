using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Bestuurders
{
    public class ZoneViewModel
    {
        private readonly Zone _zone;

        public ZoneViewModel(Zone zone, int aantalBestellingen, int aantalPakken)
        {
            _zone = zone;
            AantalBestellingen = aantalBestellingen;
            AantalPakken = aantalPakken;
        }

        public int Id => _zone.Id;
        public string Naam => _zone.Naam;
        public int AantalBestellingen { get; }
        public int AantalPakken { get; }
    }
}
