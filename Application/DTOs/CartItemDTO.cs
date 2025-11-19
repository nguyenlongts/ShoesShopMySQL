namespace ShoesShop.Application.DTOs
{
    public class CartItemDTO
    {
        public int ProductId { get; set; }
        public int CartItemId { get; set; }
        public int MaxQuantity { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total => Quantity * Price;
        public int ProductDetailId { get; set; }
        public string ImageUrl { get; set; }
    }

}
