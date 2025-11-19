using ShoesShop.Application.DTOs;
using ShoesShop.Application.Interfaces.Repositories;
using ShoesShop.Application.Interfaces.Services;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cacheService;
        public ProductService(IProductRepository productRepository, ICacheService cacheService)
        {
            _productRepository = productRepository;
            _cacheService = cacheService;
        }

        async Task<bool> IProductService.CreateAsync(CreateProductDTO product)
        {
            return await _productRepository.CreateAsync(product);
        }

        async Task<bool> IProductService.DeleteAsync(int id)
        {
            var result = await _productRepository.DeleteAsync(id);
            return result;
        }

        public async Task<ProductResponseDTO> GetAllAdminAsync(int pageSize, int pageNum)
        {
            string cacheKey = $"products_admin_{pageNum}_{pageSize}";
            var cached = await _cacheService.GetCacheAsync<ProductResponseDTO>(cacheKey);
            if (cached != null) return cached;
            var result = await _productRepository.GetProductsAdmin(pageSize, pageNum);
            await _cacheService.SetCacheAsync(cacheKey, result, TimeSpan.FromMinutes(10), TimeSpan.FromHours(1));
            return result;
        }

        async Task<GetProductDTO> IProductService.GetProductByIdAsync(int id)
        {
            string cacheKey = $"product_{id}";
            var cached = await _cacheService.GetCacheAsync<GetProductDTO>(cacheKey);
            if (cached != null) return cached;
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product != null)
            {
                await _cacheService.SetCacheAsync(cacheKey, product, TimeSpan.FromMinutes(10), TimeSpan.FromHours(1));
            }
            return product;
        }

        async Task<Product> IProductService.GetProductByNameAsync(string name)
        {
            string cacheKey = $"product_name_{name}";
            var cached = await _cacheService.GetCacheAsync<Product>(cacheKey);
            if (cached != null) return cached;
            var product = await _productRepository.GetProductByNameAsync(name);
            if (product != null)
            {
                await _cacheService.SetCacheAsync(cacheKey, product, TimeSpan.FromMinutes(10), TimeSpan.FromHours(1));
            }
            return product;
        }

        public async Task<ProductResponseDTO> GetProductsCustomerAsync(int pageSize, int pageNum)
        {
            string cacheKey = $"products_customer_{pageNum}_{pageSize}";
            var cached = await _cacheService.GetCacheAsync<ProductResponseDTO>(cacheKey);
            if (cached != null) return cached;

            var result = await _productRepository.GetProductsCustomerAsync(pageSize, pageNum);
            await _cacheService.SetCacheAsync(cacheKey, result, TimeSpan.FromMinutes(10), TimeSpan.FromHours(1));
            return result;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            return await _productRepository.UpdateAsync(product);
        }

        public async Task<ProductResponseDTO> FilterProducts(List<int>? brandIds, List<int>? sizeIds, List<int>? colorIds, string? priceRange, int page = 1, int pageSize = 10)
        {
            var result = await _productRepository.GetFilteredProducts(brandIds, sizeIds, colorIds, priceRange, page, pageSize);

            return (result);
        }
    }
}
