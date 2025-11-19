using ShoesShop.Application.DTOs;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {

        Task<ResponseDTO<Order>> GetAllOrdersAsync(int pageNum,int pageSize);


        Task<ResponseDTO<CustomerOrderResponse>> GetOrdersByUserIdAsync(string userId,int pageNum,int pageSize);


        Task<Order> GetOrderByIdAsync(Guid orderId);


        Task<Order> CreateOrderAsync(Order order);


        Task<bool> UpdateOrderStatusAsync(Guid orderId, Order.OrderStatus status);


        Task<bool> DeleteOrderAsync(Guid orderId);

        Task<IEnumerable<OrderItem>> GetOrderItemsAsync(Guid orderId);


        Task<bool> AddOrderItemAsync(OrderItem orderItem);


        Task<bool> OrderExistsAsync(Guid orderId);
    }

}
