
using AllUpFTB.Models;
using AllUpFTB.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AllUpFTB.Controllers
{
    public class HomeController : Controller
    {
        public readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel= new HomeViewModel();
            homeViewModel.Sliders=_context.Sliders.ToList();
            return View(homeViewModel);
        }
        public IActionResult Create()
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if(!ModelState.IsValid) return View();


            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction("index");
        }

       
    }
}