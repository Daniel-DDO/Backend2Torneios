using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend2Torneios.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "comentarios",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    texto = table.Column<string>(type: "text", nullable: false),
                    jogador_id = table.Column<string>(type: "text", nullable: false),
                    partida_id = table.Column<string>(type: "text", nullable: false),
                    data_hora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comentarios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "palpites",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    partida_id = table.Column<string>(type: "text", nullable: false),
                    jogador_id = table.Column<string>(type: "text", nullable: false),
                    placar_mandante = table.Column<int>(type: "integer", nullable: false),
                    placar_visitante = table.Column<int>(type: "integer", nullable: false),
                    data_palpite = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_palpites", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_palpites_partida_id_jogador_id",
                table: "palpites",
                columns: new[] { "partida_id", "jogador_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comentarios");

            migrationBuilder.DropTable(
                name: "palpites");
        }
    }
}
