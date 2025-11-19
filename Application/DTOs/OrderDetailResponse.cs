using ShoesShop.Domain.Entities;

namespace ShoesShop.Application.DTOs
{
    public class OrderDetailResponse
    {
        public Guid OrderId { get; set; }
        public string UserId { get; set; }

        public string Fullname { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime CreateAt { get; set; }
        public Order.OrderStatus Status { get; set; }
        public List<OrderItemDetail> OrderItems { get; set; }

        public decimal TotalPrice { get; set; }


        public string PhoneNumber { get; set; }
    }
}
