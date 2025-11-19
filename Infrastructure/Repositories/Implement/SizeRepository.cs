
using API_ShoesShop.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using ShoesShop.Application.Interfaces.Repositories;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Infrastructure.Repositories.Implement
{
    public class SizeRepository : ISizeRepository
    {
        private readonly AppDBContext _context;
        public SizeRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task<Size> GetByNameAsync(string name)
        {
            
            return await _context.Sizes.FirstOrDefaultAsync(c => c.Name == name);
        }
        public async Task AddAsync(Size model)
        {
            await _context.Sizes.AddAsync(model);
            await _context.SaveChangesAsync();
       
        }
    }
}
