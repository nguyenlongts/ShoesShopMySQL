
using Microsoft.EntityFrameworkCore;
using ShoesShop.Application.Interfaces.Repositories;
using ShoesShop.Domain.Entities;
using API_ShoesShop.Infrastructure.DBContext;
using ShoesShop.Application.DTOs;
public class ProductDetailRepository : IProductDetailRepository
{
    private readonly AppDBContext _context;

    public ProductDetailRepository(AppDBContext context)
    {
        _context = context;
    }

    public async Task<List<ProductDetailDTO>> GetProductDetailDtosByProductIdAsync(int productId)
    {
        return await _context.ProductDetails
            .Include(pd => pd.Color)
            .Include(pd => pd.Size)
            .Where(pd => pd.ProductId == productId)
            .Select(pd => new ProductDetailDTO
            {
                ProductDetailId = pd.ProductDetailId,
                ProductId = pd.ProductId,
                ColorName = pd.Color.Name ,
                SizeName = pd.Size.Name,
                Price = pd.Price,
                StockQuantity = pd.StockQuantity,
                ImageUrl = pd.ImageUrl
            })
            .ToListAsync();
    }

    public async Task<List<ProductDetail>> GetByIdsAsync(List<int> ids)
    {
        return await _context.ProductDetails
            .Include(p => p.Product)
            .Include(p => p.Color)
            .Include(p => p.Size)
            .Where(p => ids.Contains(p.ProductDetailId))
            .ToListAsync();
    }
    public async Task<ProductDetail?> GetByIdAsync(int id)
    {
        return await _context.ProductDetails.FindAsync(id);
    }

    public async Task AddAsync(ProductDetail productDetail)
    {
        await _context.ProductDetails.AddAsync(productDetail);
    }

    public async Task<bool> ExistsAsync(int productId, int colorId, int sizeId)
    {
        return await _context.ProductDetails
            .AnyAsync(pd => pd.ProductId == productId && pd.ColorId == colorId && pd.SizeId == sizeId);
    }
    public async Task UpdateAsync(ProductDetail productDetail)
    {
        _context.ProductDetails.Update(productDetail);
        await _context.SaveChangesAsync();
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
