
using API_ShoesShop.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using ShoesShop.Application.Interfaces.Repositories;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Infrastructure.Repositories.Implement
{
    public class ColorRepository : IColorRepository
    {
        private readonly AppDBContext _context;
        public ColorRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task<Color> GetByNameAsync(string name)
        {
            return await _context.Colors.FirstOrDefaultAsync(c => c.Name == name);
        }
        public async Task AddAsync(Color color)
        {
            await _context.Colors.AddAsync(color);
            await _context.SaveChangesAsync();
       
        }
    }
}
