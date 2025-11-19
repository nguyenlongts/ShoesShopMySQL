using Microsoft.AspNetCore.Identity;
using ShoesShop.Application.Interfaces.Repositories;

namespace ShoesShop.Infrastructure.Repositories.Implement
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleRepository(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<bool> AddAsync(IdentityRole role)
        {

            var existRole = await _roleManager.FindByNameAsync(role.Name);
            if (existRole == null)
            {
                var result = await _roleManager.CreateAsync(role);
                return result.Succeeded;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                return result.Succeeded;
            }
            return false;
        }

        public async Task<IdentityRole> GetByIdAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }

        public async Task<IdentityRole> GetByNameAsync(string name)
        {
            return await _roleManager.FindByNameAsync(name);
        }

        public async Task UpdateAsync(IdentityRole role)
        {
            await _roleManager.UpdateAsync(role);
        }

        public async Task<List<IdentityRole>> GetAllAsync()
        {
            return _roleManager.Roles.ToList();
        }
    }
}
