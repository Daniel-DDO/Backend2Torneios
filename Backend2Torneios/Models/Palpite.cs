using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend2Torneios.Models;

[Table("Palpite")]
public class Palpite
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Column("partida_id")]
    public string PartidaId { get; set; } = String.Empty;
    
    [Column("jogador_id")]
    public string JogadorId { get; set; } = String.Empty;
    
    
    
}