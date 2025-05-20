using LimsTravauxService.Models;

namespace LimsTravauxService.Dto;

public class TypeTravauxTypeEchantillonDto
{
    public int IdTypeTravaux {get; set; }

    public string Designation {get; set; } = string.Empty;
    public string Code {get; set; } = string.Empty;
    
}