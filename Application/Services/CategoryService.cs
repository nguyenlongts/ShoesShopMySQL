using Microsoft.AspNetCore.Mvc;
using ShoesShop.Application.DTOs;
using ShoesShop.Application.Interfaces.Repositories;
using ShoesShop.Application.Interfaces.Services;
using ShoesShop.Domain.Entities;
using ShoesShop.Infrastructure.Repositories.Implement;

namespace ShoesShop.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _CategoryRepository;
        private readonly ICacheService _cacheService;

        public CategoryService(ICategoryRepository CategoryRepository, ICacheService cacheService)
        {
            _CategoryRepository = CategoryRepository;
            _cacheService = cacheService;
        }

        public async Task<bool> CreateCategoryAsync(CreateCateDTO model)
        {
            await _CategoryRepository.AddAsync(model);
            var exists = await _CategoryRepository.GetByNameAsync(model.Name);
            if (exists != null)
            {
                return true;
            }
            return false;
        }
        public async Task<Category> GetCategoryByNameAsync(string name)
        {
            var cate = await _CategoryRepository.GetByNameAsync(name);
            return cate;
        }

        public async Task<ResponseDTO<Category>> GetAllAsync(int pageSize,int pageNum)
        {
            string cacheKey = $"categories-{pageNum}-{pageSize}";
            var cachedData = await _cacheService.GetCacheAsync<ResponseDTO<Category>>(cacheKey);
            if (cachedData != null)
                return cachedData;

            var result = await _CategoryRepository.GetAllAsync(pageNum, pageSize);
            await _cacheService.SetCacheAsync(cacheKey, result, TimeSpan.FromMinutes(5), TimeSpan.FromHours(1));

            return result;
         
        }

        Task<bool> ICategoryService.UpdateStatusAsync(int CategoryID)
        {
            var result = _CategoryRepository.UpdateStatusAsync(CategoryID);
            return result;
        }

        public async Task<bool> UpdateCategoryAsync(Category model)
        {
            await _CategoryRepository.UpdateAsync(model);

            return true;
        }
    }
}
