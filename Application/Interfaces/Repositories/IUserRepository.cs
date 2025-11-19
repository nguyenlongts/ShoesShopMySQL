using API_ShoesShop.Domain.Entities;
using ShoesShop.Application.DTOs;

namespace ShoesShop.Application.Interfaces.Repositories
{
    public interface IUserRepository 
    {
        Task<ResponseDTO<AdminUserInfoResponse>> GetAllAsync(int pageNumber, int pageSize);
        Task<UserInfoResponse> GetByIdAsync(Guid id);

        Task<bool> UpdateAsync(ApplicationUser entity);
        Task<bool> DeleteAsync(Guid id);

        Task<(bool success, string message)> RegisterAsync(ApplicationUser user, string password);
        Task<bool> UpdateStatusAsync(Guid id);
    }
}
