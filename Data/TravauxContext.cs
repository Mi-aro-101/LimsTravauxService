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


         // Define the many-to-many relationship between users and roles
        modelBuilder.Entity<TypeTravauxTypeEchantillon>()
            .HasKey(ur => new { ur.IdTypeTravaux, ur.IdTypeEchantillon });

        modelBuilder.Entity<TypeTravauxTypeEchantillon>()
            .HasOne(t => t.TypeTravaux)
            .WithMany(u => u.TypeTravauxTypeEchantillons)
            .HasForeignKey(t => t.IdTypeTravaux);

        modelBuilder.Entity<TypeTravauxTypeEchantillon>()
            .HasOne(ur => ur.TypeEchantillon)
            .WithMany(r => r.TypeTravauxTypeEchantillons)
            .HasForeignKey(ur => ur.IdTypeEchantillon);
    }

    public DbSet<TypeTravaux> TypeTravaux { get; set; }
    public DbSet<HistoriqueTarif> HistoriqueTarifs { get; set; }
    public DbSet<TypeTravauxTypeEchantillon> TypeTravauxTypeEchantillons { get; set; }
}