using API_ShoesShop.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using ShoesShop.Application.DTOs;
using ShoesShop.Application.Interfaces.Repositories;
using ShoesShop.Domain.Entities;

namespace ShoesShop.Infrastructure.Repositories.Implement
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDBContext _context;
        public OrderRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task<bool> AddOrderItemAsync(OrderItem orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            var result = await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteOrderAsync(Guid orderId)
        {
            var existOrder = await _context.Orders.FindAsync(orderId);
            if (existOrder == null) return false;
            _context.Orders.Remove(existOrder);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<ResponseDTO<Order>> GetAllOrdersAsync(int pageNum=1,int pageSize=5)
        {
            int totalOrder = await _context.Orders.CountAsync();
            var orders = await _context.Orders.OrderByDescending(o=>o.CreateAt)
                .Include(o=>o.OrderItems)
                .Skip((pageNum - 1) * pageSize).Take(pageSize)
                .ToListAsync();
            int totalPages = (int)Math.Ceiling((decimal)totalOrder / pageSize);
            return new ResponseDTO<Order>
            {
                Items=orders,
                TotalPages = totalPages,
                TotalItems = totalOrder
            };
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductDetail)
                        .ThenInclude(pd => pd.Size)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductDetail)
                        .ThenInclude(pd => pd.Color)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductDetail)
                        .ThenInclude(pd => pd.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsAsync(Guid orderId)
        {
            var result = await _context.OrderItems.Where(oi => oi.OrderId == orderId).ToListAsync();
            return result;
        }

        public async Task<ResponseDTO<CustomerOrderResponse>> GetOrdersByUserIdAsync(string userId, int pageNum, int pageSize)
        {
            var query = _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductDetail)
                        .ThenInclude(pd => pd.Color)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductDetail)
                        .ThenInclude(pd => pd.Size)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductDetail)
                        .ThenInclude(pd => pd.Product)
                .Where(o => o.UserId == userId);

            int totalItems = await query.CountAsync();  
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize); 

            var ordersById = await query
                .OrderByDescending(o => o.CreateAt) 
                .Skip((pageNum - 1) * pageSize)   
                .Take(pageSize)                      
                .ToListAsync();

            var result = new ResponseDTO<CustomerOrderResponse>
            {
                Items = ordersById.Select(order => new CustomerOrderResponse
                {
                    Status= order.Status,
                    OrderId = order.OrderId.ToString(),
                    UserId = order.UserId,
                    OrderDate = order.CreateAt,
                    TotalPrice = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice),
                    OrderItems = order.OrderItems.Select(oi => new OrderItemDetail
                    {
                        ProductName = oi.ProductDetail.Product.Name,
                        Quantity = oi.Quantity,
                        Total= oi.Quantity*oi.UnitPrice,
                        UnitPrice = oi.UnitPrice,
                        ColorName = oi.ProductDetail.Color.Name,
                        SizeName = oi.ProductDetail.Size.Name,
                        Image = oi.ProductDetail.Product.Image
                    }).ToList()
                }).ToList(),
                TotalItems = totalItems,
                TotalPages = totalPages
            };

            return result;
        }



        public Task<bool> OrderExistsAsync(Guid orderId)
        {
            var existOrder = _context.Orders.AnyAsync(o => o.OrderId == orderId);
            return existOrder;
        }

        public async Task<bool> UpdateOrderStatusAsync(Guid orderId, Order.OrderStatus status)
        {
            var existOrder = await _context.Orders.FindAsync(orderId);
            if (existOrder == null) return false;
            existOrder.Status = status;
            _context.Orders.Update(existOrder);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

    }
}
