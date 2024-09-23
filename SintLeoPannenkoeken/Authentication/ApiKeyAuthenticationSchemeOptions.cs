using Microsoft.AspNetCore.Authentication;

namespace SintLeoPannenkoeken.Authentication
{
    public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public string ApiKey { get; set; } = "";
    }
}
