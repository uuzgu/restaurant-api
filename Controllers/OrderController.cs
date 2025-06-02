using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Models;
using RestaurantApi.Services;
using Stripe;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly RestaurantContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrderController> _logger;
        private readonly OrderService _orderService;

        public OrderController(RestaurantContext context, IConfiguration configuration, ILogger<OrderController> logger, OrderService orderService)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                // Validate required fields based on order method
                if (request.OrderMethod.ToLower() == "delivery")
                {
                    if (string.IsNullOrEmpty(request.CustomerInfo.PostalCode) ||
                        string.IsNullOrEmpty(request.CustomerInfo.Street) ||
                        string.IsNullOrEmpty(request.CustomerInfo.House))
                    {
                        return BadRequest(new { Error = "Postal code, street, and house number are required for delivery orders" });
                    }
                }

                // Create a new order
                var order = new Order
                {
                    OrderNumber = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                    Status = request.Status,
                    Total = request.TotalAmount.ToString(),
                    PaymentMethod = request.PaymentMethod,
                    OrderMethod = request.OrderMethod,
                    SpecialNotes = request.SpecialNotes,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Add customer info if provided
                if (request.CustomerInfo != null)
                {
                    order.CustomerInfo = new CustomerOrderInfo
                    {
                        FirstName = request.CustomerInfo.FirstName,
                        LastName = request.CustomerInfo.LastName,
                        Email = request.CustomerInfo.Email,
                        Phone = request.CustomerInfo.Phone,
                        Comment = request.CustomerInfo.Comment,
                        CreateDate = DateTime.UtcNow,
                        PostalCode = request.OrderMethod.ToLower() == "delivery" ? request.CustomerInfo.PostalCode : null,
                        Street = request.OrderMethod.ToLower() == "delivery" ? request.CustomerInfo.Street : null,
                        House = request.OrderMethod.ToLower() == "delivery" ? request.CustomerInfo.House : null,
                        Stairs = request.OrderMethod.ToLower() == "delivery" ? request.CustomerInfo.Stairs : null,
                        Stick = request.OrderMethod.ToLower() == "delivery" ? request.CustomerInfo.Stick : null,
                        Door = request.OrderMethod.ToLower() == "delivery" ? request.CustomerInfo.Door : null,
                        Bell = request.OrderMethod.ToLower() == "delivery" ? request.CustomerInfo.Bell : null
                    };
                }

                // Check for required options
                foreach (var item in request.Items)
                {
                    var itemWithOptions = await _context.Items
                        .Include(i => i.ItemSelectionGroups)
                            .ThenInclude(isg => isg.SelectionGroup)
                                .ThenInclude(sg => sg.SelectionOptions)
                        .FirstOrDefaultAsync(i => i.Id == item.Id);

                    if (itemWithOptions != null)
                    {
                        foreach (var selectionGroup in itemWithOptions.ItemSelectionGroups.Select(isg => isg.SelectionGroup))
                        {
                            if (selectionGroup.IsRequired == 1 && !selectionGroup.SelectionOptions.Any(o => item.SelectedOptions.Contains(o.Id)))
                            {
                                return BadRequest(new { Error = $"Required option not selected for {itemWithOptions.Name} in {selectionGroup.Name}" });
                            }
                        }
                    }
                }

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Create order details
                foreach (var item in request.Items)
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

                return Ok(new { OrderId = order.Id, OrderNumber = order.OrderNumber });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("create-cash-order")]
        [Route("create-cash-order")]
        public async Task<IActionResult> CreateCashOrder([FromBody] CreateCashOrderRequest request)
        {
            try
            {
                _logger.LogInformation("Received cash order request: {@Request}", request);
                
                if (request.Items == null || !request.Items.Any())
                {
                    return BadRequest("No items in the order");
                }

                // Create customer info
                var customerInfo = new CustomerOrderInfo
                {
                    FirstName = request.CustomerInfo.FirstName,
                    LastName = request.CustomerInfo.LastName,
                    Email = request.CustomerInfo.Email,
                    Phone = request.CustomerInfo.Phone,
                    CreateDate = DateTime.UtcNow,
                    PostalCode = request.CustomerInfo.PostalCode,
                    Street = request.CustomerInfo.Street,
                    House = request.CustomerInfo.House,
                    Stairs = request.CustomerInfo.Stairs,
                    Stick = request.CustomerInfo.Stick,
                    Door = request.CustomerInfo.Door,
                    Bell = request.CustomerInfo.Bell
                };
                _context.CustomerOrderInfos.Add(customerInfo);
                await _context.SaveChangesAsync();

                // Create order
                var newOrder = new Order
                {
                    OrderNumber = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                    Status = "pending",
                    Total = request.TotalAmount.ToString(),
                    PaymentMethod = "cash",
                    OrderMethod = request.OrderMethod,
                    SpecialNotes = request.SpecialNotes,
                    CustomerInfoId = customerInfo.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync();

                // Create order details
                foreach (var item in request.Items)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = newOrder.Id,
                        ItemId = item.Id,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Notes = item.Notes
                    };

                    _context.OrderDetails.Add(orderDetail);
                }
                await _context.SaveChangesAsync();

                return Ok(new { OrderId = newOrder.Id, OrderNumber = newOrder.OrderNumber });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating cash order");
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.CustomerInfo)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Item)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                {
                    return NotFound();
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching order {OrderId}", id);
                return StatusCode(500, new { Error = "An error occurred while fetching the order" });
            }
        }
    }

    public class CreateOrderRequest
    {
        [Required]
        [JsonPropertyName("orderMethod")]
        public string OrderMethod { get; set; }

        [Required]
        [JsonPropertyName("paymentMethod")]
        public string PaymentMethod { get; set; }

        [Required]
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [Required]
        [JsonPropertyName("totalAmount")]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("specialNotes")]
        public string? SpecialNotes { get; set; }

        [Required]
        [JsonPropertyName("customerInfo")]
        public CustomerInfoRequest CustomerInfo { get; set; }

        [Required]
        [JsonPropertyName("items")]
        public List<OrderItemRequest> Items { get; set; }
    }

    public class CreateCashOrderRequest
    {
        [Required]
        [JsonPropertyName("orderMethod")]
        public string OrderMethod { get; set; }

        [Required]
        [JsonPropertyName("totalAmount")]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("specialNotes")]
        public string? SpecialNotes { get; set; }

        [Required]
        [JsonPropertyName("customerInfo")]
        public CustomerInfoRequest CustomerInfo { get; set; }

        [Required]
        [JsonPropertyName("items")]
        public List<OrderItemRequest> Items { get; set; }
    }

    public class CustomerInfoRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? PostalCode { get; set; }
        public string? Street { get; set; }
        public string? House { get; set; }
        public string? Stairs { get; set; }
        public string? Stick { get; set; }
        public string? Door { get; set; }
        public string? Bell { get; set; }
        public string? Comment { get; set; }
    }

    public class OrderItemRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string? Notes { get; set; }
        public List<int> SelectedOptions { get; set; } = new List<int>();
        public List<SelectedItemRequest>? SelectedItems { get; set; }
    }

    public class SelectedItemRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
} 