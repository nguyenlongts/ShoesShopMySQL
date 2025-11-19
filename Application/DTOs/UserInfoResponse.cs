namespace ShoesShop.Application.DTOs
{
    public class UserInfoResponse
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        public List<string> ShippingAddress { get; set; }
    }
}
