using Microsoft.AspNetCore.Mvc;
using ShoesShop.Application.DTOs;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<ProductResponseDTO> GetAllAdminAsync(int pageSize,int pageNum);
        Task<ProductResponseDTO> GetProductsCustomerAsync(int pageSize, int pageNum);
        Task<bool> CreateAsync(CreateProductDTO product);
        Task<Product> GetProductByNameAsync(string name);

        Task<GetProductDTO> GetProductByIdAsync(int id);
        Task<ProductResponseDTO> FilterProducts(List<int>? brandIds,List<int>? sizeIds,List<int>? colorIds,string? priceRange,int page = 1,int pageSize = 10);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
    }
}
