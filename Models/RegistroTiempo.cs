using System;

namespace pryLPWeb_DisTerminal.Models
{
    public class RegistroTiempo
    {
        public int Id { get; set; }
        public int JugadorId { get; set; }
        public TimeSpan TiempoJugado { get; set; }
        public DateTime FechaPartida { get; set; }
        public bool EncontroEstrella { get; set; }

        public Jugador Jugador { get; set; } = null!;
    }
}
