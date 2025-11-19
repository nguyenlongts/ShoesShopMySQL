using ShoesShop.Application.DTOs;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Application.Interfaces.Services
{
    public interface ICartService
    {
        Task<bool> CreateAsync(string userId);
        Task<Cart?> GetByUserIdAsync(string userId);
        Task<bool> AddItemAsync(AddToCartRequest request);
        Task<bool> UpdateQuantityAsync(string userId, int productDetailId, int newQuantity);
        Task<bool> RemoveItemAsync(string userId, int productDetailId);
        Task<bool> ClearCartAsync(string userId);
        Task<decimal> GetTotalPriceAsync(string userId);

        Task<IEnumerable<CartItemDTO>> GetAllCartItem(string userId);
    }
}
