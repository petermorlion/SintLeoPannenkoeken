using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.StreefCijfers
{
    public class IndexViewModel
    {
        public IndexViewModel(Scoutsjaar scoutsjaar, ICollection<Tak> takken)
        {
            Scoutsjaar = new ScoutsjaarViewModel(scoutsjaar);
            Takken = takken.Select(tak => new TakViewModel(tak)).OrderBy(tak => tak.Id).ToList();
        }

        public ScoutsjaarViewModel Scoutsjaar { get; set; }

        public IList<TakViewModel> Takken { get; set; }
    }
}
