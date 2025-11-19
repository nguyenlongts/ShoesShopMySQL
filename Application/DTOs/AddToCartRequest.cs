namespace ShoesShop.Application.DTOs
{
    public class AddToCartRequest
    {
        public Guid UserId { get; set; }
        public int ProductDetailId { get; set; }
        public int Quantity { get; set; }
    }

}
