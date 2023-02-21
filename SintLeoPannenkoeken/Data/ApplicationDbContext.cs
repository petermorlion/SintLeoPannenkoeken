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
        public DbSet<Tak> Takken { get; set; }
        public DbSet<Bestelling> Bestellingen { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Straat> Straten { get; set; }
        public DbSet<StreefCijfer> StreefCijfers { get; set; }
        public DbSet<Bestuurder> Bestuurders { get; set; }
        public DbSet<Ronde> Rondes { get; set; }
    }
}