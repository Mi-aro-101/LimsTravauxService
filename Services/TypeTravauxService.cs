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
        List<TypeTravaux> results = await _dbContext.TypeTravaux
            .Select(tt => new TypeTravaux {
                IdTypeTravaux = tt.IdTypeTravaux,
                Code = tt.Code,
                Designation = tt.Designation,
                HasResultat = tt.HasResultat,
                IdDepartement = tt.IdDepartement,
                Departement = tt.Departement,
                DateCreation = tt.DateCreation,
                HaveFormule = tt.HaveFormule,
                Tarif = _dbContext.HistoriqueTarifs
                    .Where(ht => ht.IdTypeTravaux == tt.IdTypeTravaux)
                    .OrderByDescending(ht => ht.IdHistoriqueTarif)
                    .Select(ht => ht.Tarif)
                    .First(),
                TypeTravauxTypeEchantillons =  _dbContext.TypeTravauxTypeEchantillons
                    .Where(te => te.IdTypeTravaux == tt.IdTypeTravaux)
                    .Include(te => te.TypeEchantillon)
                    .ToList()
            })
            .OrderByDescending(tt => tt.IdTypeTravaux)
            .Skip(skiped).Take(size)
            .ToListAsync();
        
        foreach(TypeTravaux typetravaux in results)
        {
            if(typetravaux.HaveFormule == 1)
            {
                typetravaux.LoadFormule();
            }
        }

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
            if(typeTravauxDto.Tarif.HasValue){ historiqueTarif.Tarif = typeTravauxDto.Tarif.Value; }
            else{ historiqueTarif.Tarif = 0; }
            _dbContext.HistoriqueTarifs.Add(historiqueTarif);

            //create assignation typeTravaux_typeEchantillon and save in the table "Type_travaux_typeEchantillon
            foreach (int idTypeEchantillon in typeTravauxDto.IdTypeEchantillons)
            {
                _dbContext.TypeTravauxTypeEchantillons.Add(new TypeTravauxTypeEchantillon {IdTypeEchantillon = idTypeEchantillon, IdTypeTravaux = typeTravaux.IdTypeTravaux}); 
            }
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
                DateCreation = tt.DateCreation,
                HaveFormule = tt.HaveFormule,
                Tarif = _dbContext.HistoriqueTarifs
                    .Where(ht => ht.IdTypeTravaux == tt.IdTypeTravaux)
                    .OrderByDescending(ht => ht.IdHistoriqueTarif)
                    .Select(ht => ht.Tarif)
                    .FirstOrDefault(),
                HistoriqueTarifs = tt.HistoriqueTarifs
                    .Where(ht => ht.IdTypeTravaux == tt.IdTypeTravaux)
                    .OrderByDescending(ht => ht.IdHistoriqueTarif)
                    .ToList(),
                TypeTravauxTypeEchantillons =  _dbContext.TypeTravauxTypeEchantillons
                    .Where(te => te.IdTypeTravaux == tt.IdTypeTravaux)
                    .Include(te => te.TypeEchantillon)
                    .ToList()
            })
            .FirstAsync();

            if(result.HaveFormule == 0)
            {
                result.LoadFormule();
            }

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
                    historiqueTarif.DateChangement = typeTravauxDto.DateChangement;
                    historiqueTarif.IdTypeTravaux = id;
                    if(typeTravauxDto.Tarif.HasValue){ historiqueTarif.Tarif = typeTravauxDto.Tarif.Value; }
                    else{ historiqueTarif.Tarif = 0; }

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

    public async Task<List<TypeTravaux>> GetAllTypesTravaux()
    {
        List<TypeTravaux> results = await _dbContext.TypeTravaux.ToListAsync();
        return results;
    }

    public async Task<TypeTravaux[]> SearchTypeTravaux(string search)
    {
        TypeTravaux[] results = await _dbContext.TypeTravaux
            .Where(tt => tt.Designation.Contains(search) || tt.Code.Contains(search))
            .Take(6)
            .ToArrayAsync();
        return results;
    }
}