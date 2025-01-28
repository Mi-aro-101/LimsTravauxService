using LimsTravauxService.Models;

namespace LimsTravauxService.Services;

public interface ITypeTravauxService
{
    Task<List<TypeTravaux>> GetTypeTravauxFrom(int skiped, int size);
    int CountTypeTravaux();
}