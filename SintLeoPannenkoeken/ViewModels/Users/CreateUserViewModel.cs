namespace SintLeoPannenkoeken.ViewModels.Users
{
    public class CreateUserViewModel
    {
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
