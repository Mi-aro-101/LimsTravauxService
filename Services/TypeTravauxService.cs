using LimsTravauxService.Data;
using LimsTravauxService.Models;
using Microsoft.EntityFrameworkCore;
using LimsTravauxService.Dto;
using Microsoft.AspNetCore.Mvc;

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
        // List<TypeTravaux> results = await _dbContext.TypeTravaux
        // .Include(tt => tt.Departement)
        // .Skip(skiped).Take(size)
        //     .ToListAsync();
        List<TypeTravaux> results = await _dbContext.TypeTravaux
            .Select(tt => new TypeTravaux {
                IdTypeTravaux = tt.IdTypeTravaux,
                Code = tt.Code,
                Designation = tt.Designation,
                HasResultat = tt.HasResultat,
                IdDepartement = tt.IdDepartement,
                Departement = tt.Departement,
                Tarif = _dbContext.HistoriqueTarifs
                    .Where(ht => ht.IdTypeTravaux == tt.IdTypeTravaux)
                    .OrderByDescending(ht => ht.DateChangement)
                    .Select(ht => ht.Tarif)
                    .First()
            })
            .OrderByDescending(tt => tt.IdTypeTravaux)
            .Skip(skiped).Take(size)
            .ToListAsync();

        return results;
    }

    public int CountTypeTravaux()
    {
        int result =_dbContext.TypeTravaux.Count();
        return result;
    }

    public async Task<TypeTravaux> CreateTypeTravaux(TypeTravauxDto typeTravauxDto)
    {
        TypeTravaux typeTravaux = new TypeTravaux();
        using(var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            typeTravaux = typeTravaux.FromDto(typeTravauxDto);
            _dbContext.TypeTravaux.Add(typeTravaux);
            await _dbContext.SaveChangesAsync();
            if(typeTravauxDto.DateCreation == null)
            {
                typeTravauxDto.DateCreation = DateTime.Now;
            }
            HistoriqueTarif historiqueTarif = new HistoriqueTarif();
            historiqueTarif.DateChangement = typeTravauxDto.DateCreation;
            historiqueTarif.IdTypeTravaux = typeTravaux.IdTypeTravaux;
            historiqueTarif.Tarif = typeTravauxDto.Tarif;
            _dbContext.HistoriqueTarifs.Add(historiqueTarif);
            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }

        return await this.GetTypeTravaux(typeTravaux.IdTypeTravaux);
    }

    public async Task<TypeTravaux> GetTypeTravaux(int id)
    {
        TypeTravaux result = await _dbContext.TypeTravaux
            .Where(tt => tt.IdTypeTravaux == id)
            .Select(tt => new TypeTravaux {
                IdTypeTravaux = tt.IdTypeTravaux,
                Code = tt.Code,
                Designation = tt.Designation,
                HasResultat = tt.HasResultat,
                IdDepartement = tt.IdDepartement,
                Departement = tt.Departement,
                Tarif = _dbContext.HistoriqueTarifs
                    .Where(ht => ht.IdTypeTravaux == tt.IdTypeTravaux)
                    .OrderByDescending(ht => ht.DateChangement)
                    .Select(ht => ht.Tarif)
                    .FirstOrDefault()
            })
            .FirstAsync();

            return result;
    }

    public async Task<TypeTravaux> UpdateTypeTravaux(int id, TypeTravauxDto typeTravauxDto)
    {
        TypeTravaux typeTravaux = await this.GetTypeTravaux(id);
        TypeTravaux typeTravauxToUpdate = new TypeTravaux();
        typeTravauxToUpdate = typeTravauxToUpdate.FromDto(typeTravauxDto);
        using(var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try{
                _dbContext.Update(typeTravauxToUpdate);
                await _dbContext.SaveChangesAsync();

                if(typeTravauxDto.Tarif != typeTravaux.Tarif)
                {
                    HistoriqueTarif historiqueTarif = new HistoriqueTarif();
                    historiqueTarif.DateChangement = typeTravauxDto.DateCreation;
                    historiqueTarif.IdTypeTravaux = id;
                    historiqueTarif.Tarif = typeTravauxDto.Tarif;

                    _dbContext.HistoriqueTarifs.Add(historiqueTarif);
                    await _dbContext.SaveChangesAsync();
                }
                await transaction.CommitAsync();
            }catch (Exception){
                transaction.Rollback();
                throw;
            }
        }

        return await this.GetTypeTravaux(id);
    }
}