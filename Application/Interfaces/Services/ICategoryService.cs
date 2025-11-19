using System.Threading.Tasks;
using ShoesShop.Application.DTOs;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<ResponseDTO<Category>> GetAllAsync(int pageSize,int pageNum);

        Task<bool> CreateCategoryAsync(CreateCateDTO model);
        Task<Category> GetCategoryByNameAsync(string name);
        Task<bool> UpdateStatusAsync(int CategoryID);

        Task<bool> UpdateCategoryAsync(Category model);
    }
}
