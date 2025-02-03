using LimsTravauxService.Models;
using Microsoft.EntityFrameworkCore;

namespace LimsTravauxService.Data;

public class TravauxContext : DbContext
{
    public TravauxContext(DbContextOptions<TravauxContext> options) : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TypeTravaux>()
        .HasMany(e => e.HistoriqueTarifs)
        .WithOne(e => e.TypeTravaux)
        .HasForeignKey(e => e.IdTypeTravaux)
        .HasPrincipalKey(e => e.IdTypeTravaux);
    }

    public DbSet<TypeTravaux> TypeTravaux { get; set; }
    public DbSet<HistoriqueTarif> HistoriqueTarifs { get; set; }
}