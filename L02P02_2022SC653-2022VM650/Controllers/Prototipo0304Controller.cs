using L02P02_2022SC653_2022VM650.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L02P02_2022SC653_2022VM650.Controllers
{
    public class Prototipo0304Controller : Controller
    {
        private readonly AppDbContext _context;
        public Prototipo0304Controller(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(int libroId)
        {
            ViewData["PasoActivo"] = "3";

            var libro = _context.Libros
                .Include(l => l.IdAutorNavigation)
                .FirstOrDefault(l => l.Id == libroId);

            var comentarios = _context.ComentariosLibros
                .Where(c => c.IdLibro == libroId)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();

            ViewBag.LibroNombre = libro?.Nombre;
            ViewBag.AutorNombre = libro?.IdAutorNavigation?.Autor;
            ViewBag.LibroId = libroId;

          
            ViewBag.ListaComentarios = comentarios;

            return View(new ComentariosLibro { IdLibro = libroId });
        }

     
        [HttpPost]
        public IActionResult Guardar(ComentariosLibro comentario)
        {
            var nuevoId = _context.ComentariosLibros.Any()
                 ? _context.ComentariosLibros.Max(c => c.Id) + 1
                 : 1;
            comentario.Id = nuevoId;
            comentario.CreatedAt = DateTime.Now;
            comentario.Usuario = User.Identity?.Name ?? "usuario1"; 
            _context.ComentariosLibros.Add(comentario);
            _context.SaveChanges();

            return RedirectToAction("Confirmacion", new { id = comentario.Id });
        }

  
        public IActionResult Confirmacion(int id)
        {
            ViewData["PasoActivo"] = "4";

            var comentario = _context.ComentariosLibros
                .Include(c => c.IdLibroNavigation)
                .FirstOrDefault(c => c.Id == id);

            return View(comentario);
        }
    }
}
