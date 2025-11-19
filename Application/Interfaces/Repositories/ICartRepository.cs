using ShoesShop.Application.DTOs;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task<bool> CreateAsync(string userId);

        Task<Cart> GetCartByUserId(string userId);
        Task<bool> AddToCartAsync(string userId, int ProductDetailId, int quantity);
        Task<bool> RemoveFromCartAsync(string userId, int ProductDetailId);

        Task<bool> UpdateQuantityAsync(string userId, int cartItemId, int newQuantity);

        Task<bool> ClearCartAsync(string userId);

        Task<IEnumerable<CartItemDTO>> GetAllCartItem(string cartId);

    }
}
