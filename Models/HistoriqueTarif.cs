using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimsTravauxService.Models;

[Table("Historique_tarif")]
public class HistoriqueTarif
{
    [Key]
    [Column("id_historique_tarif")]
    public int IdHistoriqueTarif { get; set; }
    [Column("tarif")]
    public decimal Tarif { get; set; }
    [Column("date_changement")]
    public DateTime? DateChangement { get; set; }
    [Column("id_type_travaux")]
    public int IdTypeTravaux { get; set; }
    [ForeignKey("IdTypeTravaux")]
    public TypeTravaux? TypeTravaux { get; set; }
}