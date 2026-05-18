using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pryLPWeb_DisTerminal.Data;
using pryLPWeb_DisTerminal.Models;
using System;
using System.Linq;

namespace pryLPWeb_DisTerminal.Pages
{
    public class JuegoModel : PageModel
    {
        public string JugadorActual { get; set; } = "";
        private readonly MecanicaJuego _mecanica = new MecanicaJuego();
        private readonly TerminalDbContext _context;

        public JuegoModel(TerminalDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            JugadorActual = Request.Cookies["Jugador"] ?? "";

            if (string.IsNullOrEmpty(JugadorActual))
            {
                return RedirectToPage("/Index");
            }

            ViewData["ReiniciarTimer"] = true;

            return Page();
        }

        
        public IActionResult OnPost(string Comando, int TiempoSegundos)
        {
            JugadorActual = Request.Cookies["Jugador"] ?? "";
            string ubicacionSecreta = Request.Cookies["UbicacionEstrella"] ?? "";
            bool estrellaEncontrada = Request.Cookies["EstrellaEncontrada"] == "true";

            if (string.IsNullOrEmpty(JugadorActual))
            {
                return RedirectToPage("/Index");
            }

            if (_mecanica.ValidarSintax(Comando, JugadorActual, out string destino))
            {
                if (destino == "ESTRELLA")
                {
                    if (estrellaEncontrada)
                    {
                        
                        var jugadorBD = _context.Jugadores.FirstOrDefault(j => j.NombreUsuario == JugadorActual);
                        if (jugadorBD != null)
                        {
                            var nuevoRegistro = new RegistroTiempo
                            {
                                JugadorId = jugadorBD.Id,
                                FechaPartida = DateTime.Now,
                                EncontroEstrella = true,
                                TiempoJugado = TimeSpan.FromSeconds(TiempoSegundos) 
                            };
                            _context.RegistrosTiempos.Add(nuevoRegistro);
                            _context.SaveChanges();
                        }
                        

                        TempData["Mensaje"] = "¡GANASTE! Has obtenido la ESTRELLA. Generando nuevo lugar...";
                        TempData["ColorMensaje"] = "#FFFF00";

                        Response.Cookies.Append("UbicacionEstrella", _mecanica.EsconderEstrella());
                        Response.Cookies.Append("EstrellaEncontrada", "false");
                    }
                    else
                    {
                        TempData["Mensaje"] = "Error: aun no has localizado la carpeta ESTRELLA en ningun directorio.";
                        TempData["ColorMensaje"] = "#FF0000";
                    }
                }
                else if (_mecanica.DirectoriosPermitidos.Contains(destino))
                {
                    if (destino == ubicacionSecreta)
                    {
                        TempData["Mensaje"] = $"¡DIRECTORIO {destino} ESCANEADO! Has encontrado la carpeta oculta, ingresa el comando {JugadorActual}/Viajar/ESTRELLA para ganar";
                        TempData["ColorMensaje"] = "#00FFFF";

                        Response.Cookies.Append("EstrellaEncontrada", "true");
                    }
                    else
                    {
                        TempData["Mensaje"] = $"Directorio {destino} escaneando... vacio, intenta en otro";
                    }
                }
                else
                {
                    TempData["Mensaje"] = $"Error, el directorio '{destino}' no existe, intenta con Docs, Img, Pdf, Gif o Vid";
                    TempData["ColorMensaje"] = "#FF0000";
                }
            }
            else
            {
                TempData["Mensaje"] = "Error de sintax, el comando debe ser: " + JugadorActual + "/Viajar/[Directorio]";
                TempData["ColorMensaje"] = "#FF0000";
            }

            return Page();
        }
    }
}