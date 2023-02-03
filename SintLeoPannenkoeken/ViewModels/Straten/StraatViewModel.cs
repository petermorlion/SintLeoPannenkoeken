using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Straten
{
    public class StraatViewModel
    {
        private readonly Straat _straat;

        public StraatViewModel(Straat straat)
        {
            _straat = straat;
            if (_straat == null)
            {
                _straat = new Straat("", "");
            }
        }

        public int Id => _straat.Id;
        public string Naam => _straat.Naam;
        public int PostNummer => _straat.PostNummer;
        public string Gemeente => _straat.Gemeente;
        public string? Omschrijving => _straat.Omschrijving;
        public int ZoneId => _straat.ZoneId;
        public string Zone => _straat.Zone?.Naam ?? "";
        public int Nummer => _straat.Nummer;
    }
}
