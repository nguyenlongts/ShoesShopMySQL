namespace ShoesShop.Application.DTOs
{
    public class AddressDTO
    {
        public Guid UserId { get; set; }
        public string FullAddress { get; set; }
        public bool IsDefault { get; set; }
    }
}
