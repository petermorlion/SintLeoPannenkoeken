using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Bestuurders
{
    public class BestuurderRondesViewModel
    {
        public BestuurderRondesViewModel(int scoutsjaarBegin, Bestuurder bestuurder, ICollection<Zone> zones, ICollection<Bestelling> bestellingen)
        {
            ScoutsjaarBegin = scoutsjaarBegin;
            Bestuurder = new BestuurderViewModel(bestuurder);

            var bestellingenPerZone = bestellingen.GroupBy(b => b.Straat.ZoneId).ToDictionary(b => b.Key, b => b.ToList());
            Zones = zones.Select(zone =>
            {
                var bestellingenForZone = bestellingenPerZone.ContainsKey(zone.Id) ? bestellingenPerZone[zone.Id] : new List<Bestelling>();
                var aantalPakken = bestellingenForZone.Sum(b => b.AantalPakken);
                return new ZoneViewModel(zone, bestellingenForZone.Count, aantalPakken);
            }).OrderBy(zone => zone.Naam).ToList();
        }

        public int ScoutsjaarBegin { get; set; }
        public BestuurderViewModel Bestuurder { get; set; }
        public List<ZoneViewModel> Zones { get; set; }
    }
}
