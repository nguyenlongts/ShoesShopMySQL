using Microsoft.AspNetCore.Identity;

namespace ShoesShop.Application.Interfaces.Services
{
    public interface IRoleService
    {
        Task<bool> CreateRoleAsync(string roleName);
        Task<bool> DeleteRoleAsync(string name);
        Task<List<IdentityRole>> GetAllRolesAsync();
    }
}
