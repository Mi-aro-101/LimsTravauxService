using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimsTravauxService.Models;

[Table("Avancee_travail")]
public class AvanceeTravail
{
    [Key]
    [Column("id_avancee_travail")]
    public int IdAvanceeTravail { get; set; }
    [Column("niveau")]
    public int Niveau { get; set; }    
    [Column("designation")]
    public string Designation { get; set; }
}