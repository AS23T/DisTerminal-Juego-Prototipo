using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pryLPWeb_PaginaPrototipo.Models;
using System.Linq;
using System.Net;

namespace pryLPWeb_PaginaPrototipo.Pages
{
    public class JuegoModel : PageModel
    {
        //propiedad publica para que almacena el nombre del jugador actual y es de tipo string y no permite valores nulos (por eso se inicializa con "" para evitar advertencias).
        public string JugadorActual { get; set; } = "";
        //instancia privada y de solo lectura de la clase 'MecanicaJuego' y se inicializa una vez y no puede ser reasignada despues.
        private readonly MecanicaJuego _mecanica = new MecanicaJuego();

        //Devuelve un IActionResult, lo que permite controlar la respuesta que se enviara al cliente (por Ej: mostrar la pagina) y se ejecuta cuando el usuario solicita datos de un servidor (GET).
        public IActionResult OnGet()
        {
            //al cargar la pagina, se obtiene el valor de la cookie "Jugador" y se asigna a la propiedad 'JugadorActual' y esto se hace para verificar si el jugador ha iniciado sesion previamente.
            JugadorActual = Request.Cookies["Jugador"] ?? "";

            //verifica si la variable 'JugadorActual' esta vacia o es null y esto indica que no hay un jugador activo o no se ha iniciado sesion.
            if (string.IsNullOrEmpty(JugadorActual))
            {
                //si no hay un jugador activo, se redirige al usuario a la pagina de inicio (Index) para que inicie sesion o registre su nombre.
                return RedirectToPage("/Index");
            }
            //si hay un jugador activo, se muestra la pagina actual (Juego).
            return Page();

        }

        //Este metodo se ejecuta cuando el usuario envia datos al servidor (POST) con un comando y se encarga de procesar ese comando para determinar la accion a realizar en el juego.
        public IActionResult OnPost(string Comando)
        {
            //requesteo el valor de la cookie en el [] desde la solicitud HTTP y asigno a variable.
            JugadorActual = Request.Cookies["Jugador"] ?? "";
            string ubicacionSecreta = Request.Cookies["UbicacionEstrella"] ?? "";
            //verificacion si la cookie "EstrellaEncontrada" tiene el valor "true", si es asi la variable 'estrellaEncontrada' es "true", si no existe o tiene otro valor sera "false".
            //TODO: solucion que encontre: "bool estrellaEncontrada =
            //  string.Equals(Request.Cookies["EstrellaEncontrada"], "true", StringComparison.OrdinalIgnoreCase);"
            bool estrellaEncontrada = Request.Cookies["EstrellaEncontrada"] == "true";

            //verificar si jugador esta vacio o nulo y si no hay redirige a la pagina "/Index".
            if (string.IsNullOrEmpty(JugadorActual))
            {
                return RedirectToPage("/Index");
            }

            //validar si el formato esta bien con el metodo "ValidarSintax" en la instancia "_mecanica" de la unica clase "MecanicaJuego" y parametros de comando, jugador actual y una variable de salida para el destino.
            if (_mecanica.ValidarSintax(Comando, JugadorActual, out string destino))
            {
                //si el destino es "ESTRELLA", se verifica si la estrella ha sido encontrada.
                if (destino == "ESTRELLA")
                {
                    //si la estrella ha sido encontrada.
                    if (estrellaEncontrada)
                    {
                        //se muestra un mensaje temporal, si el jugador ha encontrado la ESTRELLA.
                        TempData["Mensaje"] = "¡GANASTE! Has obtenido la ESTRELLA. Generando nuevo lugar...";
                        TempData["ColorMensaje"] = "#FFFF00"; //color amarillo

                        //se asigna una nueva ubicacion secreta con el metodo "EsconderEstrella" de la clase "MecanicaJuego" y se guarda en la cookie "UbicacionEstrella".
                        Response.Cookies.Append("UbicacionEstrella", _mecanica.EsconderEstrella());
                        //se resetea la cookie "EstrellaEncontrada" a "false" para indicar que la estrella no ha sido encontrada en la nueva ubicacion.
                        Response.Cookies.Append("EstrellaEncontrada", "false");
                    }
                    else
                    {
                        //si no ha sido encontrada, se muestra un mensaje de error indicando que el jugador aun no ha localizado la carpeta ESTRELLA en ningun directorio.
                        TempData["Mensaje"] = "Error: aun no has localizado la carpeta ESTRELLA en ningun directorio.";
                        TempData["ColorMensaje"] = "#FF0000"; //color rojo
                    }
                }
                //otro si el "destino" esta dentro los directorios permitidos, de la instancia "_mecanica" de la unica clase "MecanicaJuego" con el arreglo "DirectoriosPermitidos". 
                else if (_mecanica.DirectoriosPermitidos.Contains(destino))
                {
                    //si "destino" es igual a la "ubicacionSecreta"
                    if (destino == ubicacionSecreta)
                    {
                        //mensaje temporal de ayuda que encontro la carpeta.
                        TempData["Mensaje"] = $"¡DIRECTORIO {destino} ESCANEADO! Has encontrado la carpeta oculta, ingresa el comando {JugadorActual}/Viajar/ESTRELLA para ganar";
                        TempData["ColorMensaje"] = "#00FFFF"; //color celeste
                        //responde y guarda en cookie con "EstrellaEncontrada" y el valor "true".
                        Response.Cookies.Append("EstrellaEncontrada", "true");
                    }
                    else
                    {
                        //si no, mensaje temporal de probando el directorio.
                        TempData["Mensaje"] = $"Directorio {destino} escaneando... vacio, intenta en otro";
                    }
                }
                else
                {
                    //si no, mensaje temporal de error, valor no existe.
                    TempData["Mensaje"] = $"Error, el directorio '{destino}' no existe, intenta con Docs, Img, Pdf, Gif o Vid";
                    TempData["ColorMensaje"] = "#FF0000"; //color rojo.
                }

            }
            else
            {
                //error de la sintax, mensaje temporal concatenando "JugadorActual" con el formato correcto.
                TempData["Mensaje"] = "Error de sintax, el comando debe ser: " + JugadorActual + "/Viajar/[Directorio]";
                TempData["ColorMensaje"] = "#FF0000"; //color rojo
            }

            //despues de procesar el comando, se regresa a la pagina actual "Juego" con los mensajes correspondientes.
            return Page();
        }
    }
}
