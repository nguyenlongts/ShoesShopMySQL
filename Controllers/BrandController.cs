using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesShop.Application.DTOs;
using ShoesShop.Application.Interfaces.Services;
using ShoesShop.Application.Services;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Controllers
{
    
    [Route("api/brands")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll(int pageSize = 10,int pageNum = 1)
        {
            return Ok(await _brandService.GetAllAsync(pageSize,pageNum));
        }
        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> Create(CreateBrandDTO brand)
        {
            var result = await _brandService.CreateBrandAsync(brand);
            if (result)
            {
                return Ok(new { message = $"Tạo thương hiệu {brand.Name} thành công" });
            }
            return BadRequest(new { message = "Tạo thương hiệu thất bại" });
        }
        [Authorize]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            if (id == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });
            }

            var result = await _brandService.UpdateStatusAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Brand không tồn tại hoặc cập nhật thất bại" });
            }

            return Ok(new { message = "Cập nhật trạng thái thành công" });
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,[FromBody]Brand model)
        {
            model.BrandID = id;
            var result = await _brandService.UpdateBrandAsync(model);
            if (result)
            {
                return Ok(new { message = "Cập nhật thương hiệu thành công" });
            }
            return BadRequest(new { message = "Cập nhật thương hiệu thất bại" });
        }
    }
}
