namespace ProductsWebApi.Models
{
    public class CreateProductRequestModel
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public float AvailableAmount { get; set; }
        public string? Description { get; set; }
    }
}
