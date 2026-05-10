namespace pryLPWeb_PaginaPrototipo.Models
{
    public class MecanicaJuego
    {
        //arreglo de directorios permitidos para esconder la ESTRELLA
        public readonly string[] DirectoriosPermitidos = { "Docs", "Img", "Pdf", "Gif", "Vid" };

        //generar la ubicacion aleatoria para esconder la estrella
        public string EsconderEstrella()
        {
            Random rnd = new Random();
            //seleccionar un indice aleatorio del arreglo de directorios permitidos con Next (devuelve un numero entre 0 y n - 1)
            int indice = rnd.Next(DirectoriosPermitidos.Length);
            return DirectoriosPermitidos[indice];
        }

        //validar la sintax estricta del comando [Nombre]/Viajar/[Directorio]
        public bool ValidarSintax(string comando, string username, out string destino)
        {
            //inicializa la variable 'destino' como una cadena vacia para evitar errores de compilacion
            destino = string.Empty;
            //verificar que el comando no sea nulo o vacio
            if (string.IsNullOrWhiteSpace(comando))
            {
                //regreso falso si el comando es nulo o vacio
                return false;
            }
            //construir el prefijo esperado usando el nombre de usuario
            string prefijoEsperado = $"{username}/Viajar/";

            //verificar que el comando comience con el prefijo esperado y extraer el destino si es valido
            if (comando.StartsWith(prefijoEsperado))
            {
                //extraer el destino eliminando el prefijo y cualquier espacio adicional
                destino = comando.Substring(prefijoEsperado.Length).Trim();

                //regreso verdadero si el destino extraido es uno de los directorios permitidos
                return true;
            }
            //regreso falso si el comando no cumple con la sintaxis esperada
            return false;
        }
    }
}
