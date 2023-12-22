namespace BookStoreAPI.Entities
{
    public class Book:BaseEntity
    {
        public string Name {  get; set; }
        public double SalePrice { get; set; }
        public double CostPrice {  get; set; }
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
    }
}
