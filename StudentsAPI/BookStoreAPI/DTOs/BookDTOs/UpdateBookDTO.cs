namespace BookStoreAPI.DTOs.BookDTOs
{
    public class UpdateBookDTO
    {
        public string Name { get; set; }
        public double SalePrice { get; set; }
        public double CostPrice { get; set; }
        public int CategoryId { get; set; }
    }
}
