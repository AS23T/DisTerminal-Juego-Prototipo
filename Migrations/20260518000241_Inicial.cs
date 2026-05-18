using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pryLPWeb_DisTerminal.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jugadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NombreUsuario = table.Column<string>(type: "TEXT", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jugadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrosTiempos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    JugadorId = table.Column<int>(type: "INTEGER", nullable: false),
                    TiempoJugado = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    FechaPartida = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EncontroEstrella = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosTiempos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosTiempos_Jugadores_JugadorId",
                        column: x => x.JugadorId,
                        principalTable: "Jugadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosTiempos_JugadorId",
                table: "RegistrosTiempos",
                column: "JugadorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrosTiempos");

            migrationBuilder.DropTable(
                name: "Jugadores");
        }
    }
}
