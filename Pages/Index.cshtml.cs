using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pryLPWeb_DisTerminal.Models;
using pryLPWeb_DisTerminal.Data;
using System;
using System.Linq;

namespace pryLPWeb_DisTerminal.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly TerminalDbContext _context;

        public IndexModel(ILogger<IndexModel> logger, TerminalDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        
        public void OnGet()
        {
            Response.Cookies.Delete("Jugador");
            Response.Cookies.Delete("UbicacionEstrella");
            Response.Cookies.Delete("EstrellaEncontrada");
        }

        
        public IActionResult OnPost(string Username)
        {

            if (!string.IsNullOrEmpty(Username))
            {

                var jugadorDB = _context.Jugadores.FirstOrDefault(j => j.NombreUsuario == Username);
                if (jugadorDB == null)
                {
                    jugadorDB = new Jugador
                    {
                        NombreUsuario = Username,
                        FechaRegistro = DateTime.Now
                    };
                    _context.Jugadores.Add(jugadorDB);
                    _context.SaveChanges();
                }

                Response.Cookies.Append("Jugador", Username);

                
                var mecanica = new MecanicaJuego();
                
                Response.Cookies.Append("UbicacionEstrella", mecanica.EsconderEstrella());
                
                Response.Cookies.Append("EstrellaEncontrada", "false");
                
                return RedirectToPage("/Juego");

            }
            
            return Page();
        }
    }
}
