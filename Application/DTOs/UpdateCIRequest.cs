namespace ShoesShop.Application.DTOs
{
    public class UpdateCIRequest
    {
        public string UserId { get; set; }
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
