using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Blazor.Models;

namespace SintLeoPannenkoeken.Blazor.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
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
