using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoesShop.Application.Interfaces.Services;

namespace ShoesShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var result = await _roleService.CreateRoleAsync(roleName);
            if (result == true)
            {
                return Ok("Create role "+roleName+" successfully");
            }
            return BadRequest("Create failed");
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var result = await _roleService.DeleteRoleAsync(roleName);
            if (result == true)
            {
                return Ok("Delete role " + roleName + " successfully");
            }
            return BadRequest("Delete failed");
        }
    }
}
