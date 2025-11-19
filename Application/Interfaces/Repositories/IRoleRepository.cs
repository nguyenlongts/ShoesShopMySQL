using Microsoft.AspNetCore.Identity;

namespace ShoesShop.Application.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<bool> AddAsync(IdentityRole role);
        Task<bool> DeleteAsync(string name);
        Task<IdentityRole> GetByIdAsync(string id);
        Task<IdentityRole> GetByNameAsync(string name);
        Task UpdateAsync(IdentityRole role);
        Task<List<IdentityRole>> GetAllAsync();
    }
}
