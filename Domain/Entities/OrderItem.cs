using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ShoesShop.Domain.Entities
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        [JsonIgnore]
        public Guid OrderId { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }
        [JsonIgnore]
        public int ProductDetailId { get; set; }

        [ForeignKey(nameof(ProductDetailId))]
        public ProductDetail ProductDetail { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        [NotMapped]
        public decimal Total => UnitPrice * Quantity;
    }
}
