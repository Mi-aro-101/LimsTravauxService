using LimsTravauxService.Models;
using Microsoft.EntityFrameworkCore;

namespace LimsTravauxService.Data;

public class TravauxContext : DbContext
{
    public TravauxContext(DbContextOptions<TravauxContext> options) : base(options)
    {}

    public DbSet<TypeTravaux> TypeTravaux { get; set; }
}