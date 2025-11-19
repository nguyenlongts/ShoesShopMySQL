namespace ShoesShop.Application.DTOs
{
    public class GetProductDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public string? Image { get; set; }
        public string BrandName { get; set; } 
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
    }
}
