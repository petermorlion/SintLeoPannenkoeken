using Microsoft.AspNetCore.Identity;

namespace SintLeoPannenkoeken.ViewModels.Users
{
    public class IndexViewModel
    {
        public IndexViewModel(ICollection<IdentityUser> users)
        {
            Users = users.Select(user => new UserViewModel(user)).OrderBy(user => user.Email).ToList();
        }

        public IList<UserViewModel> Users { get; set; }
    }
}
