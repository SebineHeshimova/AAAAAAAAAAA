namespace AllUpFTB.Models
{
    public class Brend:BaseEntity
    {
        public string Name {  get; set; }
        public List<Product>? Products { get; set; }
    }
}
