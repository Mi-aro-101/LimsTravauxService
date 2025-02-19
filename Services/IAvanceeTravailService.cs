using LimsTravauxService.Models;

namespace LimsTravauxService.Services;

public interface IAvanceeTravailService
{
    Task<AvanceeTravail> CreateAvanceeTravail(AvanceeTravail avanceeTravail);
    Task<AvanceeTravail> GetAvanceeTravail(int id);
    Task<List<AvanceeTravail>> GetAvanceesTravaux();
}