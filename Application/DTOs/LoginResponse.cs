namespace API_ShoesShop.Application.DTOs
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }

        public required string RefreshToken { get; set; }
    }
}
