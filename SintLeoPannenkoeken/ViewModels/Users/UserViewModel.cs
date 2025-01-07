using Microsoft.AspNetCore.Identity;

namespace SintLeoPannenkoeken.ViewModels.Users
{
    public class UserViewModel
    {
        private readonly IdentityUser _user;
        private readonly IList<string> _roles;

        public UserViewModel(IdentityUser user, IList<string> roles)
        {
            _user = user;
            if (_user == null)
            {
                _user = new IdentityUser("");
            }

            _roles = roles;
        }

        public string Id => _user.Id;
        public string Email => _user.Email;
        public IList<string> Roles => _roles;
    }
}
