using API_ShoesShop.Application.DTOs;
using System.Text;
using API_ShoesShop.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using ShoesShop.Application.DTOs;
using ShoesShop.Application.Interfaces.Repositories;
using ShoesShop.Application.Interfaces.Services;
using ShoesShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using API_ShoesShop.Infrastructure.DBContext;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace ShoesShop.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICartService _cartService;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly AppDBContext _context;
        private readonly ICacheService _cacheService;
        public UserService(IUserRepository userRepository, ICartService cartService, IEmailService emailService, UserManager<ApplicationUser> userManager, IConfiguration config, AppDBContext context, ICacheService cacheService)
        {
            _userRepository = userRepository;
            _cartService = cartService;
            _emailService = emailService;
            _userManager = userManager;
            _config = config;
            _context = context;
            _cacheService = cacheService;
        }

        public async Task<(bool success, string message)> RegisterAsync(RegisterDTO model)
        {
            var existUser = await _userManager.FindByEmailAsync(model.Email);
            if (existUser != null)
                return (false, "Email đã tồn tại!");

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Address = model.Address,
                DoB = model.DoB,
                PhoneNumber = model.Phone,
                Gender = model.Gender,
                LastName = model.LastName,
                FirstName = model.FirstName
            };

            var result = await _userRepository.RegisterAsync(user, model.Password);
            if (!result.success)
                return (false, "Đăng ký thất bại!");
            var address = new Address
            {
                FullAddress = model.Address,
                UserId = user.Id,
                IsDefault = true
            }; 
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            var createCart = await _cartService.CreateAsync(user.Id);
            if (!createCart)
                return (false, "Không thể tạo giỏ hàng!");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var clientUrl = _config["AppSettings:ClientURL"];
            var confirmLink = $"{clientUrl}/api/Auth/confirm-email?userId={user.Id}&token={encodedToken}";

            string subject = "Xác nhận tài khoản";
            string body = $"<p>Nhấp vào link sau để xác nhận tài khoản: <a href='{confirmLink}'>Xác nhận Email</a></p>";

            await _emailService.SendMailAsync(user.Email, subject, body);

            return (true, "Đăng ký thành công! Vui lòng kiểm tra email để xác nhận.");
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            return _userRepository.DeleteAsync(id);
        }

        

        public async Task<ResponseDTO<AdminUserInfoResponse>> GetAllAsync(int pageSize, int pageNum)
        {
            string cacheKey = $"users_page_{pageNum}_size_{pageSize}";
            var cachedResponse = await _cacheService.GetCacheAsync<ResponseDTO<AdminUserInfoResponse>>(cacheKey);
            if (cachedResponse != null)
            {
                return cachedResponse;
            }
            var result = await _userRepository.GetAllAsync(pageNum, pageSize);
            await _cacheService.SetCacheAsync(cacheKey, result, TimeSpan.FromMinutes(5), TimeSpan.FromHours(1));
            return result;
        }

        public async Task<UserInfoResponse> GetByIdAsync(Guid id)
        {
            string cachedKey = $"user_{id}";
            var cachedUser = await _cacheService.GetCacheAsync<UserInfoResponse>(cachedKey);
            if (cachedUser != null)
            {
                return cachedUser;
            }
            var user = await _userRepository.GetByIdAsync(id);
            await _cacheService.SetCacheAsync(cachedKey, user, TimeSpan.FromMinutes(5), TimeSpan.FromHours(1));
            return user;
        }

        public async Task<bool> UpdateAsync(ApplicationUser user)
        {
            var result = await _userRepository.UpdateAsync(user);

            if (result)
            {
                await _cacheService.RemoveCacheAsync($"user_{user.Id}");
                for (int page = 1; page <= 3; page++)
                {
                    await _cacheService.RemoveCacheAsync($"users_page_{page}_size_10");
                }
            }
            return result;
        }


        public async Task<bool> UpdateStatusAsync(Guid userID)
        {
            var result = await _userRepository.UpdateStatusAsync(userID);

            if (result)
            {
                await _cacheService.RemoveCacheAsync($"user_{userID}");
                for (int page = 1; page <= 3; page++)
                {
                    await _cacheService.RemoveCacheAsync($"users_page_{page}_size_10");
                }
            }

            return result;
        }

    }
}
