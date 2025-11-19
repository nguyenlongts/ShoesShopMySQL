using API_ShoesShop.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesShop.Application.Interfaces.Services;
using ShoesShop.Application.Services;

namespace ShoesShop.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageSize = 5,int pageNum = 1) {
            return Ok(await _userService.GetAllAsync(pageSize, pageNum));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _userService.GetByIdAsync(id));
        }
        //[Authorize]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id)
        {
            if (id == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });
            }

            var result = await _userService.UpdateStatusAsync(id);
            if (!result)
            {
                return NotFound(new { message = "User không tồn tại hoặc cập nhật thất bại" });
            }

            return Ok(new { message = "Cập nhật trạng thái thành công" });
        }
    }
}
