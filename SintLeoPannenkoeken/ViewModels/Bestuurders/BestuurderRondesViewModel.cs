using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Bestuurders
{
    public class BestuurderRondesViewModel
    {
        public BestuurderRondesViewModel(int scoutsjaarBegin, Bestuurder bestuurder, ICollection<Zone> zones)
        {
            ScoutsjaarBegin = scoutsjaarBegin;
            Bestuurder = new BestuurderViewModel(bestuurder);
            Zones = zones.Select(zone => new ZoneViewModel(zone)).OrderBy(zone => zone.Naam).ToList();
        }

        public int ScoutsjaarBegin { get; set; }
        public BestuurderViewModel Bestuurder { get; set; }
        public List<ZoneViewModel> Zones { get; set; }
    }
}
