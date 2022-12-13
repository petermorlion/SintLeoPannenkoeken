using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Scoutsjaar> Scoutsjaren { get; set; }
        public DbSet<Lid> Leden { get; set; }
    }
}