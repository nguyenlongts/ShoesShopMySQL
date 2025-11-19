namespace ShoesShop.Application.DTOs
{
    public class ProductDetailDTO
    {
        public int ProductDetailId { get; set; }
        public int ProductId { get; set; }
        public string ColorName { get; set; }    
        public string SizeName { get; set; } 
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }
    }

}
