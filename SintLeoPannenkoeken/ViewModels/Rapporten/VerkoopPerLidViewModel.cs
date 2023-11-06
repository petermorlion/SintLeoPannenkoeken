using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Rapporten
{
    public class VerkoopPerLidViewModel
    {
        public VerkoopPerLidViewModel(IDictionary<Lid, int> verkoopPerLid)
        {
            LidVerkoopViewModels = new List<LidVerkoopViewModel>();
            foreach (var lidVerkoop in verkoopPerLid)
            {
                LidVerkoopViewModels.Add(new LidVerkoopViewModel
                {
                    Naam = lidVerkoop.Key.Achternaam + " " + lidVerkoop.Key.Voornaam,
                    TakNaam = lidVerkoop.Key.Tak.Naam,
                    AantalPakkenVerkocht = lidVerkoop.Value
                });
            }
        }

        public IList<LidVerkoopViewModel> LidVerkoopViewModels { get; set; }
    }
}
