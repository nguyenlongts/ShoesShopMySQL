using ShoesShop.Domain.Entities;
using ShoesShop.Application.Interfaces.Repositories;
using ShoesShop.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ShoesShop.Application.DTOs;

namespace ShoesShop.Application.Services
{
    public class ColorService : IColorService
    {

        private readonly IGenericRepository<Color> _genericRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ICacheService _cacheService;
        public ColorService(IColorRepository colorRepository, IGenericRepository<Color> genericRepository, ICacheService cacheService)
        {
            _colorRepository = colorRepository;
            _genericRepository = genericRepository;
            _cacheService = cacheService;
        }
        public async Task<bool> CreateColorAsync(Color model)
        {
            model.IsActive = true;
            await _colorRepository.AddAsync(model);
            return true;
        }

        public async Task<bool> DeleteColorAsync(int id)
        {
 
            await _genericRepository.DeleteAsync(id);
            return true;
        }

        public async Task<ResponseDTO<Color>> GetAllColorsAsync(int pageNumber, int pageSize)
        {
            string cacheKey = $"colors-{pageNumber}-{pageSize}";
            var cachedData = await _cacheService.GetCacheAsync<ResponseDTO<Color>>(cacheKey);
            if (cachedData != null)
                return cachedData;
            var result = await _genericRepository.GetAllAsync(pageNumber, pageSize);
            await _cacheService.SetCacheAsync(cacheKey, result, TimeSpan.FromMinutes(5), TimeSpan.FromHours(1));
            return result;
        }

        public async Task<Color> GetColorByNameAsync(string name)
        {
            string cacheKey = $"color-name-{name}";
            var cachedData = await _cacheService.GetCacheAsync<Color>(cacheKey);
            if (cachedData != null)
                return cachedData;

            var color = await _colorRepository.GetByNameAsync(name);
            if (color != null)
            {
                await _cacheService.SetCacheAsync(cacheKey, color, TimeSpan.FromMinutes(10), TimeSpan.FromHours(2));
            }
            return color;
        }

        public async Task<bool> UpdateStatusAsync(int id)
        {
            var entity = await _genericRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return false;
            }

            entity.IsActive = !entity.IsActive;
            await _genericRepository.UpdateAsync(entity);


            return true;
        }
        

        public async Task<bool> UpdateColorAsync(Color model)
        {
            await _genericRepository.UpdateAsync(model);
            return true;
        }
    }
}
