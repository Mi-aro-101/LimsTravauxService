using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LimsTravauxService.Utils;

namespace LimsTravauxService.Models;

[Table("Departement")]
public class Departement
{

    [Key]
    [Column("id_departement")]
    public int IdDepartement { get; set; }
    [Column("code")]
    public string? Code { get; set; }
    [Column("designation")]
    public string? Designation { get; set; }
}