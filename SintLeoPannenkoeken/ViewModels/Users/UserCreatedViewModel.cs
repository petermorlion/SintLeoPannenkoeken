using Microsoft.AspNetCore.Identity;

namespace SintLeoPannenkoeken.ViewModels.Users
{
    public class UserCreatedViewModel
    {
        public UserCreatedViewModel(IdentityUser user, string passwordResetLink)
        {
            User = user;
            PasswordResetLink  = passwordResetLink;
        }

        public IdentityUser User { get; set; }
        public string PasswordResetLink { get; set; }
    }
}
