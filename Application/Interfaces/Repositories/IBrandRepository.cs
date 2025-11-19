using ShoesShop.Application.DTOs;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Application.Interfaces.Repositories
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Task<bool> UpdateStatusAsync(int brandId);
        Task AddAsync(CreateBrandDTO model);

        Task<Brand> GetByNameAsync(string name);
    }
}
