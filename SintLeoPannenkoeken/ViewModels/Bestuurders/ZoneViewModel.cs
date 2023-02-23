using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Bestuurders
{
    public class ZoneViewModel
    {
        private readonly Zone _zone;

        public ZoneViewModel(Zone zone, IList<Bestelling> bestellingen)
        {
            _zone = zone;
            AantalBestellingen = bestellingen.Count;
            AantalPakken = bestellingen.Sum(b => b.AantalPakken);
            AantalAdressen = bestellingen.GroupBy(b => $"{b.StraatId}-{b.Nummer}").Count();
        }

        public int Id => _zone.Id;
        public string Naam => _zone.Naam;
        public int AantalAdressen{ get; }
        public int AantalBestellingen { get; }
        public int AantalPakken { get; }
    }
}
