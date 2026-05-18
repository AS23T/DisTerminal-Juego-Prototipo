using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pryLPWeb_DisTerminal.Data;
using pryLPWeb_DisTerminal.Models;
using System.Collections.Generic;
using System.Linq;

namespace pryLPWeb_DisTerminal.Pages
{
    public class HistorialModel : PageModel
    {
        private readonly TerminalDbContext _context;

        public HistorialModel(TerminalDbContext context)
        {
            _context = context;
        }

        public IList<RegistroTiempo>? Registros { get; set; }

        public void OnGet()
        {
            Registros = _context.RegistrosTiempos
                                .Include(r => r.Jugador)
                                .ToList()
                                .OrderBy(r => r.TiempoJugado)
                                .ToList();
        }
    }
}
