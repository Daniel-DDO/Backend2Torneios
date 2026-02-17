using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend2Torneios.Models;

[Table("palpites")] 
public class Palpite
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [Column("partida_id")]
    public string PartidaId { get; set; } = string.Empty;

    [Required]
    [Column("jogador_id")]
    public string JogadorId { get; set; } = string.Empty;

    [Column("placar_mandante")]
    public int PlacarMandante { get; set; }

    [Column("placar_visitante")]
    public int PlacarVisitante { get; set; }

    [Column("data_palpite")]
    public DateTime DataPalpite { get; set; } = DateTime.UtcNow;
}