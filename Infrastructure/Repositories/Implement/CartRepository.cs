using System.Net.WebSockets;
using API_ShoesShop.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using ShoesShop.Application.DTOs;
using ShoesShop.Application.Interfaces.Repositories;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Infrastructure.Repositories.Implement
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDBContext _context;
        public CartRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<bool> AddToCartAsync(string userId, int ProductDetailId, int quantity)
        {
            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId.ToString());
            var variant =await _context.ProductDetails.FindAsync(ProductDetailId);
            var existCartItem =  cart.CartItems.FirstOrDefault(ci=>ci.ProductDetailId==ProductDetailId);
            if (existCartItem != null)
            {
                existCartItem.Quantity += quantity;
                if (existCartItem.Quantity > variant.StockQuantity)
                {
                    return false;
                }
            }
            else
            {
                var newCartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductDetailId = ProductDetailId,
                    Quantity = quantity,
                };
                _context.CartItems.Add(newCartItem);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId.ToString());
            foreach (var cartItem in cart.CartItems){
                _context.CartItems.Remove(cartItem);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CreateAsync(string userId)
        {
            var existCart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (existCart == null)
            {
                var newCart = new Cart { UserId = userId};
                _context.Carts.Add(newCart);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<CartItemDTO>> GetAllCartItem(string cartId)
        {
            var cartItems =await _context.CartItems.Include(ci=>ci.ProductDetail)
                .ThenInclude(pd=>pd.Size)
                .Include(ci=>ci.ProductDetail)
                .ThenInclude(pd=>pd.Color)
                .Where(ci => ci.CartId.ToString() == cartId)
                .ToListAsync();
            return cartItems.Select(ci => new CartItemDTO
            {
                ProductId = ci.ProductDetail.ProductId,
                CartItemId = ci.CartItemId,
                MaxQuantity = ci.ProductDetail.StockQuantity,
                SizeName = ci.ProductDetail?.Size?.Name,
                ColorName = ci.ProductDetail?.Color?.Name,
                Price = ci.ProductDetail?.Price ?? 0,
                Quantity = ci.Quantity,
                ImageUrl = ci.ProductDetail.ImageUrl,
                ProductDetailId=ci.ProductDetail.ProductDetailId
            }).ToList();
        }

        public async Task<Cart> GetCartByUserId(string userId)
        {
            var cart = await _context.Carts.Include(c=>c.CartItems).ThenInclude(ci => ci.ProductDetail).FirstOrDefaultAsync(c=>c.UserId==userId);
            return cart;
        }

        public async Task<bool> RemoveFromCartAsync(string userId, int ProductDetailId)
        {
            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId.ToString());
            foreach (var cartItem in cart.CartItems)
            {
                if (cartItem.ProductDetailId == ProductDetailId)
                {
                    _context.CartItems.Remove(cartItem);
                }
            }

            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> UpdateQuantityAsync(string userId, int cartItemId, int newQuantity)
        {
            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId.ToString());
            foreach (var cartItem in cart.CartItems)
            {
                if(cartItem.CartItemId == cartItemId)
                {
                    cartItem.Quantity = newQuantity;
                    await _context.SaveChangesAsync();
                    return true;
                    
                }
            }
            return false;
        }
    }
}
