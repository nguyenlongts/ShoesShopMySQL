namespace ShoesShop.Application.DTOs
{
    public class AdminUserInfoResponse
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public bool isActive { get; set; }  
    }
}
