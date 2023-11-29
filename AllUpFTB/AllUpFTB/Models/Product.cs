using System.ComponentModel.DataAnnotations.Schema;

namespace AllUpFTB.Models
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code {  get; set; }
        public double ExTax {  get; set; }
        public double CostPrice {  get; set; }
        public double SalePrice {  get; set; }
        public double DiscountPercent { get; set;}
        public bool IsAvailable { get; set; }
        
        public List<ProductTag>? ProductTags { get; set; }
        [NotMapped]
        public List<int>? TagIds { get; set; }
        public int BrendId { get; set; }    
        public Brend? Brend {  get; set; }
        public List<ProductImage>? ProductImages { get; set; }
        [NotMapped]
        public List<IFormFile>? ImageFile { get; set; }
        [NotMapped]
        public IFormFile? ProductPosteImageFile { get; set; }
        [NotMapped]
        public IFormFile? ProductHoverImageFile { get; set; }
        [NotMapped]
        public List<int>? ImageIds { get; set; }

    }
}
