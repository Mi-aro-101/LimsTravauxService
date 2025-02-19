using LimsTravauxService.Data;
using LimsTravauxService.Models;
using Microsoft.EntityFrameworkCore;

namespace LimsTravauxService.Services;

public class AvanceeTravailService : IAvanceeTravailService
{
    private readonly TravauxContext _dbContext;
    public AvanceeTravailService(TravauxContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AvanceeTravail> CreateAvanceeTravail(AvanceeTravail avanceeTravail)
    {
        _dbContext.AvanceeTravaux.Add(avanceeTravail);
        await _dbContext.SaveChangesAsync();
        return await Task.FromResult(avanceeTravail);
    }

    public async Task<List<AvanceeTravail>> GetAvanceesTravaux()
    {
        return await Task.FromResult(_dbContext.AvanceeTravaux.ToList());
    }

    public async Task<AvanceeTravail> GetAvanceeTravail(int id)
    {
        AvanceeTravail? result = await _dbContext.AvanceeTravaux.Where(at => at.IdAvanceeTravail == id).FirstOrDefaultAsync();
        if(result == null)
        {
            throw new ArgumentException("Cet element n'existe pas dans la base");
        }
        return result;
    }
}