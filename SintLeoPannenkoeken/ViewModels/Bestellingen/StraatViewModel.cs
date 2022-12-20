using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Bestellingen
{
    public class StraatViewModel
    {
        private readonly Straat _straat;

        public StraatViewModel(Straat straat)
        {
            _straat = straat;
        }

        public int Id => _straat.Id;
        public string Naam => _straat.Naam;
        public string Gemeente => _straat.Gemeente;
    }
}
