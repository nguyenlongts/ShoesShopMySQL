using ShoesShop.Application.DTOs;

namespace ShoesShop.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T  :class
    {
        Task<ResponseDTO<T>> GetAllAsync(int pageNumber,int pageSize);
        Task<T> GetByIdAsync(int id);
       
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);

    }
}
