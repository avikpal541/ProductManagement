namespace ProductManagement.Models
{
    public class Product
    {
        public required string title { get; set; }
        public required int price { get; set; }
        public List<string>? sizes { get; set; }
        public required string description { get; set; }
    }
}
