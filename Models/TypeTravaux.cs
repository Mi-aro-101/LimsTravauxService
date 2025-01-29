using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using LimsTravauxService.Dto;

namespace LimsTravauxService.Models;

[Table("Type_travaux")]
public class TypeTravaux
{
    public TypeTravaux FromDto(TypeTravauxDto typeTravauxDto)
    {
        string typeTravauxAsString = JsonSerializer.Serialize(typeTravauxDto);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        TypeTravaux typeTravaux = JsonSerializer.Deserialize<TypeTravaux>(typeTravauxAsString, options);

        return typeTravaux;
    }

    [Key]
    [Column("id_type_travaux")]
    public int IdTypeTravaux { get; set; }
    [Column("code")]
    public string Code { get; set; }
    [Column("designation")]
    public string Designation { get; set; }
    [Column("hasResultat")]
    public int HasResultat { get; set; }
    [Column("id_departement")]
    public int IdDepartement { get; set; }
    [ForeignKey("IdDepartement")]
    public Departement? Departement { get; set; }
    [NotMapped]
    public decimal? Tarif { get; set; }
}