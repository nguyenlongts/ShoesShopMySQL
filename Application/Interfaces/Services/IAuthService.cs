using API_ShoesShop.Application.DTOs;
using API_ShoesShop.Domain.Entities;

namespace ShoesShop.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<(bool success, string message)> RegisterAsync(RegisterDTO model);
        Task<(bool success, LoginResponse response, string message)> LoginAsync(LoginDTO model);
        Task<bool> ConfirmEmailAsync(string userId, string token);
        Task<(string newAccessToken, bool isSuccess, string message)> RefreshAccessTokenAsync(string userId, string refreshToken);
    }
}
