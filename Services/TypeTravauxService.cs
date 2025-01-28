using LimsTravauxService.Data;
using LimsTravauxService.Models;
using Microsoft.EntityFrameworkCore;

namespace LimsTravauxService.Services;

public class TypeTravauxService : ITypeTravauxService
{
    private readonly TravauxContext _dbContext;
    public TypeTravauxService(TravauxContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TypeTravaux>> GetTypeTravauxFrom(int skiped, int size)
    {
        List<TypeTravaux> results = await _dbContext.TypeTravaux
        .Include(tt => tt.Departement)
        .Skip(skiped).Take(size)
            .ToListAsync();

        return results;
    }

    public int CountTypeTravaux()
    {
        int result =_dbContext.TypeTravaux.Count();
        return result;
    }
}