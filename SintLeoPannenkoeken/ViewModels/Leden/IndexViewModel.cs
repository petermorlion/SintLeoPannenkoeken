using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Leden
{
    public class IndexViewModel
    {
        public IndexViewModel(ICollection<Tak> takken)
        {
            Takken = takken.Select(tak => new TakViewModel(tak)).OrderBy(tak => tak.Id).ToList();
        }

        public IList<TakViewModel> Takken { get; set; }
    }
}
