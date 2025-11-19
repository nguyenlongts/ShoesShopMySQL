using API_ShoesShop.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using ShoesShop.Application.DTOs;
using ShoesShop.Application.Interfaces.Repositories;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Infrastructure.Repositories.Implement
{
    public class BrandRepository : IBrandRepository
    {
        private readonly AppDBContext _context;

        public BrandRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task<Brand> GetByNameAsync(string name)
        {
            return await _context.Brands.FirstOrDefaultAsync(b => b.Name == name);
        }
        public async Task AddAsync(CreateBrandDTO brand)
        {
            var newBrand = new Brand { IsActive = brand.IsActive,Name=brand.Name };
            _context.Brands.Add(newBrand);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Brand>> GetAllAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task UpdateAsync(Brand brand)
        {
            var existingBrand = await _context.Brands.FindAsync(brand.BrandID);
            if (existingBrand == null) return;
            existingBrand.Name = brand.Name;
            existingBrand.IsActive = brand.IsActive;

            await _context.SaveChangesAsync();
        }
        public async Task<bool> UpdateStatusAsync(int brandId)
        {
            var brand = await _context.Brands.FindAsync(brandId);
            if (brand == null) return false;

            brand.IsActive = !brand.IsActive;
            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task DeleteAsync(int id)
        {
            var existingBrand = await _context.Brands.FindAsync(id);
            _context.Brands.Remove(existingBrand);
            await _context.SaveChangesAsync();
        }

        public async Task<ResponseDTO<Brand>> GetAllAsync(int pageNumber, int pageSize)
        {
            int totalItem = await _context.Brands.CountAsync();

            var items = await _context.Brands.Skip((pageNumber-1)*pageSize).Take(pageSize).ToListAsync();
            int totalPages = (int)Math.Ceiling((decimal)totalItem / pageSize);
            return new ResponseDTO<Brand> { Items = items, TotalPages = totalPages,TotalItems=totalItem };
        }

        async Task<Brand> IGenericRepository<Brand>.GetByIdAsync(int id)
        {
            return await _context.Brands.FindAsync(id);
        }

        
        
    }
}
