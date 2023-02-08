namespace SintLeoPannenkoeken.ViewModels.StreefCijfers
{
    public class StreefCijfersViewModel
    {
        private readonly IList<StreefCijferViewModel> _streefCijfers;

        public StreefCijfersViewModel(IList<StreefCijferViewModel> streefCijfers)
        {
            _streefCijfers = streefCijfers;
        }

        public IList<StreefCijferViewModel> StreefCijfers => _streefCijfers;
    }
}
