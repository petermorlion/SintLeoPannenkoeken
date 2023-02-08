using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.StreefCijfers
{
    public class StreefCijferViewModel
    {
        private readonly StreefCijfer _streefCijfer;

        public StreefCijferViewModel(StreefCijfer streefCijfer)
        {
            _streefCijfer = streefCijfer;
        }

        public int Id => _streefCijfer.Id;
        public string TakNaam => _streefCijfer.Tak.Naam;
        public int Aantal => _streefCijfer.Aantal;
    }
}
