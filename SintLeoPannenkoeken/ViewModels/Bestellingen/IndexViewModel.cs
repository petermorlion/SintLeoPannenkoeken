using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Bestellingen
{
    public class IndexViewModel
    {
        public IndexViewModel(IList<Scoutsjaar> scoutsjaren)
        {
            Scoutsjaren = scoutsjaren.Select(scoutsjaar => new ScoutsjaarViewModel(scoutsjaar)).ToList();
        }

        public IList<ScoutsjaarViewModel> Scoutsjaren { get; set; }
    }
}
