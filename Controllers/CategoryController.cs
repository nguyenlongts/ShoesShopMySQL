using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesShop.Application.DTOs;
using ShoesShop.Application.Interfaces.Services;
using ShoesShop.Application.Services;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _CategoryService;
        public CategoryController(ICategoryService CategoryService)
        {
            _CategoryService = CategoryService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll(int pageSize = 10,int pageNum = 1)
        {
            return Ok(await _CategoryService.GetAllAsync(pageSize,pageNum));
        }
        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> Create(CreateCateDTO Category)
        {
            var result = await _CategoryService.CreateCategoryAsync(Category);
            if (result)
            {
                return Ok("Tạo Category " + Category.Name +" thành công" );
            }
            return BadRequest("Tạo thất bại");

        }

        [Authorize]
        [HttpGet("filter/name/{name}")]
        public async Task<Category> GetCategoryByNameAsync(string name)
        {
            return await _CategoryService.GetCategoryByNameAsync(name);
        }

        [Authorize]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            if (id == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });
            }

            var result = await _CategoryService.UpdateStatusAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Category không tồn tại hoặc cập nhật thất bại" });
            }

            return Ok(new { message = "Cập nhật trạng thái thành công" });
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Category model)
        {
            var result = await _CategoryService.UpdateCategoryAsync(model);
            if (result == true)
            {
                return Ok("Cập nhật thành công");
            }
            return BadRequest("Cập nhật thất bại");
        }
    }
}
