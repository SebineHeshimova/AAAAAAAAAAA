namespace AllUpFTB.Models
{
    public class ProductImage:BaseEntity
    {
        public string ImageUrl { get; set; }
        public bool? IsPoster { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
