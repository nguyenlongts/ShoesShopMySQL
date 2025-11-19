namespace ShoesShop.Application.DTOs
{
    public class CreateOrderDto
    {
        public string UserId { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; } = new List<CreateOrderItemDto>();
        public string ShippingAddress { get; set; }
    }

}
