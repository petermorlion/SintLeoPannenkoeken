using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SintLeoPannenkoeken.Blazor.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Build config
            var basePath = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: true)
                .AddEnvironmentVariables();
            var config = builder.Build();

            var connectionString = config.GetConnectionString("DefaultConnection");
            // For DEBUG, allow override from environment variable (to match Program.cs)
#if DEBUG
            var envConn = System.Environment.GetEnvironmentVariable("ConnectionStrings__database");
            if (!string.IsNullOrEmpty(envConn))
                connectionString = envConn;
#endif
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
} 
