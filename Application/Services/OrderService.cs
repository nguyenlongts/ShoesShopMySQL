using ShoesShop.Application.DTOs;
using ShoesShop.Application.Interfaces.Repositories;
using ShoesShop.Application.Interfaces.Services;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        private readonly IProductDetailRepository _productDetailRepository;

        private readonly ICacheService _cacheService;
        public OrderService(IOrderRepository orderRepository, IProductDetailRepository productDetailRepository, ICacheService cacheService)
        {
            _orderRepository = orderRepository;
            _productDetailRepository = productDetailRepository;
            _cacheService = cacheService;
        }
        public async Task<(bool IsSuccess, string Message, Guid? OrderId)> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            if (createOrderDto == null || createOrderDto.OrderItems == null || !createOrderDto.OrderItems.Any())
                return (false, "Dữ liệu đơn hàng không hợp lệ.", null);

            foreach (var item in createOrderDto.OrderItems)
            {
                var product = await _productDetailRepository.GetByIdAsync(item.ProductDetailId);
                if (product == null)
                    return (false, $"Không tìm thấy sản phẩm với ID {item.ProductDetailId}.", null);

                if (product.StockQuantity < item.Quantity)
                    return (false, $"Sản phẩm {product.ProductDetailId} không đủ tồn kho.", null);

                product.StockQuantity -= item.Quantity;
                await _productDetailRepository.UpdateAsync(product);

            }

            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                UserId = createOrderDto.UserId,
                ShippingAddress = createOrderDto.ShippingAddress,
                OrderItems = createOrderDto.OrderItems.Select(item => new OrderItem
                {
                    ProductDetailId = item.ProductDetailId,
                    Quantity = item.Quantity,
                    UnitPrice = item.PriceAtOrder,
                }).ToList(),
                CreateAt = DateTime.UtcNow
            };

            await _orderRepository.CreateOrderAsync(order);
            return (true, "Tạo đơn hàng thành công", (order.OrderId));
        }
        

        public Task<bool> DeleteOrderAsync(Guid orderId)
        {
            return _orderRepository.DeleteOrderAsync(orderId);
        }

        public Task<ResponseDTO<Order>> GetAllOrdersAsync(int pageNum, int pageSize)
        {
            return _orderRepository.GetAllOrdersAsync(pageNum, pageSize);
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            var cacheKey = $"order_{orderId}";
            var cached = await _cacheService.GetCacheAsync<Order>(cacheKey);
            if (cached != null)
                return cached;

            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order != null)
            {
                await _cacheService.SetCacheAsync(cacheKey, order, TimeSpan.FromMinutes(10), TimeSpan.FromHours(1));
            }
            return order;
        }

        public Task<ResponseDTO<CustomerOrderResponse>> GetOrdersByUserAsync(string userId, int pageNum, int pageSize)
        {
            return _orderRepository.GetOrdersByUserIdAsync(userId,pageNum,pageSize);
        }

        public Task<bool> UpdateOrderStatusAsync(Guid orderId, Order.OrderStatus newStatus)
        {
            return _orderRepository.UpdateOrderStatusAsync(orderId, newStatus);
        }

        
    }
}
