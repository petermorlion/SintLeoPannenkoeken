using SintLeoPannenkoeken.Models.Views;

namespace SintLeoPannenkoeken.ViewModels.Rapporten
{
    public class BestuurderDetailsViewModel
    {
        public string BestuurderNaam { get; set;}
        public IList<VwRonde> VwRondes { get; set;}
        public string PostNummer => VwRondes.Count > 0 ? VwRondes[0].zone_postnummer : "";
        public string Gemeente => VwRondes.Count > 0 ? VwRondes[0].zone_gemeente: "";
    }
}
