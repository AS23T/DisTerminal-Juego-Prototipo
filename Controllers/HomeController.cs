using Microsoft.AspNetCore.Mvc;
using pryLPWeb_DisTerminal.Data;
using pryLPWeb_DisTerminal.Models;

namespace pryLPWeb_DisTerminal.Controllers
{
    public class HomeController : Controller
    {
        private readonly TerminalDbContext _context;

        public HomeController(TerminalDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Response.Cookies.Delete("Jugador");
            Response.Cookies.Delete("UbicacionEstrella");
            Response.Cookies.Delete("EstrellaEncontrada");
            return View();
        }

        [HttpPost]
        public IActionResult Index(string Username)
        {
            if (!string.IsNullOrEmpty(Username))
            {
                var jugadorDB = _context.Jugadores.FirstOrDefault(j => j.NombreUsuario == Username);

                if (jugadorDB != null)
                {
                    if (Username != "admin963")
                    {
                        ViewBag.Error = "Ese nombre de usuario ya esta en uso. Porfavor, elige otro.";
                        return View();
                    }
                    
                }
                else
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

                return RedirectToAction("Index", "Juego");
            }
            return View();
        }
    }
}
