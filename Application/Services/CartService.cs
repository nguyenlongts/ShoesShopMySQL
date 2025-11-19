using Microsoft.Extensions.Caching.Memory;
using ShoesShop.Application.DTOs;
using ShoesShop.Application.Interfaces.Repositories;
using ShoesShop.Application.Interfaces.Services;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICacheService _cache;
        public CartService(ICartRepository cartRepository, ICacheService cache)
        {
            _cartRepository = cartRepository;
            _cache = cache;
        }
        public async Task<bool> AddItemAsync(AddToCartRequest request)
        {
            var result = await _cartRepository.AddToCartAsync(request.UserId.ToString(), request.ProductDetailId, request.Quantity);
            if (result)
            {
                await _cache.RemoveCacheAsync($"cart-items-{request.UserId}");
            }
            return result;
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            var result = await _cartRepository.ClearCartAsync(userId);

            if (result)
            {
                await _cache.RemoveCacheAsync($"cart-items-{userId}");
            }
            return result;
        }

        public Task<bool> CreateAsync(string userId)
        {
            return _cartRepository.CreateAsync(userId);
        }

        public async Task<IEnumerable<CartItemDTO>> GetAllCartItem(string userId)
        {
            string cacheKey = $"cart-items-{userId}";
            var cachedResponse = await _cache.GetCacheAsync<IEnumerable<CartItemDTO>>(cacheKey);
            if (cachedResponse != null)
            {
                return cachedResponse;
            }
            var cart = await _cartRepository.GetCartByUserId(userId);
            var cartItems = await _cartRepository.GetAllCartItem(cart.CartId.ToString());
            await _cache.SetCacheAsync(cacheKey, cartItems, TimeSpan.FromMinutes(5), TimeSpan.FromHours(1));
            return cartItems;
        }

        public async Task<Cart?> GetByUserIdAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserId(userId);
            return cart;
        }

        public async Task<decimal> GetTotalPriceAsync(string userId)
        {
            var cartItems = await GetAllCartItem(userId);
            return cartItems.Sum(item => item.Price * item.Quantity);
        }

        public async Task<bool> RemoveItemAsync(string userId, int productDetailId)
        {
            var result = await _cartRepository.RemoveFromCartAsync(userId, productDetailId);
            if (result)
            {
                await _cache.RemoveCacheAsync($"cart-items-{userId}");
            }
            return result;
        }

        public async Task<bool> UpdateQuantityAsync(string userId, int cartItemId, int newQuantity)
        {
            var result = await _cartRepository.UpdateQuantityAsync(userId.ToString(), cartItemId, newQuantity);
            if (result)
            {
                await _cache.RemoveCacheAsync($"cart-items-{userId}");
            }
            return result;
        }
    }
}
