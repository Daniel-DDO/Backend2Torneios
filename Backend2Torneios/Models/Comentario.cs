using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend2Torneios.Models;

[Table("comentarios")]
public class Comentario
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required] 
    [Column("texto", TypeName = "text")]
    public string Texto { get; set; } = string.Empty;

    [Column("jogador_id")]
    public string JogadorId { get; set; } = string.Empty;

    [Column("partida_id")]
    public string PartidaId { get; set; } = string.Empty;

    [Column("data_hora")]
    public DateTime DataHora { get; set; } = DateTime.UtcNow;
}