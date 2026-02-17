using Backend2Torneios.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend2Torneios.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Comentario> Comentarios { get; set; }
    public DbSet<Palpite> Palpites { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Palpite>()
            .HasIndex(p => new { p.PartidaId, p.JogadorId })
            .IsUnique();
    }
}