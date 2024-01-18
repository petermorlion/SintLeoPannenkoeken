namespace SintLeoPannenkoeken.ViewModels.Users
{
    public class IndexViewModel
    {
        public IndexViewModel(IList<UserViewModel> users)
        {
            Users = users;
        }

        public IList<UserViewModel> Users { get; set; }
    }
}
