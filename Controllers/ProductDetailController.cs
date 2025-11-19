using API_ShoesShop.Infrastructure.DBContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoesShop.Application.DTOs;
using ShoesShop.Application.Interfaces.Services;
using ShoesShop.Application.Services;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductDetailController : ControllerBase
    {
        private readonly IProductDetailService _productDetailService;

        public ProductDetailController(IProductDetailService productDetailService)
        {
            _productDetailService = productDetailService;
        }

        [HttpGet("{productId}/variants")]
        public async Task<IActionResult> GetProductDetails(int productId)
        {
            var result = await _productDetailService.GetByProductIdAsync(productId);
            if (!result.Any()) return NotFound("Không tìm thấy biến thể.");
            return Ok(result);
        }

        [HttpGet("variants/{id}")]
        public async Task<IActionResult> GetVariantById(int id)
        {
            var productDetail = await _productDetailService.GetByIdAsync(id);
            return productDetail != null ? Ok(productDetail) : NotFound("Không tìm thấy biến thể.");
        }

        [HttpPost("variants")]
        public async Task<IActionResult> CreateProductDetail([FromForm] CreateProductDetailDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message, productDetail) = await _productDetailService.CreateProductDetailAsync(model);

            if (!success)
                return BadRequest(new { message });

            return Ok(new { message, productDetail });
        }
    }

}