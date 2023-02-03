using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Bestellingen
{
    public class BestellingenViewModel
    {
        private readonly IList<BestellingViewModel> _bestellingen;

        public BestellingenViewModel(IList<BestellingViewModel> bestellingen)
        {
            _bestellingen = bestellingen;
        }

        public IList<BestellingViewModel> Bestellingen => _bestellingen;
        public int AantalPakjes => _bestellingen.Count;
        public int AantalPannenkoeken => _bestellingen.Count * 5;
    }
}
