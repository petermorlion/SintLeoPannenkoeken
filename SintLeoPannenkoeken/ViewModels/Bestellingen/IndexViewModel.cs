using SintLeoPannenkoeken.Models;
using SintLeoPannenkoeken.ViewModels.Leden;

namespace SintLeoPannenkoeken.ViewModels.Bestellingen
{
    public class IndexViewModel
    {
        public IndexViewModel(Scoutsjaar scoutsjaar, ICollection<Lid> leden, ICollection<Straat> straten)
        {
            Scoutsjaar = new ScoutsjaarViewModel(scoutsjaar);
            Leden = leden.Select(lid => new LidViewModel(lid)).OrderBy(lid => lid.Achternaam).ThenBy(lid => lid.Voornaam).ToList();
            Straten = straten.Select(straat => new StraatViewModel(straat)).OrderBy(straat => straat.Naam).ToList();
        }

        public ScoutsjaarViewModel Scoutsjaar { get; set; }
        public IList<LidViewModel> Leden { get; set; }
        public IList<StraatViewModel> Straten { get; set; }
    }
}
