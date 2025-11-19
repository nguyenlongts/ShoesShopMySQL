using System.ComponentModel.DataAnnotations;

namespace ShoesShop.Domain.Entities
{
    public class Color
    {
        [Key]
        public int ColorId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
