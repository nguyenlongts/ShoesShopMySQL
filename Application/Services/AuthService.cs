using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using API_ShoesShop.Application.DTOs;
using API_ShoesShop.Domain.Entities;
using ShoesShop.Application.Interfaces.Services;
using API_ShoesShop.Application.Services;
using Azure.Core;

namespace ShoesShop.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenService _tokenService;
        private readonly IUserService _userService;

        public AuthService(UserManager<ApplicationUser> userManager, TokenService tokenService, IUserService userService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _userService = userService;
        }

        public async Task<(bool success, string message)> RegisterAsync(RegisterDTO model)
        {
            return await _userService.RegisterAsync(model);
        }

        public async Task<(bool success, LoginResponse? response,string message)> LoginAsync(LoginDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                return (false, null, "User not found");

            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
                return (false, null, "Invalid password");

            var accessToken = await _tokenService.GenerateAccessToken(user);
            if (string.IsNullOrEmpty(user.RefreshToken) || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                user.RefreshToken = await _tokenService.GenerateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);
            }
            await _userManager.UpdateAsync(user);
            return (true, new LoginResponse { AccessToken = accessToken, RefreshToken = user.RefreshToken }, "Login successful");
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            return result.Succeeded;
        }

        public async Task<(string newAccessToken, bool isSuccess, string message)> RefreshAccessTokenAsync(string userId,string refreshToken)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                return (null, false, "Invalid or expired refresh token.");
            }
            var newAccessToken = await _tokenService.GenerateAccessToken(user);

            return (newAccessToken, true, "Refresh successful");
        }
    }
}
