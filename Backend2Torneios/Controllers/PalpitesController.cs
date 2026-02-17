using Backend2Torneios.Data;
using Backend2Torneios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend2Torneios.Controllers;

[ApiController]
[Route("palpites")]
public class PalpitesController : ControllerBase
{
    private readonly AppDbContext _context;

    public PalpitesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{partidaId}")]
    public async Task<ActionResult<List<Palpite>>> GetPalpitesDaPartida(string partidaId)
    {
        return await _context.Palpites
                             .Where(p => p.PartidaId == partidaId)
                             .OrderByDescending(p => p.DataPalpite)
                             .ToListAsync();
    }
    
    [HttpGet("meu/{partidaId}")]
    public async Task<ActionResult<Palpite>> GetMeuPalpite(string partidaId, [FromQuery] string jogadorId)
    {
        var palpite = await _context.Palpites
            .FirstOrDefaultAsync(p => p.PartidaId == partidaId && p.JogadorId == jogadorId);

        if (palpite == null) return NotFound();

        return Ok(palpite);
    }

    [HttpPost]
    public async Task<ActionResult<Palpite>> SalvarPalpite([FromBody] Palpite palpiteRecebido)
    {
        if (string.IsNullOrEmpty(palpiteRecebido.PartidaId) || string.IsNullOrEmpty(palpiteRecebido.JogadorId))
        {
            return BadRequest("Partida e Jogador são obrigatórios.");
        }

        var palpiteExistente = await _context.Palpites
            .FirstOrDefaultAsync(p => p.PartidaId == palpiteRecebido.PartidaId && 
                                      p.JogadorId == palpiteRecebido.JogadorId);

        if (palpiteExistente != null)
        {
            palpiteExistente.PlacarMandante = palpiteRecebido.PlacarMandante;
            palpiteExistente.PlacarVisitante = palpiteRecebido.PlacarVisitante;
            palpiteExistente.DataPalpite = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(palpiteExistente);
        }
        else
        {
            if (string.IsNullOrEmpty(palpiteRecebido.Id)) palpiteRecebido.Id = Guid.NewGuid().ToString();
            palpiteRecebido.DataPalpite = DateTime.UtcNow;

            _context.Palpites.Add(palpiteRecebido);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMeuPalpite), 
                new { partidaId = palpiteRecebido.PartidaId, jogadorId = palpiteRecebido.JogadorId }, 
                palpiteRecebido);
        }
    }
    
    [HttpDelete("{partidaId}")]
    public async Task<IActionResult> DeletarPalpite(string partidaId, [FromQuery] string jogadorId)
    {
        var palpite = await _context.Palpites
            .FirstOrDefaultAsync(p => p.PartidaId == partidaId && p.JogadorId == jogadorId);

        if (palpite == null)
        {
            return NotFound(new { mensagem = "Nenhum palpite encontrado para excluir." });
        }

        _context.Palpites.Remove(palpite);
        await _context.SaveChangesAsync();

        return NoContent(); 
    }
}