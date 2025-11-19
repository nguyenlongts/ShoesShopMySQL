namespace ShoesShop.Application.DTOs
{
    public class CreateOrderItemDto
    {
        public int ProductDetailId { get; set; }
        public int Quantity { get; set; }       
        public decimal PriceAtOrder { get; set; } 
    }

}
