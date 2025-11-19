using Microsoft.EntityFrameworkCore;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Application.Interfaces.Repositories
{
    public interface ISizeRepository
    {
        Task<Size> GetByNameAsync(string name);

        Task AddAsync(Size model);
    }
}
