using CRUD_Class.Extensions;
using CRUD_Class.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CRUD_Class.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        public readonly AppDbContext _context;
        public readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Sliders> sliders = _context.Sliders.ToList();
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Sliders sliders)
        {
            if (!ModelState.IsValid) return View(sliders);
            if (sliders.ImageFile != null)
            {
                if (sliders.ImageFile.ContentType != "image/png" && sliders.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "can only upload .png or .jpeg");
                    return View();
                }

                if (sliders.ImageFile.Length > 1048576)
                {
                    ModelState.AddModelError("ImageFile", "file size must be lower than");
                    return View();
                }

                sliders.ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/sliders", sliders.ImageFile);
            }
            else
            {
                ModelState.AddModelError("ImageFile", "Required!");
                return View();
            }

            _context.Sliders.Add(sliders);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Update(int id)
        {
            Sliders existsliders = _context.Sliders.FirstOrDefault(x => x.Id == id);
            if (existsliders == null) return NotFound();

            return View(existsliders);
        }
        [HttpPost]
        public IActionResult Update(Sliders sliders)
        {
            Sliders existSliders = _context.Sliders.FirstOrDefault(x => x.Id == sliders.Id);
            if (existSliders == null) return NotFound();

            if (!ModelState.IsValid) return View();
            if (sliders.ImageFile != null)
            {
                if (sliders.ImageFile.ContentType != "image/png" && sliders.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "can only upload .png or .jpeg 1mb");
                    View();
                }

                if (sliders.ImageFile.Length > 1048576)
                {
                    ModelState.AddModelError("ImageFile", "file size must be lower than");
                    View();
                }

                string deletePath = Path.Combine(_env.WebRootPath, "uploads/sliders", existSliders.ImageUrl);

                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
                existSliders.ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/sliders", sliders.ImageFile);
            }

            existSliders.Title = sliders.Title;
            existSliders.Description = sliders.Description;
            existSliders.RedirectUrl = sliders.RedirectUrl;
            existSliders.RedirectText = sliders.RedirectText;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Sliders sliders = _context.Sliders.FirstOrDefault(x => x.Id == id);
            if (sliders == null) return NotFound();
            return View(sliders);
        }
        [HttpPost]
        public IActionResult Delete(Sliders sliders)
        {
            Sliders existSliders = _context.Sliders.FirstOrDefault(x => x.Id == sliders.Id);
            if (existSliders == null) return NotFound();

            _context.Sliders.Remove(existSliders);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
