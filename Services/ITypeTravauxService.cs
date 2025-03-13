using LimsTravauxService.Models;
using LimsTravauxService.Dto;

namespace LimsTravauxService.Services;

public interface ITypeTravauxService
{
    Task<List<TypeTravaux>> GetTypeTravauxFrom(int skiped, int size);
    int CountTypeTravaux();
    Task<TypeTravaux> CreateTypeTravaux(TypeTravauxDto typeTravauxDto);
    Task<TypeTravaux> GetTypeTravaux(int id);
    Task<TypeTravaux> UpdateTypeTravaux(int id, TypeTravauxDto typeTravauxDto);
    Task<List<TypeTravaux>> GetAllTypesTravaux();
}