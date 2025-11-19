using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShoesShop.Domain.Entities
{
    public class Size
    {
        [Key]
        public int SizeId { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; } = true;
        [JsonIgnore]
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
