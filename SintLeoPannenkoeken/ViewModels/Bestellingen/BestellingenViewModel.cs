using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Bestellingen
{
    public class BestellingenViewModel
    {
        private readonly IList<BestellingViewModel> _bestellingen;
        private readonly int _pannenkoekenPerPak;

        public BestellingenViewModel(IList<BestellingViewModel> bestellingen, int pannenkoekenPerPak)
        {
            _bestellingen = bestellingen;
            _pannenkoekenPerPak = pannenkoekenPerPak;
        }

        public IList<BestellingViewModel> Bestellingen => _bestellingen;
        public int AantalPakjes => _bestellingen.Sum(bestelling => bestelling.AantalPakken);
        public int AantalPannenkoeken => _bestellingen.Sum(bestelling => bestelling.AantalPakken) * _pannenkoekenPerPak;
    }
}
