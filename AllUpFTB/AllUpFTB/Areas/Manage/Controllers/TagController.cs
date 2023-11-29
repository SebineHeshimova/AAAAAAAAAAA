using AllUpFTB.Models;
using Microsoft.AspNetCore.Mvc;

namespace AllUpFTB.Areas.Admin.Controllers
{
    [Area("Manage")]
    public class TagController : Controller
    {
        public readonly AppDbContext _context;

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
            Tag tag = _context.Tags.FirstOrDefault(x => x.Id == id);
            if (tag == null) return NotFound();

            return View(tag);
        }
        [HttpPost]
        public IActionResult Update(Tag tag)
        {
            if (!ModelState.IsValid) return View();
            Tag existTag = _context.Tags.FirstOrDefault(x => x.Id == tag.Id);
            if (existTag == null) return NotFound();

            if (_context.Tags.Any(x => x.Id != tag.Id && x.Name.ToLower() == tag.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Tag already exist!");
                return View();
            }

            existTag.Name = tag.Name;
            _context.SaveChanges();
            return RedirectToAction("index");
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

