using Microsoft.AspNetCore.Identity;

namespace SintLeoPannenkoeken.ViewModels.Users
{
    public class UserViewModel
    {
        private readonly IdentityUser _user;

        public UserViewModel(IdentityUser user)
        {
            _user = user;
            if (_user == null)
            {
                _user = new IdentityUser("");
            }
        }

        public string Id => _user.Id;
        public string Email => _user.Email;
    }
}
