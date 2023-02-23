using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Rondes
{
    public class BestuurderViewModel
    {
        private readonly Bestuurder _bestuurder;
        public BestuurderViewModel(Bestuurder bestuurder)
        {
            _bestuurder = bestuurder;
        }

        public int Id => _bestuurder.Id;
        public string Achternaam => _bestuurder.Achternaam;
        public string Voornaam => _bestuurder.Voornaam;
    }
}
