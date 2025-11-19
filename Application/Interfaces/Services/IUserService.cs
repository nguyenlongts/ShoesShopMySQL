using API_ShoesShop.Application.DTOs;
using API_ShoesShop.Domain.Entities;
using ShoesShop.Application.DTOs;

namespace ShoesShop.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<ResponseDTO<AdminUserInfoResponse>> GetAllAsync(int pageSize,int pageNum);

        Task<UserInfoResponse> GetByIdAsync(Guid id);

        Task<bool> UpdateAsync(ApplicationUser user);

        Task<bool> DeleteAsync(Guid id);
        Task<(bool success, string message)> RegisterAsync(RegisterDTO model);
        Task<bool> UpdateStatusAsync(Guid userID);
    }
}
