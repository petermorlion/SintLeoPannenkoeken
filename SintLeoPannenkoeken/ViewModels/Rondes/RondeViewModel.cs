using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Rondes
{
    public class RondeViewModel
    {
        public RondeViewModel(Ronde ronde, IList<Bestelling> bestellingen)
        {
            Zone = new ZoneViewModel(ronde.Zone);
            Bestuurder = new BestuurderViewModel(ronde.Bestuurder);

            AantalAdressen = bestellingen.GroupBy(b => $"{b.StraatId}-{b.Nummer}").Count();
            AantalBestellingen = bestellingen.Count;
            AantalPakken = bestellingen.Sum(b => b.AantalPakken);
        }

        public ZoneViewModel Zone { get; set; }
        public int AantalBestellingen { get; set; }
        public int AantalAdressen { get; set; }
        public int AantalPakken { get; set; }
        public BestuurderViewModel Bestuurder { get; set; }
    }
}
