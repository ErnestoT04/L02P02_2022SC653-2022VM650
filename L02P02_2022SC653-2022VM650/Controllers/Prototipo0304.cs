﻿using L02P02_2022SC653_2022VM650.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L02P02_2022SC653_2022VM650.Controllers
{
    public class Prototipo0304 : Controller
    {
        private readonly AppDbContext _context;
        public Prototipo0304(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(int libroId)
        {
            ViewData["PasoActivo"] = "3";

            // Obtener los comentarios
            var comentarios = _context.ComentariosLibros
                .Where(c => c.IdLibro == libroId)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();

            // Obtener información del libro y autor
            var libroConAutor = _context.Libros
                .Include(l => l.IdAutorNavigation)
                .FirstOrDefault(l => l.Id == libroId);

            ViewBag.LibroId = libroId;
            ViewBag.LibroNombre = libroConAutor?.Nombre ?? "Libro no encontrado";
            ViewBag.AutorNombre = libroConAutor?.IdAutorNavigation?.Autor ?? "Autor no encontrado";

            return View(comentarios);
        }

        // POST: Guardar nuevo comentario
        [HttpPost]
        public IActionResult Guardar(ComentariosLibro comentario)
        {
            var nuevoId = _context.ComentariosLibros.Any()
                   ? _context.ComentariosLibros.Max(c => c.Id) + 1
                   : 1;

            comentario.Id = nuevoId;
            comentario.CreatedAt = DateTime.Now;
            comentario.Usuario = User.Identity.Name ?? "usuario1";

            _context.ComentariosLibros.Add(comentario);
            _context.SaveChanges();

            return RedirectToAction("Confirmacion", new { id = comentario.Id });
        }

        // GET: Confirmación del comentario añadido
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
