using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Straten
{
    public class IndexViewModel
    {
        public IndexViewModel(ICollection<Straat> straten)
        {
            Straten = straten.Select(straat => new StraatViewModel(straat)).OrderBy(straat => straat.Id).ToList();
        }

        public IList<StraatViewModel> Straten { get; set; }
    }
}
