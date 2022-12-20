using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Bestellingen
{
    public class LidViewModel
    {
        private readonly Lid _lid;

        public LidViewModel(Lid lid)
        {
            _lid = lid;
            if (_lid == null)
            {
                _lid = new Lid("", "");
            }
        }

        public int Id => _lid.Id;
        public string Achternaam => _lid.Achternaam;
        public string Voornaam => _lid.Voornaam;
        public string TakNaam => _lid.Tak.Naam;
    }
}
