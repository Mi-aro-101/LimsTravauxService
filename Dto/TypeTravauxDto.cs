using System.Text.Json.Serialization;
using LimsTravauxService.Models;

namespace LimsTravauxService.Dto;

public class TypeTravauxDto
{
    [JsonPropertyName("idTypeTravaux")]
    public int IdTypeTravaux { get; set; }
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;
    [JsonPropertyName("designation")]
    public string Designation { get; set; } = string.Empty;
    [JsonPropertyName("hasResultat")]
    public int HasResultat { get; set; }
    [JsonPropertyName("idDepartement")]
    public int IdDepartement { get; set; }
    [JsonPropertyName("tarif")]
    public decimal? Tarif { get; set; }
    [JsonPropertyName("dateCreation")]
    public DateTime? DateCreation { get; set; }
    [JsonPropertyName("dateChangement")]
    public DateTime? DateChangement { get; set; }
    [JsonPropertyName("haveFormule")]
    public int HaveFormule { get; set; }
    [JsonPropertyName("formuleString")]
    public string? FormuleString { get; set; }
    [JsonPropertyName("formuleBytes")]
    public byte[]? FormuleBytes { get; set; }

    [JsonPropertyName("idtypeEchantillons")]
    public ICollection<int> IdTypeEchantillons { get; set; } = new List<int>();

}