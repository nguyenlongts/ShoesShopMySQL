using Microsoft.EntityFrameworkCore;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Application.Interfaces.Repositories
{
    public interface IColorRepository
    {
        Task<Color> GetByNameAsync(string name);

        Task AddAsync(Color model);
    }
}
