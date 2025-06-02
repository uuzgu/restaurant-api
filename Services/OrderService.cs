using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Models;

namespace RestaurantApi.Services
{
    public class OrderService
    {
        private readonly RestaurantContext _context;

        public OrderService(RestaurantContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetOrderById(int id)
        {
            return await _context.Orders
                .Include(o => o.CustomerInfo)
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order?> GetOrderByNumber(string orderNumber)
        {
            return await _context.Orders
                .Include(o => o.CustomerInfo)
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
        }

        public async Task<Order> CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Order> CreateOrderAsync(
            List<OrderItemRequest> items,
            CustomerOrderInfo? customerInfo,
            string status,
            string paymentMethod,
            decimal totalAmount)
        {
            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(),
                Status = status,
                Total = totalAmount.ToString(),
                PaymentMethod = paymentMethod,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CustomerInfo = new CustomerOrderInfo
                {
                    FirstName = customerInfo.FirstName,
                    LastName = customerInfo.LastName,
                    Email = customerInfo.Email,
                    Phone = customerInfo.Phone,
                    Comment = customerInfo.Comment,
                    CreateDate = DateTime.UtcNow,
                    PostalCode = customerInfo.PostalCode,
                    Street = customerInfo.Street,
                    House = customerInfo.House,
                    Stairs = customerInfo.Stairs,
                    Door = customerInfo.Door,
                    Bell = customerInfo.Bell
                }
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Create order details
            foreach (var item in items)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ItemId = item.Id,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Notes = item.Notes
                };
                _context.OrderDetails.Add(orderDetail);
            }
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Order?> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return null;
            }

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return order;
        }

        private string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }
} 