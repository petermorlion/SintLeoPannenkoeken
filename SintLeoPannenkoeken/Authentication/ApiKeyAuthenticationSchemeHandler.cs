using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.ViewModels.ApiClients;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace SintLeoPannenkoeken.Authentication
{
    public class ApiKeyAuthenticationSchemeHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ApplicationDbContext _dbContext;

        public ApiKeyAuthenticationSchemeHandler(
            ApplicationDbContext dbContext,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder) : base(options, logger, encoder)
        {
            _dbContext = dbContext;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var apiKey = Context.Request.Headers["X-API-KEY"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return AuthenticateResult.Fail("Invalid X-API-KEY");
            }

            var apiClients = await _dbContext.ApiClients.ToListAsync();
            var apiClientsByKey = apiClients.ToDictionary(x => x.ApiKey, x => x.Naam);

            var passwordHasher = new PasswordHasher<IdentityUser>();

            foreach (var apiClient in apiClients)
            {
                // The user parameter isn't used (see https://github.com/dotnet/AspNetCore/blob/main/src/Identity/Extensions.Core/src/PasswordHasher.cs)
                var result = passwordHasher.VerifyHashedPassword(null, apiClient.ApiKey, apiKey);
                if (result.HasFlag(PasswordVerificationResult.Success))
                {
                    var claims = new[] { 
                        new Claim(ClaimTypes.Name, apiClient.Naam),
                        new Claim(ClaimTypes.Role, "ApiClient")
                    };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
            }

            return AuthenticateResult.Fail("Invalid X-API-KEY");
        }
    }
}
