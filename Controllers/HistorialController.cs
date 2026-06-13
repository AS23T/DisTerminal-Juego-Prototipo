using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pryLPWeb_DisTerminal.Data;

namespace pryLPWeb_DisTerminal.Controllers
{
    public class HistorialController : Controller
    {
        private readonly TerminalDbContext _context;

        public HistorialController(TerminalDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var registros = _context.RegistrosTiempos
                                    .Include(r => r.Jugador)
                                    .ToList()
                                    .OrderBy(r => r.TiempoJugado)
                                    .ToList();

            ViewBag.JugadorActual = Request.Cookies["Jugador"] ?? "";
            return View(registros);
        }

        [HttpPost]
        public IActionResult EliminarRecord(int id)
        {
            var record = _context.RegistrosTiempos.Find(id);
            if (record != null)
            {
                _context.RegistrosTiempos.Remove(record);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult LimpiarMiHistorial()
        {
            string jugadorActual = Request.Cookies["Jugador"] ?? "";
            var jugadorDB = _context.Jugadores.FirstOrDefault(j => j.NombreUsuario == jugadorActual);

            if (jugadorDB != null)
            {
                var misRecords = _context.RegistrosTiempos.Where(r => r.JugadorId == jugadorDB.Id);
                _context.RegistrosTiempos.RemoveRange(misRecords);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult LimpiarTodoHistorial()
        {
            string jugadorActual = Request.Cookies["Jugador"] ?? "";

            if (jugadorActual == "admin963")
            {
                var todosLosRecords = _context.RegistrosTiempos.ToList();

                _context.RegistrosTiempos.RemoveRange(todosLosRecords);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }

}
