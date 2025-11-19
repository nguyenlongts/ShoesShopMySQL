namespace ShoesShop.Application.DTOs
{
    public class ProductFilterRequest
    {
        public List<int>? Brands { get; set; }
        public List<int>? Sizes { get; set; }
        public List<int>? Colors { get; set; }
        public string? PriceRange { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
