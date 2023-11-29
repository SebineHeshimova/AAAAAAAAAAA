using CRUD_Class.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Class.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class TagController : Controller
    {
        private readonly AppDbContext _context;
        public TagController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Tag> tags = _context.Tags.ToList();
            return View(tags);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Tag tag)
        {
            if (!ModelState.IsValid) return View();

            if (_context.Tags.Any(x => x.Name.ToLower() == tag.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Tag already exist!");
                return View();
            }

            _context.Tags.Add(tag);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Update(int id)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            Book book = _context.Books.Include(eb => eb.BookTags).FirstOrDefault(b => b.Id == id);
            book.TagIds = _context.BookTags.Where(bt => bt.BookId == id).Select(x => x.TagId).ToList();

            if (book == null) return NotFound();
            return View(book);
        }
        [HttpPost]
        public IActionResult Update(Book book)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            if (!ModelState.IsValid) return View(book);
            if (!_context.Authors.Any(x => x.Id == book.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Author not found!");
                return View();
            }
            if (!_context.Genres.Any(x => x.Id == book.GenreId))
            {
                ModelState.AddModelError("GenreId", "Genre not found!");
                return View();
            }
            Book existBook = _context.Books.Include(eb => eb.BookTags).FirstOrDefault(b => b.Id == book.Id);
            existBook.BookTags.RemoveAll(eb => !book.TagIds.Any(ti => eb.TagId == ti));
            foreach (var tagId in book.TagIds.Where(ti => !existBook.BookTags.Any(eb => eb.TagId == ti)))
            {
                BookTag bookTag = new BookTag()
                {
                    TagId = tagId
                };
                existBook.BookTags.Add(bookTag);

            }

            if (existBook == null) return NotFound();
            existBook.Name = book.Name;
            existBook.Description = book.Description;
            existBook.CostPrice = book.CostPrice;
            existBook.SalePrice = book.SalePrice;
            existBook.DiscountPercent = book.DiscountPercent;
            existBook.Code = book.Code;
            existBook.Genre = book.Genre;
            existBook.Author = book.Author;
            existBook.Tax = book.Tax;
            _context.Books.Add(existBook);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Tag tag = _context.Tags.FirstOrDefault(x => x.Id == id);
            if (tag == null) return NotFound();
            return View(tag);
        }
        [HttpPost]
        public IActionResult Delete(Tag tag)
        {
            Tag existTag = _context.Tags.FirstOrDefault(x => x.Id == tag.Id);
            if (existTag == null) return NotFound();

            _context.Tags.Remove(existTag);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
