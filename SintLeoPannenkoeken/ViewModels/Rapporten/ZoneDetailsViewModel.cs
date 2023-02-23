using SintLeoPannenkoeken.Models.Views;

namespace SintLeoPannenkoeken.ViewModels.Rapporten
{
    public class ZoneDetailsViewModel
    {
        public string ZoneNaam { get; set;}
        public IList<VwRonde> VwRondes { get; set;}
        public string PostNummer => VwRondes.Count > 0 ? VwRondes[0].zone_postnummer : "";
        public string Gemeente => VwRondes.Count > 0 ? VwRondes[0].zone_gemeente: "";
        public string Bestuurder => VwRondes.Count > 0 ? VwRondes[0].bestuurder : "";
    }
}
