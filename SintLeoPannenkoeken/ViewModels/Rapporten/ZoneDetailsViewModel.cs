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
    }
}
