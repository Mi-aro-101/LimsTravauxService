using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimsTravauxService.Models;

[Table("Type_travaux")]
public class TypeTravaux
{
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
}