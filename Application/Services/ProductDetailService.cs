
using Microsoft.AspNetCore.Http;
using ShoesShop.Application.DTOs;
using ShoesShop.Application.Interfaces.Repositories;
using ShoesShop.Application.Interfaces.Services;
using ShoesShop.Domain.Entities;

public class ProductDetailService : IProductDetailService
{
    private readonly IProductDetailRepository _repository;
    private readonly IProductRepository _productRepository;
    private readonly IFileService _fileService;

    public ProductDetailService(IProductDetailRepository repository, IProductRepository productRepository, IFileService fileService)
    {
        _repository = repository;
        _productRepository = productRepository;
        _fileService = fileService;
    }

    public async Task<List<ProductDetailDTO>> GetByProductIdAsync(int productId)
    {
        var details = await _repository.GetProductDetailDtosByProductIdAsync(productId);

        return details.Select(pd => new ProductDetailDTO
        {
            ProductDetailId = pd.ProductDetailId,
            ProductId = pd.ProductId,
            ColorName = pd.ColorName,
            SizeName = pd.SizeName,
            Price = pd.Price,
            StockQuantity = pd.StockQuantity,
            ImageUrl = pd.ImageUrl
        }).ToList();
    }


    public async Task<ProductDetail?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<(bool Success, string Message, ProductDetail? Result)> CreateProductDetailAsync(CreateProductDetailDTO model)
    {
        var product = await _productRepository.GetProductByIdAsync(model.ProductId);
        if (product == null)
            return (false, "Sản phẩm không tồn tại.", null);

        if (await _repository.ExistsAsync(model.ProductId, model.ColorID, model.SizeID))
            return (false, "Biến thể đã tồn tại.", null);

        if (model.Image?.Length > 1 * 2048 * 2048)
            return (false, "Dung lượng ảnh vượt quá 1MB.", null);

        string[] allowedExts = [".jpg", ".jpeg", ".png", ".PNG"];
        string imageName = await _fileService.SaveFileAsync(model.Image, allowedExts);

        var productDetail = new ProductDetail
        {
            ProductId = model.ProductId,
            ColorId = model.ColorID,
            SizeId = model.SizeID,
            StockQuantity = model.Quantity,
            Price = model.Price,
            ImageUrl = imageName
        };

        await _repository.AddAsync(productDetail);
        await _repository.SaveChangesAsync();

        return (true, "Thêm biến thể thành công!", productDetail);
    }
}
