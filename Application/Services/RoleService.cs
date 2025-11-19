using Microsoft.AspNetCore.Identity;
using ShoesShop.Application.Interfaces.Repositories;
using ShoesShop.Application.Interfaces.Services;

namespace ShoesShop.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<bool> CreateRoleAsync(string roleName)
        {
            return await _roleRepository.AddAsync(new IdentityRole(roleName));
           
        }

        public async Task<bool> DeleteRoleAsync(string name)
        {
            return await (_roleRepository.DeleteAsync(name));
        }

        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            return await _roleRepository.GetAllAsync();
        }
    }
}
