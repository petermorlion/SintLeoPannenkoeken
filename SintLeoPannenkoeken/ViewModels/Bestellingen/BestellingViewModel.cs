using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Bestellingen
{
    public class BestellingViewModel
    {
        private readonly Bestelling _bestelling;

        public BestellingViewModel(Bestelling bestelling)
        {
            _bestelling = bestelling;
        }

        public int Id => _bestelling.Id;
        public string Naam => _bestelling.Naam;
        public string Telefoon => _bestelling.Telefoon;
        public int AantalPakken => _bestelling.AantalPakken;
        public string Opmerkingen => _bestelling.Opmerkingen;
        public bool Betaald => _bestelling.Betaald;
    }
}
