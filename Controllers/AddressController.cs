using API_ShoesShop.Infrastructure.DBContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoesShop.Application.DTOs;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly AppDBContext _context;

        public AddressController(AppDBContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAddress([FromBody] AddressDTO addressDto)
        {
            if (addressDto == null)
            {
                return BadRequest("Invalid address data.");
            }

            var user = await _context.Users.FindAsync(addressDto.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (addressDto.IsDefault)
            {
                var existingAddresses = await _context.Addresses
                    .Where(a => a.UserId == addressDto.UserId.ToString())
                    .ToListAsync();

                existingAddresses.ForEach(a => a.IsDefault = false);
            }

            var newAddress = new Address
            {
                UserId = addressDto.UserId.ToString(),
                FullAddress = addressDto.FullAddress,
                IsDefault = addressDto.IsDefault
            };

            _context.Addresses.Add(newAddress);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Address saved successfully!", addressId = newAddress.AddressId });
        }
    }

}
