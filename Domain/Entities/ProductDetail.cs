using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ShoesShop.Domain.Entities
{
    public class ProductDetail
    {
        public int ProductDetailId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [JsonIgnore]
        public Product Product { get; set; }
        [ForeignKey("Color")]
        public int ColorId { get; set; }
        public Color Color { get; set; }
        [ForeignKey("Size")]
        public int SizeId { get; set; }
        public Size Size { get; set; }
        public decimal Price { get; set; } 
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }
    }
}
