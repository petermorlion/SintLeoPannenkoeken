using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Leden
{
    public class LidViewModel
    {
        private readonly Lid _lid;

        public LidViewModel(Lid lid)
        {
            _lid = lid;
        }

        public int Id => _lid.Id;
        public string Achternaam => _lid.Achternaam;
        public string Voornaam => _lid.Voornaam;
        public string Functie => _lid.Functie;
    }
}
