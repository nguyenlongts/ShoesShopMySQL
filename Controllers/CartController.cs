using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesShop.Application.DTOs;
using ShoesShop.Application.Interfaces.Services;
using ShoesShop.Application.Services;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart ([FromBody] AddToCartRequest request) { 
            var result = await _cartService.AddItemAsync(request);
            if (result == true)
            {
                return Ok("Thêm thành công");
            }
            return Ok("Thêm thất bại");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByUserId(string id)
        {
            var cart =await _cartService.GetByUserIdAsync(id);
            if (cart == null)
            {
                await   _cartService.CreateAsync(id);
            }
            return Ok(cart);
        }
        [HttpGet("{userId}/items")]
        public async Task<IActionResult> GetAllCartItems(string userId)
        {
            return Ok(await _cartService.GetAllCartItem(userId));
        }
        [HttpPut("items/{cartItemId}")]
        public async Task<IActionResult> UpdateQuantity ([FromBody]UpdateCIRequest request)
        {
            return Ok(await _cartService.UpdateQuantityAsync(request.UserId, request.CartItemId, request.Quantity));
        }

        [HttpDelete("items")]
        public async Task<IActionResult> RemoveItem([FromBody] RemoveCIRequest request)
        {
            var result = await _cartService.RemoveItemAsync(request.UserId, request.ProductDetailId);
            if (result)
            {
                return Ok("Xóa sản phẩm khỏi giỏ hàng thành công");
            }
            return BadRequest("Xóa sản phẩm khỏi giỏ hàng thất bại");
        }

        [HttpDelete("{userId}/clear")]
        public async Task<IActionResult> ClearCart(string userId)
        {
            var result = await _cartService.ClearCartAsync(userId);
            if (result)
            {
                return Ok("Xóa toàn bộ giỏ hàng thành công");
            }
            return BadRequest("Xóa giỏ hàng thất bại");
        }
    }
}
