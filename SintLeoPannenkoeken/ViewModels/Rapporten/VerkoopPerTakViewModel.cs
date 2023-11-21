using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Rapporten
{
    public class VerkoopPerTakViewModel
    {
        public VerkoopPerTakViewModel(IDictionary<int, int> verkoopPerTak, IDictionary<int, int> streefcijfersPerTak, IList<Tak> takken, int scoutsjaarBegin)
        {
            TakVerkoopViewModels = new List<TakVerkoopViewModel>();
            foreach (var tak in takken)
            {
                TakVerkoopViewModels.Add(new TakVerkoopViewModel
                {
                    Naam = tak.Naam,
                    AantalPakkenVerkocht = verkoopPerTak.ContainsKey(tak.Id) ? verkoopPerTak[tak.Id] : 0,
                    Streefcijfer = streefcijfersPerTak.ContainsKey(tak.Id) ? streefcijfersPerTak[tak.Id] : 0,
                    Leden = tak.Leden.Count()

                });
            }

            ScoutsjaarBegin = scoutsjaarBegin;
        }

        public IList<TakVerkoopViewModel> TakVerkoopViewModels { get; set; }
        public int ScoutsjaarBegin { get; }
    }
}
