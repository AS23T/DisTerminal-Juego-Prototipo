using Microsoft.EntityFrameworkCore;
using pryLPWeb_DisTerminal.Models;

namespace pryLPWeb_DisTerminal.Data
{
    public class TerminalDbContext : DbContext
    {
        public TerminalDbContext(DbContextOptions<TerminalDbContext> options) : base(options)
        {
        }

        public DbSet<Jugador> Jugadores { get; set; }
        public DbSet<RegistroTiempo> RegistrosTiempos { get; set; }
    }
}
