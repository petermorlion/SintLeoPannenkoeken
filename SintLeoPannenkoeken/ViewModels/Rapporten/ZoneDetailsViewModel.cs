using SintLeoPannenkoeken.Models.Views;

namespace SintLeoPannenkoeken.ViewModels.Rapporten
{
    public class ZoneDetailsViewModel
    {
        public int ScoutsjaarBegin { get; set; }
        public string ZoneNaam { get; set;}
        public IList<BestellingViewModel> Bestellingen { get; set;}
        public int? PostNummer { get; set; }
        public string Gemeente { get; set; }
        public string Bestuurder { get; set; }

        public int AantalGeleverd => Bestellingen.Where(x => x.Geleverd).Sum(x => x.Aantal);
        public int AantalNietGeleverd => Bestellingen.Where(x => !x.Geleverd).Sum(x => x.Aantal);
        public int Aantal => Bestellingen.Sum(x => x.Aantal);
    }
}
