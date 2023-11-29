using AllUpFTB.Models;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;

namespace AllUpFTB.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BrendController : Controller
    {
        public readonly AppDbContext _context;

        public BrendController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Brend> brends=_context.Brends.ToList();
            return View(brends);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Brend brend)
        {
            if (!ModelState.IsValid) return View();

            if (_context.Brends.Any(x => x.Name.ToLower() == brend.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Genre already exist!");
                return View();
            }

            _context.Brends.Add(brend);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Update(int id)
        {
            Brend brend = _context.Brends.FirstOrDefault(x => x.Id == id);
            if (brend == null) return NotFound();

            return View(brend);
        }
        [HttpPost]
        public IActionResult Update(Brend brend)
        {
            if (!ModelState.IsValid) return View();
            Brend existBrend = _context.Brends.FirstOrDefault(x => x.Id == brend.Id);
            if (existBrend == null) return NotFound();

            if (_context.Brends.Any(x => x.Id != brend.Id && x.Name.ToLower() == brend.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Genre already exist!");
                return View();
            }

            existBrend.Name = brend.Name;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Brend brend = _context.Brends.FirstOrDefault(x => x.Id == id);
            if (brend == null) return NotFound();
            return View(brend);
        }
        [HttpPost]
        public IActionResult Delete(Brend brend)
        {
            Brend existGenre = _context.Brends.FirstOrDefault(x => x.Id == brend.Id);
            if (existGenre == null) return NotFound();

            _context.Brends.Remove(existGenre);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
