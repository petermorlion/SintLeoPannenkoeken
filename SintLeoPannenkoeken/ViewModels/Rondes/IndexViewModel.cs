using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Rondes
{
    public class IndexViewModel
    {
        public IndexViewModel(Scoutsjaar scoutsjaar, ICollection<Ronde> rondes, ICollection<Bestelling> bestellingen)
        {
            Scoutsjaar = new ScoutsjaarViewModel(scoutsjaar);

            var bestellingenPerZone = bestellingen.GroupBy(b => b.Straat.ZoneId).ToDictionary(g => g.Key, g => g.ToList());
            Rondes = rondes.Select(ronde =>
            {
                var bestellingenForZone = bestellingenPerZone.ContainsKey(ronde.ZoneId) ? bestellingenPerZone[ronde.ZoneId] : new List<Bestelling>();
                return new RondeViewModel(ronde, bestellingenForZone);
            }).ToList();
        }

        public ScoutsjaarViewModel Scoutsjaar { get; set; }
        public IList<RondeViewModel> Rondes { get; set; }
    }
}
