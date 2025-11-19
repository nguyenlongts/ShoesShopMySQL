using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesShop.Application.Interfaces.Services;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Controllers
{
    
    [Route("api/sizes")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly ISizeService _SizeService;
        public SizeController(ISizeService SizeService)
        {
            _SizeService = SizeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(int pageNumber=1,int pageSize=5)
        {
            var result = await _SizeService.GetAllSizeAsync(pageNumber, pageSize);
            return Ok(result);
        }
        //[authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Size model)
        {
            var result = await _SizeService.CreateSizeAsync(model);
            if (result == false) {
                return BadRequest("Add failed");
            }
            return Ok("Add Size successfully");
        }
        //[authorize]
        [HttpGet("search")]
        public async Task<IActionResult> GetSizeByNameAsync(string name)
        {
            var result = await _SizeService.GetSizeByNameAsync(name);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Not Existed!");
        }
        ////[authorize]
        [HttpPut]
        public async Task<IActionResult> Update(Size model)
        {
            var result =await _SizeService.UpdateSizeAsync(model);
            if (result == true)
            {
                return Ok("Update Size successfully");
            }
            return BadRequest("Update failed");
        }
        //[authorize]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _SizeService.UpdateStatusAsync(id);

            if (!result)
            {
                return NotFound("Size not found");
            }
            return Ok("Size status updated successfully");
        }
        //[authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _SizeService.DeleteSizeAsync(id);
            if(result == true)
            {
                return Ok("Delete Successfully");
            }
            return BadRequest("Delete Failed");
        }
    }
    
}
