using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API_ShoesShop.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API_ShoesShop.Application.Services
{
    public class TokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task< string > GenerateAccessToken(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireTime = DateTime.UtcNow.AddMinutes(1);
            var authClaims = new List<Claim>
            {
                new Claim("UserId",user.Id),
                new Claim("Username",user.UserName),
                new Claim("email_confirm",user.EmailConfirmed.ToString()),
                new Claim("Email",user.Email),
                new Claim("Phone",user.PhoneNumber),
                new Claim("Address",user.Address)

            };
            foreach (var role in roles)
            {
                authClaims.Add(new Claim("role", role));
            }
            var token = new JwtSecurityToken(
                _configuration["JWT:Issuer"],
                _configuration["JWT:Audience"],
                claims:authClaims,
                expires: expireTime,
                signingCredentials: creds
                );
            string accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return accessToken;
        }

        public async Task <string> GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
