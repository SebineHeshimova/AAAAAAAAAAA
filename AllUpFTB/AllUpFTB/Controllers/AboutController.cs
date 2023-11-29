using Microsoft.AspNetCore.Mvc;

namespace AllUpFTB.Controllers
{
    public class AboutController : Controller
    {
        public readonly AppDbContext _context;
        public AboutController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            return View();
        }
    }
}
