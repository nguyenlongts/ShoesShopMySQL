using ShoesShop.Application.DTOs;
using ShoesShop.Domain.Entities;
using static ShoesShop.Domain.Entities.Order;

namespace ShoesShop.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<(bool IsSuccess, string Message, Guid? OrderId)> CreateOrderAsync(CreateOrderDto createOrderDto);

 
        Task<ResponseDTO<Order>> GetAllOrdersAsync(int pageNum, int pageSize);

        Task<Order?> GetOrderByIdAsync(Guid orderId);

        Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus);


        Task<bool> DeleteOrderAsync(Guid orderId);


        Task<ResponseDTO<CustomerOrderResponse>> GetOrdersByUserAsync(string userId, int pageNum, int pageSize);
    }
}
