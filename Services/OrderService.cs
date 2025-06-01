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

        public async Task<List<Order>> GetOrdersByUserId(int userId)
        {
            return await _context.Orders
                .Include(o => o.CustomerInfo)
                .Include(o => o.OrderDetails)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
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
                    Address = new Address
                    {
                        PostalCode = customerInfo.Address?.PostalCode,
                        Street = customerInfo.Address?.Street,
                        House = customerInfo.Address?.House,
                        Stairs = customerInfo.Address?.Stairs,
                        Door = customerInfo.Address?.Door,
                        Bell = customerInfo.Address?.Bell
                    }
                }
            };

            if (customerInfo != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == customerInfo.Email);
                if (user == null)
                {
                    user = new User
                    {
                        FirstName = customerInfo.FirstName,
                        LastName = customerInfo.LastName,
                        Email = customerInfo.Email,
                        Password = Guid.NewGuid().ToString(),
                        Phone = customerInfo.Phone,
                        PostalCode = customerInfo.Address?.PostalCode,
                        Address = customerInfo.Address?.Street,
                        House = customerInfo.Address?.House,
                        Stairs = customerInfo.Address?.Stairs,
                        Door = customerInfo.Address?.Door,
                        Bell = customerInfo.Address?.Bell,
                        Comment = customerInfo.Comment
                    };
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }
                order.UserId = user.Id;
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Create order details
            var orderDetails = new OrderDetails
            {
                OrderId = order.Id,
                ItemDetails = System.Text.Json.JsonSerializer.Serialize(
                    items.Select(item => new
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        SelectedOptions = item.SelectedOptions.Select(optionId => new
                        {
                            OptionId = optionId,
                            Quantity = 1 // Default quantity, can be updated if needed
                        }).ToList()
                    }).ToList()
                )
            };

            _context.OrderDetails.Add(orderDetails);
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