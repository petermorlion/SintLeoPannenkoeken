using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Rondes
{
    public class RondeViewModel
    {
        public RondeViewModel(Zone zone, Ronde? ronde, IList<Bestelling> bestellingen)
        {
            Zone = new ZoneViewModel(zone);
            if (ronde != null)
            {
                Bestuurder = new BestuurderViewModel(ronde.Bestuurder);
            }

            AantalAdressen = bestellingen.GroupBy(b => $"{b.StraatId}-{b.Nummer}").Count();
            AantalBestellingen = bestellingen.Count;
            AantalPakken = bestellingen.Sum(b => b.AantalPakken);
        }

        public ZoneViewModel Zone { get; set; }
        public int AantalBestellingen { get; set; }
        public int AantalAdressen { get; set; }
        public int AantalPakken { get; set; }
        public BestuurderViewModel? Bestuurder { get; set; }
    }
}
