using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace SintLeoPannenkoeken.Blazor.Data
{
    public class UsersService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UsersService> _logger;

        public UsersService(IDbContextFactory<ApplicationDbContext> dbContextFactory,
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            ILogger<UsersService> logger)
        {
            _dbContextFactory = dbContextFactory;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<IList<GebruikerDto>> GetGebruikers()
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var users = await dbContext.Users.ToListAsync();

                var userDtos = new List<GebruikerDto>();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    userDtos.Add(new GebruikerDto(user.Id, user.Email, roles.ToList()));
                }

                return userDtos;
            }
        }

        public async Task<GebruikerCreatedDto> CreateUser(NewGebruikerDto gebruiker)
        {
            var user = new ApplicationUser();
            user.UserName = gebruiker.Email;
            user.Email = gebruiker.Email;
            user.EmailConfirmed = true;

            var randomPassword = GetRandomPassword();
            var userResult = await _userManager.CreateAsync(user, randomPassword);
            if (!userResult.Succeeded)
            {
                _logger.LogError("Error creating user: {Errors}", string.Join(", ", userResult.Errors.Select(e => e.Description)));
                throw new ApplicationException("Error creating user");
            }

            foreach (var role in gebruiker.Rollen)
            {
                var roleExists = await _roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

                var roleResult = await _userManager.AddToRoleAsync(user, role);
                if (!roleResult.Succeeded)
                {
                    _logger.LogError("Error adding role to user: {Errors}", string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                    throw new ApplicationException("Error adding roles to user");
                }
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var gebruikerDto = new GebruikerDto(user.Id, user.Email, gebruiker.Rollen);
            
            return new GebruikerCreatedDto(gebruikerDto, encodedCode);
        }

        private string GetRandomPassword()
        {
            var length = 32;
            var alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890&é'(§è!çà)";
            var secret = new StringBuilder();
            while (length-- > 0)
            {
                secret.Append(alphabet[RandomNumberGenerator.GetInt32(alphabet.Length)]);
            }

            return secret.ToString();
        }
    }
}
