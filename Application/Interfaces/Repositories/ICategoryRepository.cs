using ShoesShop.Application.DTOs;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Application.Interfaces.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<bool> UpdateStatusAsync(int cateID);
        Task AddAsync(CreateCateDTO model);


        Task<Category> GetByNameAsync(string name);
    }
}
