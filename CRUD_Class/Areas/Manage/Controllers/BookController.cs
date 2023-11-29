using CRUD_Class.Extensions;
using CRUD_Class.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Class.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BookController : Controller
    {
        public readonly AppDbContext _context;
        public readonly IWebHostEnvironment _env;

        public BookController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Book> books = _context.Books.ToList();
            return View(books);
        }
        public IActionResult Create()
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Book book)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            if (!ModelState.IsValid) return View();
       
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
            bool check = true;
            if (book.TagIds != null)
            {
                foreach (var tagId in book.TagIds)
                {
                    if (_context.Tags.Any(x => x.Id == tagId))
                    {
                        check = false;
                        break;
                    }
                }
            }
            if (!check)
            {
                foreach (var tagId in book.TagIds)
                {
                    BookTag bookTag = new BookTag()
                    {
                        Book = book,
                        TagId = tagId
                    };
                    _context.BookTags.Add(bookTag);
                }
            }
            else
            {
                ModelState.AddModelError("TagId", "Tag not faund!");
                return View();
            }

            if (book.BookPosteImageFile != null)
            {
                if (book.BookPosteImageFile.ContentType != "image/png" && book.BookPosteImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("BookPosteImageFile", "can only upload .png or .jpeg");
                    return View();
                }

                if (book.BookPosteImageFile.Length > 1048576)
                {
                    ModelState.AddModelError("BookPosteImageFile", "file size must be lower than");
                    return View();
                }
                BookImage bookImage = new BookImage()
                {
                    Book=book,
                    ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", book.BookPosteImageFile),
                    IsPoster=true
                };
                _context.BookImages.Add(bookImage);
            }
            if (book.BookHoverImageFile != null)
            {
                if (book.BookHoverImageFile.ContentType != "image/png" && book.BookHoverImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("BookHoverImageFile", "can only upload .png or .jpeg");
                    return View();
                }

                if (book.BookHoverImageFile.Length > 1048576)
                {
                    ModelState.AddModelError("BookHoverImageFile", "file size must be lower than");
                    return View();
                }
                BookImage bookImage = new BookImage()
                {
                    Book = book,
                    ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", book.BookHoverImageFile),
                    IsPoster = false
                };
                _context.BookImages.Add(bookImage);
            }
            if (book.ImageFile != null)
            {
                foreach(var imageFile in book.ImageFile)
                {
                    if (imageFile.ContentType != "image/png" && imageFile.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("ImageFile", "can only upload .png or .jpeg");
                        return View();
                    }

                    if (imageFile.Length > 1048576)
                    {
                        ModelState.AddModelError("ImageFile", "file size must be lower than");
                        return View();
                    }
                    BookImage bookImage = new BookImage
                    {
                        Book = book,
                        ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", imageFile),
                        IsPoster = null
                    };
                    _context.BookImages.Add(bookImage);
                }
            }
            _context.Books.Add(book);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Update(int id)
        {

            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            Book existbook = _context.Books.Include(x => x.BookTags).Include(x=>x.BookImages).FirstOrDefault(x => x.Id == id);
            if (existbook == null)
            {
                return NotFound();
            };
            existbook.TagIds = existbook.BookTags.Select(bt => bt.TagId).ToList();

            return View(existbook);
        }
        [HttpPost]
        public IActionResult Update(Book book)
        {
            //return Ok(book.ImageIds);
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            if (!ModelState.IsValid) return View();

            Book existbook = _context.Books.Include(b=>b.BookImages).FirstOrDefault(b => b.Id == book.Id);
            if (existbook == null) return NotFound();
            if (!_context.Genres.Any(g => g.Id == book.GenreId))
            {
                ModelState.AddModelError("GenreId", "Genre not found!");
                return View();
            }
            if (!_context.Authors.Any(x => x.Id == book.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Author not found!");
                return View();
            }
            var existBook = _context.Books.Include(x => x.BookTags).FirstOrDefault(x => x.Id == book.Id);
            if (existBook == null)
            {
                return NotFound();
            }

            existBook.BookTags.RemoveAll(bt => !book.TagIds.Contains(bt.TagId));

            foreach (var tagId in book.TagIds.Where(t => !existBook.BookTags.Any(bt => bt.TagId == t)))
            {
                BookTag bookTag = new BookTag
                {
                    TagId = tagId
                };
                existBook.BookTags.Add(bookTag);
            }

            if (book.BookPosteImageFile != null)
            {
                if (book.BookPosteImageFile.ContentType != "image/png" && book.BookPosteImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("BookPosteImageFile", "can only upload .png or .jpeg");
                    return View();
                }

                if (book.BookPosteImageFile.Length > 1048576)
                {
                    ModelState.AddModelError("BookPosteImageFile", "file size must be lower than");
                    return View();
                }
                //string deletePath = Path.Combine(_env.WebRootPath, "uploads/books", existbook.BookPosteImageFile.ToString());

                //if (System.IO.File.Exists(deletePath))
                //{
                //    System.IO.File.Delete(deletePath);
                //}
                BookImage bookImage = new BookImage()
                {
                    Book = book,
                    ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", book.BookPosteImageFile),
                    IsPoster = true
                };
                existBook.BookImages.Add(bookImage);
            }
            if (book.BookHoverImageFile != null)
            {
                if (book.BookHoverImageFile.ContentType != "image/png" && book.BookHoverImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("BookHoverImageFile", "can only upload .png or .jpeg");
                    return View();
                }

                if (book.BookHoverImageFile.Length > 1048576)
                {
                    ModelState.AddModelError("BookHoverImageFile", "file size must be lower than");
                    return View();
                }
                //string deletePath = Path.Combine(_env.WebRootPath, "uploads/books", existbook.BookHoverImageFile.ToString());

                //if (System.IO.File.Exists(deletePath))
                //{
                //    System.IO.File.Delete(deletePath);
                //}
                BookImage bookImage = new BookImage()
                {
                    Book = book,
                    ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", book.BookHoverImageFile),
                    IsPoster = false
                };
                existBook.BookImages.Add(bookImage);
            }

            existBook.BookImages.RemoveAll(x=>!book.ImageIds.Contains(x.Id) && x.IsPoster==null);

            if (book.ImageFile != null)
            {
                foreach (var imageFile in book.ImageFile)
                {
                    if (imageFile.ContentType != "image/png" && imageFile.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("ImageFile", "can only upload .png or .jpeg");
                        return View();
                    }

                    if (imageFile.Length > 1048576)
                    {
                        ModelState.AddModelError("ImageFile", "file size must be lower than");
                        return View();
                    }
                    //string deletePath = Path.Combine(_env.WebRootPath, "uploads/books", existbook..ToString());

                    //if (System.IO.File.Exists(deletePath))
                    //{
                    //    System.IO.File.Delete(deletePath);
                    //}
                    BookImage bookImage = new BookImage
                    {
                        Book = book,
                        ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", imageFile),
                        IsPoster = null
                    };
                    existBook.BookImages.Add(bookImage);
                }
            }


            existbook.Name = book.Name;
            existbook.Description = book.Description;
            existbook.SalePrice = book.SalePrice;
            existbook.CostPrice = book.CostPrice;
            existbook.DiscountPercent = book.DiscountPercent;
            existbook.IsAvailable = book.IsAvailable;
            existbook.Tax = book.Tax;
            existbook.Code = book.Code;
            existbook.AuthorId = book.AuthorId;
            existbook.GenreId = book.GenreId;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Book existBook = _context.Books.FirstOrDefault(x => x.Id == id);
            if(existBook==null) return NotFound();
            return View(existBook);
        }
        [HttpPost]
        public IActionResult Delete(Book book)
        {
            Book existBook= _context.Books.FirstOrDefault( x => x.Id==book.Id);
            if(existBook==null) return NotFound();

            _context.Books.Remove(existBook);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
