using Microsoft.AspNetCore.Mvc;
using ShoesShop.Application.DTOs;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Application.Interfaces.Services
{
    public interface IColorService
    {
        Task<ResponseDTO<Color>> GetAllColorsAsync(int pageNumber,int pageSize);
        Task<Color> GetColorByNameAsync(string name); 
        Task<bool> CreateColorAsync(Color model);
        Task<bool> UpdateColorAsync(Color model);
        Task<bool> DeleteColorAsync(int id);
        Task<bool> UpdateStatusAsync(int id);
    }

}
