namespace ShoesShop.Application.DTOs
{
    public class UpdateOrderStatusRequest
    {

        public Guid OrderId { get; set; }

        public int Status { get; set; }


    }
}
