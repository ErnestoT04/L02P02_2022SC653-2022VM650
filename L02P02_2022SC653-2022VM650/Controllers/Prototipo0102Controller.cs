using L02P02_2022SC653_2022VM650.Models;
using Microsoft.AspNetCore.Mvc;

namespace L02P02_2022SC653_2022VM650.Controllers
{
    public class Prototipo0102Controller : Controller
    {
        private readonly AppDbContext _context;

        public Prototipo0102Controller(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var autores = _context.Autores.ToList();
            return View(autores);
        }

        public IActionResult Autorseleccionado(int idAutor)
        {
            var autor = _context.Autores.FirstOrDefault(a => a.Id == idAutor);

            var librosAutor = _context.Libros.Where(libroAutor => libroAutor.IdAutor == idAutor).ToList();

            var viewModel = new AutorConLibrosViewModel
            {
                Autor = autor,
                Libros = librosAutor
            };

            return View(viewModel);
        }
    }
}