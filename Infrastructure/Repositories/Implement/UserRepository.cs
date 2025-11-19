using API_ShoesShop.Domain.Entities;
using API_ShoesShop.Infrastructure.DBContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoesShop.Application.DTOs;
using ShoesShop.Application.Interfaces.Repositories;

namespace ShoesShop.Infrastructure.Repositories.Implement
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDBContext _context;
        public UserRepository(UserManager<ApplicationUser> userManager, AppDBContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return false; 
            }
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<ResponseDTO<AdminUserInfoResponse>> GetAllAsync(int pageNumber, int pageSize)
        {
            int totalUser = await _context.Users.CountAsync();
            var users = await _userManager.Users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var responseUser =  users.Select(u => new AdminUserInfoResponse 
            { 
                Email = u.Email,
                FullName = $"{u.FirstName} {u.LastName}",
                Phone = u.PhoneNumber,
            isActive=u.isActive,
            UserId=new Guid(u.Id)}).ToList();
            int totalPages = (int)Math.Ceiling((decimal)totalUser / pageSize);
            return new ResponseDTO<AdminUserInfoResponse> { Items = responseUser,
            TotalItems=totalUser,
            TotalPages = totalPages} ;
        }

        public async Task<UserInfoResponse> GetByIdAsync(Guid id)
        {
            var existUser = await _userManager.FindByIdAsync(id.ToString());
            if (existUser == null) {
                return null;
            }
            var addresses = await _context.Addresses
        .Where(a => a.UserId == id.ToString()).ToListAsync();
            var response = new UserInfoResponse
            {
                Email = existUser.Email,
                Phone = existUser.PhoneNumber,
                FullName = $"{existUser.FirstName} {existUser.LastName}",
                ShippingAddress = addresses.Select(a => a.FullAddress).ToList()
            };
            return response ;
        }

        public async Task<(bool success, string message)> RegisterAsync(ApplicationUser user, string password)
        {
            var existUser = await _userManager.FindByEmailAsync(user.Email);
            if (existUser != null)
                return (false, "Email đã tồn tại!");

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return (false, "Đăng ký thất bại!");

            return (true, "Đăng ký thành công!");
        }

        public async Task<bool> UpdateAsync(ApplicationUser entity)
        {
            var result = await _userManager.UpdateAsync(entity);
            return result.Succeeded;
        }

        async Task<bool> IUserRepository.UpdateStatusAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return false;
            }
            user.isActive = !user.isActive;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
