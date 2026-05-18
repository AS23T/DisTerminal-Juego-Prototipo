namespace pryLPWeb_DisTerminal.Models
{
    public class MecanicaJuego
    {
        
        public readonly string[] DirectoriosPermitidos = { "Docs", "Img", "Pdf", "Gif", "Vid" };

        
        public string EsconderEstrella()
        {
            Random rnd = new Random();
            
            int indice = rnd.Next(DirectoriosPermitidos.Length);
            return DirectoriosPermitidos[indice];
        }


        public bool ValidarSintax(string comando, string username, out string destino)
        {

            destino = string.Empty;

            if (string.IsNullOrWhiteSpace(comando))
            {
                
                return false;
            }

            string prefijoEsperado = $"{username}/Viajar/";


            if (comando.StartsWith(prefijoEsperado))
            {

                destino = comando.Substring(prefijoEsperado.Length).Trim();


                return true;
            }
            
            return false;
        }
    }
}
