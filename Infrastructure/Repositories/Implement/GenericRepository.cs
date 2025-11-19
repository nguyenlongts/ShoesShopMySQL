using API_ShoesShop.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using ShoesShop.Application.DTOs;
using ShoesShop.Application.Interfaces.Repositories;

namespace ShoesShop.Infrastructure.Repositories.Implement
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDBContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDBContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<ResponseDTO<T>> GetAllAsync(int pageNumber, int pageSize)
        {

            int totalItem = _dbSet.Count();
            var items =await _dbSet
                .Skip((pageNumber - 1) * pageSize) 
                .Take(pageSize)                    
                .ToListAsync();
            int totalPages = (int)Math.Ceiling((decimal)totalItem / pageSize);

            return new ResponseDTO<T>
            {
                Items = items,
                TotalItems = totalItem,
                TotalPages = totalPages
            };
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }


        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existingEntity = await _dbSet.FindAsync(id);
            _dbSet.Remove(existingEntity);
            await _context.SaveChangesAsync();
           
        }

    }

}
