using Backend2Torneios.Data;
using Microsoft.AspNetCore.Mvc;

namespace Backend2Torneios.Controllers;

[ApiController]
[Route("/")] 
public class StatusController : ControllerBase
{
    private readonly AppDbContext _context;

    public StatusController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetStatus()
    {
        bool bancoConectado = false;
        string erroBanco = "";

        try
        {
            bancoConectado = await _context.Database.CanConnectAsync();
        }
        catch (Exception ex)
        {
            erroBanco = ex.Message;
        }

        var status = new
        {
            Sistema = "API 2 - Torneios DDO (C#)",
            Status = "Rodando",
            DataHoraServidor = DateTime.UtcNow,
            Ambiente = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
            BancoDeDados = new
            {
                Conectado = bancoConectado,
                Tipo = "PostgreSQL (Aiven)",
                Erro = string.IsNullOrEmpty(erroBanco) ? "Nenhum" : erroBanco
            }
        };

        if (!bancoConectado)
        {
            return StatusCode(503, status);
        }

        return Ok(status);
    }
}