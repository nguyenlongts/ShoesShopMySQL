using ShoesShop.Application.DTOs;
using ShoesShop.Domain.Entities;

public interface IProductDetailService
{
    Task<List<ProductDetailDTO>> GetByProductIdAsync(int productId);
    Task<ProductDetail?> GetByIdAsync(int id);
    Task<(bool Success, string Message, ProductDetail? Result)> CreateProductDetailAsync(CreateProductDetailDTO dto);
}

