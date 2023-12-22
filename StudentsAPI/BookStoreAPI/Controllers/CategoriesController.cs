using BookStoreAPI.DAL;
using BookStoreAPI.DTOs.CategoryDTOs;
using BookStoreAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("")]
        public IActionResult CreateCategory(CreateCategoryDTO createDTO)
        {
            Category category = new Category()
            {
                Name = createDTO.Name,
            };
            category.CreatedDate = DateTime.UtcNow.AddHours(4);
            category.UpdatedDate = DateTime.UtcNow.AddHours(4);
            category.IsDeleted = false;
            _context.Categories.Add(category);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            List<Category> categories = _context.Categories.ToList();
            IEnumerable<GetCategoryDTO> categoryDTOs=new List<GetCategoryDTO>();
            categoryDTOs = categories.Select(x => new GetCategoryDTO { Name = x.Name, CreateDate = x.CreatedDate, UpdateDate=x.UpdatedDate });
            return Ok(categoryDTOs);
        }

        [HttpGet("{id}")]
        public IActionResult GetByIdCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null) return NotFound();
            GetCategoryDTO dto = new GetCategoryDTO()
            {
                Name = category.Name,
                CreateDate = category.CreatedDate
            };
            return Ok(dto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, UpdateCategoryDTO dto)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null) return NotFound();
            category.Name = dto.Name;
            category.UpdatedDate = DateTime.UtcNow.AddHours(4);
            category.IsDeleted = false;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(x=>x.Id == id);
            if (category == null) return NotFound();
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("id")]
        public IActionResult SoftDeleteCategory(int id)
        {
            var book = _context.Books.FirstOrDefault(x => x.Id == id);
            book.IsDeleted = true;
            _context.SaveChanges();
            return Ok();
        }
    }
}
