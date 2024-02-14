using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Rondes
{
    public class IndexViewModel
    {
        public IndexViewModel(Scoutsjaar scoutsjaar, ICollection<Ronde> rondes, ICollection<Bestelling> bestellingen, ICollection<Zone> zones)
        {
            Scoutsjaar = new ScoutsjaarViewModel(scoutsjaar);

            var bestellingenPerZone = bestellingen.GroupBy(b => b.Straat.ZoneId).ToDictionary(g => g.Key, g => g.ToList());
            Rondes = zones.OrderBy(zone => zone.Naam).Select(zone =>
            {
                var bestellingenForZone = bestellingenPerZone.ContainsKey(zone.Id) ? bestellingenPerZone[zone.Id] : new List<Bestelling>();
                var ronde = rondes.SingleOrDefault(r => r.ScoutsjaarId == scoutsjaar.Id && r.ZoneId == zone.Id);
                return new RondeViewModel(zone, ronde, bestellingenForZone);
            }).ToList();
        }

        public ScoutsjaarViewModel Scoutsjaar { get; set; }
        public IList<RondeViewModel> Rondes { get; set; }
    }
}
