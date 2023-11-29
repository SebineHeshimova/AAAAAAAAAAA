using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_Class.Models
{
    public class Book:BaseEntity
    {
        public string Name {  get; set; }
        public string Description { get; set; }
        public double Tax { get; set; }
        public string Code { get; set; }
        public bool IsAvailable { get; set; }
        public double CostPrice {  get; set; }
        public double SalePrice {  get; set; }
        public double DiscountPercent { get; set; }
        public int GenreId {  get; set; }
        public Genre? Genre { get; set; }
        public int AuthorId {  get; set; }
        public Author? Author { get; set; }
        public List<BookTag>? BookTags { get; set; }
        [NotMapped]
        public List<int>? TagIds { get; set; }
        [NotMapped]
        public List<IFormFile>? ImageFile { get; set; }
        [NotMapped]
        public IFormFile? BookPosteImageFile { get; set; }
        [NotMapped]
        public IFormFile? BookHoverImageFile { get; set; }
        public List<BookImage>? BookImages { get; set; }
        [NotMapped]
        public List<int>? ImageIds { get; set; }
       
    }
}
