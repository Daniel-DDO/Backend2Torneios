using Backend2Torneios.Data;
using Backend2Torneios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend2Torneios.Controllers;

[ApiController]
[Route("comentarios")] 
public class ComentariosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ComentariosController(AppDbContext context)
    {
        _context = context;
    }

    //GET: /comentarios/{partidaId}
    [HttpGet("{partidaId}")]
    public async Task<ActionResult<List<Comentario>>> GetPorPartida(string partidaId)
    {
        var lista = await _context.Comentarios
            .Where(c => c.PartidaId == partidaId)
            .OrderByDescending(c => c.DataHora) 
            .ToListAsync();

        return Ok(lista);
    }

    //POST: /comentarios
    [HttpPost]
    public async Task<ActionResult<Comentario>> PostComentario([FromBody] Comentario comentario)
    {
        if (string.IsNullOrEmpty(comentario.Texto))
        {
            return BadRequest("O texto do comentário é obrigatório.");
        }

        if (string.IsNullOrEmpty(comentario.Id))
        {
            comentario.Id = Guid.NewGuid().ToString();
        }
        
        comentario.DataHora = DateTime.UtcNow;

        _context.Comentarios.Add(comentario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPorPartida), new { partidaId = comentario.PartidaId }, comentario);
    }
    
    //DELETE: /comentarios/{id}?jogadorId=id-do-jogador
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComentario(string id, [FromQuery] string jogadorId)
    {
        var comentario = await _context.Comentarios.FindAsync(id);

        if (comentario == null)
        {
            return NotFound(new { mensagem = "Comentário não encontrado." });
        }

        if (comentario.JogadorId != jogadorId)
        {
            return StatusCode(403, new { mensagem = "Você não tem permissão para deletar este comentário." });
        }

        _context.Comentarios.Remove(comentario);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}