using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pryLPWeb_PaginaPrototipo.Models;

namespace pryLPWeb_PaginaPrototipo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        //al cargar la pagina, se eliminan las cookies anteriores para evitar conflictos
        public void OnGet()
        {
            Response.Cookies.Delete("Jugador");
            Response.Cookies.Delete("UbicacionEstrella");
            Response.Cookies.Delete("EstrellaEncontrada");
        }

        //al enviar el formulario, se guarda el nombre del jugador y se genera la ubicacion de la ESTRELLA
        public IActionResult OnPost(string Username)
        {
            //si esto no es asi guardo el nombre del jugador en la cookie y genero la ubicacion de la estrellita, luego redirijo al juego
            if (!string.IsNullOrEmpty(Username))
            {
                //guardar el usuario en la cookie
                Response.Cookies.Append("Jugador", Username);

                //generar y inyectar la ubicacion de la estrellita al inicio
                var mecanica = new MecanicaJuego();
                //la ubicacion de la estrella se guarda en una cookie para que el juego pueda acceder a ella
                Response.Cookies.Append("UbicacionEstrella", mecanica.EsconderEstrella());
                //al iniciar el juego, la estrella no ha sido encontrada, por lo que se guarda ese estado en una cookie
                Response.Cookies.Append("EstrellaEncontrada", "false");
                //redireccionar al juego
                return RedirectToPage("/Juego");

            }
            //si esto es asi significa que el usuario no ingreso un nombre, entonces se recarga la pagina
            return Page();
        }
    }
}
