using CRUD_Class.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Class.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class GenreController : Controller
    {
        public readonly AppDbContext _context;
        public GenreController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Genre> genres = _context.Genres.ToList();

            return View(genres);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Genre genre)
        {
            if (!ModelState.IsValid) return View();

            if (_context.Genres.Any(x => x.Name.ToLower() == genre.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Genre already exist!");
                return View();
            }

            _context.Genres.Add(genre);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Update(int id)
        {
            Genre genre = _context.Genres.FirstOrDefault(x => x.Id == id);
            if (genre == null) return NotFound();

            return View(genre);
        }
        [HttpPost]
        public IActionResult Update(Genre genre)
        {
            if (!ModelState.IsValid) return View();
            Genre existGenre = _context.Genres.FirstOrDefault(x=>x.Id==genre.Id);
            if (existGenre == null) return NotFound();

            if (_context.Genres.Any(x=>x.Id!=genre.Id && x.Name.ToLower() == genre.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Genre already exist!");
                return View();
            }

            existGenre.Name = genre.Name;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Genre genre=_context.Genres.FirstOrDefault(x=>x.Id == id);
            if (genre == null) return NotFound();
            return View(genre);
        }
        [HttpPost]
        public IActionResult Delete(Genre genre)
        {
            Genre existGenre = _context.Genres.FirstOrDefault(x => x.Id == genre.Id);
            if (existGenre == null) return NotFound();

            _context.Genres.Remove(existGenre);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
