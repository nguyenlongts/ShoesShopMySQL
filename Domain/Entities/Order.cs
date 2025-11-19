using System.Text.Json.Serialization;
using API_ShoesShop.Domain.Entities;

namespace ShoesShop.Domain.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; }= Guid.NewGuid();
        [JsonIgnore]
        public string UserId { get; set; }
        
        public ApplicationUser User { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        
        public decimal TotalPrice => OrderItems.Sum(item => item.Total);
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public string ShippingAddress { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public enum OrderStatus:int
        {
            Pending,
            Processing,
            Shipping,
            Completed,
            Cancelled
        }
    }
}
