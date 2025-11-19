using API_ShoesShop.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using ShoesShop.Application.DTOs;
using ShoesShop.Application.Interfaces.Services;

namespace API_ShoesShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var (success, message) = await _authService.RegisterAsync(model);
            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var (success, response, message) = await _authService.LoginAsync(model);
            if (!success)
                return Unauthorized(new { message });

            return Ok(new { response, message });
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh( RefreshAccessTokenDTO model)
        {
            var result = await _authService.RefreshAccessTokenAsync(model.UserId,model.RefreshToken);

            if (!result.isSuccess)
            {
                return Unauthorized(result.message);
            }

            return Ok(new
            {
                AccessToken = result.newAccessToken
            });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var success = await _authService.ConfirmEmailAsync(userId, token);
            if (!success)
                return BadRequest("Xác nhận email thất bại.");

            return Ok("Xác nhận email thành công!");
        }
    }
}
