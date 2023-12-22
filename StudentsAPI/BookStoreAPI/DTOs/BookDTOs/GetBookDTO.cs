namespace BookStoreAPI.DTOs.BookDTOs
{
    public class GetBookDTO
    {
        public int Id { get; set; }      
        public int CategoryId {  get; set; }
        public string Name { get; set; }
        public double SalePrice {  get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsDeleted { get; set; }

    }
}
