using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Straten
{
    public class IndexViewModel
    {
        public IndexViewModel(ICollection<Straat> straten, ICollection<Zone> zones)
        {
            Straten = straten.Select(straat => new StraatViewModel(straat)).OrderBy(straat => straat.Id).ToList();
            Zones = zones.Select(zone => new ZoneViewModel(zone)).OrderBy(zone => zone.Naam).ToList();
        }

        public IList<StraatViewModel> Straten { get; set; }
        public IList<ZoneViewModel> Zones { get; set; }
    }
}
