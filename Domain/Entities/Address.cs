using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using API_ShoesShop.Domain.Entities;

namespace ShoesShop.Domain.Entities
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        [Required]
        [MaxLength(255)]
        public string FullAddress { get; set; }
        public string UserId { get; set; } 
        [JsonIgnore]
        public ApplicationUser User { get; set; }

        public bool IsDefault { get; set; } = false;
    }

}
