using BookStoreAPI.DAL;
using BookStoreAPI.DTOs.BookDTOs;
using BookStoreAPI.DTOs.CategoryDTOs;
using BookStoreAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }
       
        [HttpPost]
        public IActionResult CreateBook(CreateBookDTO createDTO)
        {
            Book book = new Book()
            {
                Name = createDTO.Name,
                SalePrice = createDTO.SalePrice,
                CostPrice = createDTO.CostPrice,
                CategoryId = createDTO.CategoryId,
            };
            book.CreatedDate = DateTime.UtcNow.AddHours(4);
            book.UpdatedDate = DateTime.UtcNow.AddHours(4);
            book.IsDeleted = false;
            _context.Books.Add(book);
            _context.SaveChanges();
            return StatusCode(201, new { message = "Object yaradildi!" });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, UpdateBookDTO bookDTO)
        {
            var existBook = _context.Books.FirstOrDefault(y => y.Id ==id);
            if (existBook == null) return NotFound();

            existBook.Name = bookDTO.Name;
            existBook.CategoryId= bookDTO.CategoryId;
            existBook.SalePrice = bookDTO.SalePrice;
            existBook.CostPrice = bookDTO.CostPrice;
            existBook.UpdatedDate= DateTime.UtcNow.AddHours(4);
            existBook.IsDeleted=false;
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("")]
        public IActionResult GetAllBook()
        {
            List<Book> book = _context.Books.Where(x=>x.IsDeleted==false).ToList();
            IEnumerable<GetBookDTO> bookDTO = new List<GetBookDTO>();
            bookDTO = book.Select(x => new GetBookDTO {Id=x.Id, Name = x.Name, CategoryId=x.CategoryId, SalePrice=x.SalePrice, CreateDate = x.CreatedDate, UpdateDate = x.UpdatedDate, IsDeleted=x.IsDeleted });
            return Ok(bookDTO);
        }

        [HttpGet("{id}")]
        public IActionResult GetByIdBook(int id)
        {
            var book = _context.Books.FirstOrDefault(x => x.Id == id);
            if (book == null) return NotFound();
            GetBookDTO dto = new GetBookDTO()
            {
                Id = book.Id,
                Name = book.Name,
                CreateDate = book.CreatedDate,
                UpdateDate = book.UpdatedDate,
                CategoryId= book.CategoryId,
                SalePrice= book.SalePrice,
            };
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book= _context.Books.FirstOrDefault(x=>x.Id == id);
            if (book == null) return NotFound();
            _context.Books.Remove(book);
            _context.SaveChanges();
            return Ok();
        }


        [HttpPut("id")]
        public IActionResult SoftDeleteBook(int id)
        {
            var book= _context.Books.FirstOrDefault(x=>x.Id==id);
            book.IsDeleted=!book.IsDeleted;
            _context.SaveChanges();
            return Ok();
        }
    }
}
