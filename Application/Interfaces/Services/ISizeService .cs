using Microsoft.AspNetCore.Mvc;
using ShoesShop.Application.DTOs;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Application.Interfaces.Services
{
    public interface ISizeService
    {
        Task<ResponseDTO<Size>> GetAllSizeAsync(int pageNumber,int pageSize);
        Task<Size> GetSizeByNameAsync(string name); 
        Task<bool> CreateSizeAsync(Size model);
        Task<bool> UpdateSizeAsync(Size model);
        Task<bool> DeleteSizeAsync(int id);
        Task<bool> UpdateStatusAsync(int id);
    }

}
