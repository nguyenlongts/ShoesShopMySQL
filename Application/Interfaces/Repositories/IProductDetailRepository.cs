
using ShoesShop.Application.DTOs;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Application.Interfaces.Repositories
{
    public interface IProductDetailRepository
    {
        Task<List<ProductDetailDTO>> GetProductDetailDtosByProductIdAsync(int productId);
        Task<List<ProductDetail>> GetByIdsAsync(List<int> ids);
        Task<ProductDetail?> GetByIdAsync(int id);
        Task AddAsync(ProductDetail productDetail);
        Task<bool> ExistsAsync(int productId, int colorId, int sizeId);
        Task SaveChangesAsync();
        Task UpdateAsync(ProductDetail productDetail);
    }
}
