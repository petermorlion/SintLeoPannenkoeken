using Microsoft.AspNetCore.Identity;
using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Scoutsjaren
{
    public class IndexViewModel
    {
        public IndexViewModel(ICollection<Scoutsjaar> scoutsjaren)
        {
            Scoutsjaren = scoutsjaren.Select(scoutsjaar => new ScoutsjaarViewModel(scoutsjaar)).OrderBy(scoutsjaar => scoutsjaar.Begin).ToList();
        }

        public IList<ScoutsjaarViewModel> Scoutsjaren { get; set; }
    }
}
