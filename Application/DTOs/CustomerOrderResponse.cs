using ShoesShop.Domain.Entities;

namespace ShoesShop.Application.DTOs
{
    public class CustomerOrderResponse
    {
        public string OrderId { get; set; }  // ID của đơn hàng
        public string UserId { get; set; }   // ID của người đặt hàng
        public DateTime OrderDate { get; set; } // Ngày đặt hàng
        public decimal TotalPrice { get; set; } // Tổng tiền đơn hàng
        public List<OrderItemDetail> OrderItems { get; set; }
        public Order.OrderStatus Status { get; set; }
    }
}
