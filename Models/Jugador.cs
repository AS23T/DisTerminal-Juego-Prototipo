using System;
using System.Collections.Generic;

namespace pryLPWeb_DisTerminal.Models
{
    public class Jugador
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }

        public List<RegistroTiempo> RegistrosTiempos { get; set; } = new List<RegistroTiempo>();
    }
}
