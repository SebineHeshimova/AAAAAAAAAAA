using CRUD_Class.Models;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Class.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AuthorController : Controller
    {
        private readonly AppDbContext _context;

        public AuthorController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Author> authors = _context.Authors.ToList();
            return View(authors);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Author author)
        {
            if (!ModelState.IsValid) return View();
            
            if (_context.Authors.Any(x => x.FullName.ToLower().Trim() == author.FullName.ToLower().Trim()))
            {
                ModelState.AddModelError("FullName", "Author already exist!");
                return View();
            }

            _context.Authors.Add(author);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Update(int id)
        {
            Author author = _context.Authors.FirstOrDefault(x => x.Id == id);
            if (author == null) return NotFound();
            return View(author);
        }
        [HttpPost]
        public IActionResult Update(Author author)
        {
            if (!ModelState.IsValid) return View();
            Author existAuthor = _context.Authors.FirstOrDefault(x => x.Id == author.Id);
            if (existAuthor == null) return NotFound();

            if (_context.Authors.Any(x => x.Id != author.Id && x.FullName.ToLower()==author.FullName.ToLower()))
            {
                ModelState.AddModelError("FullName", "Genre already exist!");
                return View();
            }

            existAuthor.FullName = author.FullName;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Author author= _context.Authors.FirstOrDefault(x => x.Id == id);
            if (author == null) return NotFound();
            return View(author);
        }
        [HttpPost]
        public IActionResult Delete(Author author)
        {
            Author existAuthor = _context.Authors.FirstOrDefault(x => x.Id == author.Id);
            if (existAuthor == null) return NotFound();

            _context.Authors.Remove(existAuthor);
            _context.SaveChanges();
            return RedirectToAction("index");   
        }
    }
}
