using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesShop.Application.Interfaces.Services;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Controllers
{

    [Route("api/colors")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IColorService _colorService;
        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }
        [HttpGet()]
        public async Task<IActionResult> GetAllAsync(int pageNumber = 1, int pageSize = 5)
        {
            var result = await _colorService.GetAllColorsAsync(pageNumber, pageSize);
            return Ok(result);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Color model)
        {
            var result = await _colorService.CreateColorAsync(model);
            if (result == false) {
                return BadRequest("Tạo thất bại");
            }
            return Ok("Tạo màu thành công");
        }
        [Authorize]
        [HttpGet("filter/name/{name}")]
        public async Task<Color> GetColorByNameAsync(string name)
        {
            return await _colorService.GetColorByNameAsync(name);
        }
 
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Color model)
        {
            var result =await _colorService.UpdateColorAsync(model);
            if (result == true)
            {
                return Ok("Cập nhật màu thành công");
            }
            return BadRequest("Cập nhật màu thất bại");
        }
        [Authorize]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _colorService.UpdateStatusAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok("Cập nhật màu thành công");
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _colorService.DeleteColorAsync(id);
            if(result == true)
            {
                return Ok("Xoá màu thành công");
            }
            return BadRequest("Xoá thất bại");
        }
    }
    
}
