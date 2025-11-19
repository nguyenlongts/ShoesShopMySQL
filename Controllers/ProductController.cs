using API_ShoesShop.Infrastructure.DBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesShop.Application.DTOs;
using ShoesShop.Application.Interfaces.Services;

namespace ShoesShop.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        //[Authorize]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAllAdmin(int pageSize=5, int pageNum=1) {
            var response = await _productService.GetAllAdminAsync(pageSize, pageNum);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest("Get Failed");
        }
        [HttpGet("customer")]
        public async Task<IActionResult> GetHomeProduct(int pageSize = 5, int pageNum = 1)
        {
            var response = await _productService.GetProductsCustomerAsync(pageSize, pageNum);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest("Get Failed");
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest("Get Failed");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDTO model) {
            var result = await _productService.CreateAsync(model);
            if (result)
            {
                return(Ok("Create successfully"));
            }
            return BadRequest("Create failed");
        }
        [HttpPost("filter")]
        public async Task<IActionResult> FilterProducts([FromBody] ProductFilterRequest filter)
        {
            var result = await _productService.FilterProducts(
                filter.Brands,
                filter.Sizes,
                filter.Colors,
                filter.PriceRange,
                filter.Page,
                filter.PageSize
            );

            return Ok(result);
        }
    }
}
