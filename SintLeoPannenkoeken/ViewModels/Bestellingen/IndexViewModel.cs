using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.ViewModels.Leden;

namespace SintLeoPannenkoeken.ViewModels.Bestellingen
{
    public class IndexViewModel
    {
        public IndexViewModel(Scoutsjaar scoutsjaar, ICollection<Lid> leden)
        {
            Scoutsjaar = new ScoutsjaarViewModel(scoutsjaar);
            Leden = leden.Select(lid => new LidViewModel(lid)).OrderBy(lid => lid.Achternaam).ThenBy(lid => lid.Voornaam).ToList();
        }

        public ScoutsjaarViewModel Scoutsjaar { get; set; }
        public IList<LidViewModel> Leden { get; set; }
    }
}
