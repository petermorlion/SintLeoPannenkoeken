
using Microsoft.AspNetCore.Identity;
using SintLeoPannenkoeken.Blazor.Data;

namespace SintLeoPannenkoeken.Blazor.Startup
{
    public class AddAdminUser : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public AddAdminUser(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope  = _serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var user = await userManager.FindByEmailAsync("peter.morlion@gmail.com");
                if (user != null)
                {
                    return;
                }

                user = new ApplicationUser();
                user.UserName = "peter.morlion@gmail.com";
                user.Email = "peter.morlion@gmail.com";
                user.EmailConfirmed = true;
                var userResult = await userManager.CreateAsync(user, "adminPassword1234!");

                var adminRole = "Admin";
                var roleExists = await roleManager.RoleExistsAsync(adminRole);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                var roleResult = await userManager.AddToRoleAsync(user, adminRole);
            }
        }
    }
}
