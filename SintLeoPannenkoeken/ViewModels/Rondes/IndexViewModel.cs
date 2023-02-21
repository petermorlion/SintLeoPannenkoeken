using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Rondes
{
    public class IndexViewModel
    {
        public IndexViewModel(Scoutsjaar scoutsjaar, ICollection<Bestuurder> bestuurders, ICollection<Zone> zones)
        {
            Scoutsjaar = new ScoutsjaarViewModel(scoutsjaar);
            Bestuurders = bestuurders.Select(bestuurder => new BestuurderViewModel(bestuurder)).OrderBy(bestuurder => bestuurder.Achternaam).ThenBy(lid => lid.Voornaam).ToList();
            Zones = zones.Select(zone => new ZoneViewModel(zone)).OrderBy(zone => zone.Naam).ToList();
        }

        public ScoutsjaarViewModel Scoutsjaar { get; set; }
        public IList<BestuurderViewModel> Bestuurders { get; set; }
        public IList<ZoneViewModel> Zones { get; set; }
    }
}
