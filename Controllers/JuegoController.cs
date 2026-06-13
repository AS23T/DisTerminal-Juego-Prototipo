using Microsoft.AspNetCore.Mvc;
using pryLPWeb_DisTerminal.Data;
using pryLPWeb_DisTerminal.Models;

namespace pryLPWeb_DisTerminal.Controllers
{
    public class JuegoController : Controller
    {
        private readonly MecanicaJuego _mecanica = new MecanicaJuego();
        private readonly TerminalDbContext _context;

        public JuegoController(TerminalDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string jugadorActual = Request.Cookies["Jugador"] ?? "";
            if (string.IsNullOrEmpty(jugadorActual)) return RedirectToAction("Index", "Home");
            {

            }

            ViewBag.JugadorActual = jugadorActual;
            ViewBag.ReiniciarTimer = true;
            return View();
        }

        [HttpPost]
        public IActionResult Index(string Comando, int TiempoSegundos)
        {
            string jugadorActual = Request.Cookies["Jugador"] ?? "";
            string ubicacionSecreta = Request.Cookies["UbicacionEstrella"] ?? "";
            bool estrellaEncontrada = Request.Cookies["EstrellaEncontrada"] == "true";

            if (string.IsNullOrEmpty(jugadorActual)) return RedirectToAction("Index", "Home");
            {

            }

            ViewBag.JugadorActual = jugadorActual;

            if (_mecanica.ValidarSintax(Comando, jugadorActual, out string destino))
            {
                if (destino == "ESTRELLA")
                {
                    if (estrellaEncontrada)
                    {
                        var jugadorBD = _context.Jugadores.FirstOrDefault(j => j.NombreUsuario == jugadorActual);
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
                        TempData["Mensaje"] = $"¡DIRECTORIO {destino} ESCANEADO! Has encontrado la carpeta oculta, ingresa el comando {jugadorActual}/Viajar/ESTRELLA para ganar";
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
                TempData["Mensaje"] = "Error de sintax, el comando debe ser: " + jugadorActual + "/Viajar/[Directorio]";
                TempData["ColorMensaje"] = "#FF0000";
            }

            return View();
        }
    }
}
