using Microsoft.AspNetCore.Mvc;
using ShoesShop.Application.DTOs;
using ShoesShop.Application.Interfaces.Repositories;
using ShoesShop.Application.Interfaces.Services;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Application.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ICacheService _cacheService;

        public BrandService(IBrandRepository brandRepository, ICacheService cacheService)
        {
            _brandRepository = brandRepository;
            _cacheService = cacheService;
        }

        public async Task<bool> CreateBrandAsync(CreateBrandDTO brand)
        {
            
            var exists = await _brandRepository.GetByNameAsync(brand.Name);
            if (exists == null)
            {
                await _brandRepository.AddAsync(brand);
                return true;
            }
            return false;
        }

        public async Task<ResponseDTO<Brand>> GetAllAsync(int pageSize,int pageNum)
        {
            string cacheKey = $"brands-{pageNum}-{pageSize}";

            var cachedResponse = await _cacheService.GetCacheAsync<ResponseDTO<Brand>>(cacheKey);
            if (cachedResponse != null)
            {
                return cachedResponse;
            }
            var result = await _brandRepository.GetAllAsync(pageNum, pageSize);
            await _cacheService.SetCacheAsync(cacheKey, result, TimeSpan.FromMinutes(5),TimeSpan.FromHours(1));

            return result;
            
        }

        public async Task<bool> UpdateBrandAsync(Brand model)
        {
            await _brandRepository.UpdateAsync(model);
            return true;
        }

        Task<bool> IBrandService.UpdateStatusAsync(int brandID)
        {
            var result = _brandRepository.UpdateStatusAsync(brandID);
            return result;
        }
    }
}
