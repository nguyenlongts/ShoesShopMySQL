using Azure.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoesShop.Application.DTOs;
using ShoesShop.Application.Interfaces.Services;
using ShoesShop.Domain.Entities;
using static ShoesShop.Domain.Entities.Order;

namespace ShoesShop.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            var (result,message,OrderId) = await _orderService.CreateOrderAsync(createOrderDto);
            if (!result) return BadRequest("Không thể tạo đơn hàng.");
            return Ok(OrderId);
        }



        [HttpGet]
        public async Task<IActionResult> GetAllOrders([FromQuery] int pageNum = 1, [FromQuery] int pageSize = 10)
        {
            var orders = await _orderService.GetAllOrdersAsync(pageNum, pageSize);
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null) return NotFound("Đơn hàng không tồn tại.");
            var detail = new OrderDetailResponse
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                Fullname = $"{order.User.FirstName} {order.User.LastName}",
                ShippingAddress = order.ShippingAddress,
                CreateAt = order.CreateAt,
                Status = order.Status,
                TotalPrice = order.TotalPrice,
                PhoneNumber = order.User.PhoneNumber,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDetail
                {
                    Quantity = oi.Quantity,
                    ColorName = oi.ProductDetail.Color.Name,
                    SizeName = oi.ProductDetail.Size.Name,
                    UnitPrice = oi.UnitPrice,
                    Total = oi.Total,
                    ProductName = oi.ProductDetail.Product.Name,
                    Image = oi.ProductDetail.ImageUrl
                }).ToList()
                
            };
            return Ok(detail);
        }

        [Authorize]
        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusRequest request)
        {
            var result = await _orderService.UpdateOrderStatusAsync(request.OrderId, ((Order.OrderStatus)request.Status));
            if (!result) return BadRequest("Cập nhật trạng thái thất bại.");
            return Ok("Cập nhật trạng thái thành công.");
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            var result = await _orderService.DeleteOrderAsync(orderId);
            if (!result) return BadRequest("Xóa đơn hàng thất bại.");
            return Ok("Đơn hàng đã được xóa.");
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(string userId, [FromQuery] int pageNum = 1, [FromQuery] int pageSize = 10)
        {
            var orders = await _orderService.GetOrdersByUserAsync(userId, pageNum, pageSize);
            return Ok(orders);
        }


        
    }

}
