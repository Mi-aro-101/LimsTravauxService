using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using LimsTravauxService.Dto;

namespace LimsTravauxService.Models;

[Table("type_travaux_type_echantillon")]
public class TypeTravauxTypeEchantillon
{

    [Key]
    [Column("id_type_travaux")]
    public int IdTypeTravaux { get; set; }
    [JsonIgnore]
    public TypeTravaux TypeTravaux { get; set; }

    [Column("id_type_echantillon")]
    public int IdTypeEchantillon { get; set; }
    [ForeignKey("IdTypeEchantillon")]
    public TypeEchantillon TypeEchantillon { get; set; }

}