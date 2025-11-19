using ShoesShop.Domain.Entities;
using ShoesShop.Application.Interfaces.Repositories;
using ShoesShop.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ShoesShop.Application.DTOs;

namespace ShoesShop.Application.Services
{
    public class SizeService : ISizeService
    {

        private readonly IGenericRepository<Size> _genericRepository;
        private readonly ISizeRepository _SizeRepository;
        private readonly ICacheService _cacheService;
        public SizeService(ISizeRepository SizeRepository, IGenericRepository<Size> genericRepository, ICacheService cacheService)
        {
            _SizeRepository = SizeRepository;
            _genericRepository = genericRepository;
            _cacheService = cacheService;
        }
        public async Task<bool> CreateSizeAsync(Size model)
        {
            model.IsActive = true;
            await _SizeRepository.AddAsync(model);
            return true;
        }

        public async Task<bool> DeleteSizeAsync(int id)
        {
 
            await _genericRepository.DeleteAsync(id);
            return true;
        }

        public async Task<ResponseDTO<Size>> GetAllSizeAsync(int pageNumber, int pageSize)
        {
            string cacheKey = $"sizes-{pageNumber}-{pageSize}";
            var cachedData = await _cacheService.GetCacheAsync<ResponseDTO<Size>>(cacheKey);
            if (cachedData != null)
                return cachedData;
            var result = await _genericRepository.GetAllAsync(pageNumber, pageSize);
            await _cacheService.SetCacheAsync(cacheKey, result, TimeSpan.FromMinutes(5), TimeSpan.FromHours(1));
            return result;
        }

        public async Task<Size> GetSizeByNameAsync(string name)
        {
            string cacheKey = $"size_name_{name}";
            var cachedData = await _cacheService.GetCacheAsync<Size>(cacheKey);
            if (cachedData != null)
                return cachedData;
            var Size = await _SizeRepository.GetByNameAsync(name);
            if (Size != null)
            {
                await _cacheService.SetCacheAsync(cacheKey, Size, TimeSpan.FromMinutes(10), TimeSpan.FromHours(2));
            }
            return Size;
        }

        public async Task<bool> UpdateStatusAsync(int id)
        {
            var entity = await _genericRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return false;
            }

            // Cập nhật trạng thái
            entity.IsActive = !entity.IsActive;
            await _genericRepository.UpdateAsync(entity);

            // Trả về true nếu update thành công
            return true;
        }
        

        public async Task<bool> UpdateSizeAsync(Size model)
        {
            await _genericRepository.UpdateAsync(model);
            return true;
        }
    }
}
