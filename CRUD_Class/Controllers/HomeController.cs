
using CRUD_Class.Models;
using CRUD_Class.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CRUD_Class.Controllers
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
            HomeViewModel model = new HomeViewModel()
            {
                Sliders = _context.Sliders.ToList(),
            };
            return View(model);
        }
    }
}