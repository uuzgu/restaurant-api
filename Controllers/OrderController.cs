using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Models;
using RestaurantApi.Services;
using Stripe;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

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
                    if (string.IsNullOrEmpty(request.CustomerInfo?.PostalCode) ||
                        string.IsNullOrEmpty(request.CustomerInfo?.Street) ||
                        string.IsNullOrEmpty(request.CustomerInfo?.House))
                    {
                        return BadRequest(new { Error = "Postal code, street, and house number are required for delivery orders" });
                    }
                }

                // Create a new order
                var order = new Order
                {
                    OrderNumber = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                    Status = request.Status,
                    Total = request.TotalAmount,
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
                        CreateDate = DateTime.UtcNow
                    };

                    // Create delivery address if needed
                    if (request.OrderMethod.ToLower() == "delivery" && !string.IsNullOrEmpty(request.CustomerInfo.PostalCode))
                    {
                        var postcode = await _context.Postcodes
                            .FirstOrDefaultAsync(p => p.Code == request.CustomerInfo.PostalCode);

                        if (postcode == null)
                        {
                            return BadRequest(new { Error = "Invalid postal code" });
                        }

                        var deliveryAddress = new DeliveryAddress
                        {
                            PostcodeId = postcode.Id,
                            Street = request.CustomerInfo.Street ?? string.Empty,
                            House = request.CustomerInfo.House,
                            Stairs = request.CustomerInfo.Stairs,
                            Stick = request.CustomerInfo.Stick,
                            Door = request.CustomerInfo.Door,
                            Bell = request.CustomerInfo.Bell
                        };
                        _context.DeliveryAddresses.Add(deliveryAddress);
                        await _context.SaveChangesAsync();
                    }
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
                            if (selectionGroup.IsRequired && !selectionGroup.SelectionOptions.Any(o => item.SelectedOptions.Contains(o.Id)))
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
                        ItemDetails = JsonSerializer.Serialize(new
                        {
                            ItemId = item.Id,
                            Quantity = item.Quantity,
                            Price = item.Price,
                            Notes = item.Notes,
                            SelectedOptions = item.SelectedOptions
                        })
                    };

                    _context.OrderDetails.Add(orderDetail);
                }
                await _context.SaveChangesAsync();

                // Log recent orders after creation
                var recentOrders = await _context.Orders
                    .Include(o => o.CustomerInfo)
                    .Include(o => o.OrderDetails)
                    .OrderByDescending(o => o.Id)
                    .Take(5)
                    .ToListAsync();

                _logger.LogInformation("Recent Orders after creation: {Orders}", 
                    JsonSerializer.Serialize(recentOrders, new JsonSerializerOptions { WriteIndented = true }));

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
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Received cash order request: {@Request}", request);
                
                if (request.Items == null || !request.Items.Any())
                {
                    return BadRequest("No items in the order");
                }

                // Validate delivery information if it's a delivery order
                if (request.OrderMethod.ToLower() == "delivery")
                {
                    if (string.IsNullOrEmpty(request.CustomerInfo.PostalCode))
                    {
                        return BadRequest(new { Error = "Postal code is required for delivery orders" });
                    }
                    if (string.IsNullOrEmpty(request.CustomerInfo.Street))
                    {
                        return BadRequest(new { Error = "Street address is required for delivery orders" });
                    }
                    if (string.IsNullOrEmpty(request.CustomerInfo.House))
                    {
                        return BadRequest(new { Error = "House number is required for delivery orders" });
                    }
                }

                // Create customer info
                var customerInfo = new CustomerOrderInfo
                {
                    FirstName = request.CustomerInfo.FirstName,
                    LastName = request.CustomerInfo.LastName,
                    Email = request.CustomerInfo.Email,
                    Phone = request.CustomerInfo.Phone,
                    Comment = request.CustomerInfo.Comment,
                    CreateDate = DateTime.UtcNow
                };
                _context.CustomerOrderInfos.Add(customerInfo);
                await _context.SaveChangesAsync();

                // Create delivery address if needed
                DeliveryAddress? deliveryAddress = null;
                if (request.OrderMethod.ToLower() == "delivery" && !string.IsNullOrEmpty(request.CustomerInfo.PostalCode))
                {
                    var postcode = await _context.Postcodes
                        .FirstOrDefaultAsync(p => p.Code == request.CustomerInfo.PostalCode);

                    if (postcode == null)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(new { Error = "Invalid postal code" });
                    }

                    deliveryAddress = new DeliveryAddress
                    {
                        PostcodeId = postcode.Id,
                        Street = request.CustomerInfo.Street ?? string.Empty,
                        House = request.CustomerInfo.House,
                        Stairs = request.CustomerInfo.Stairs,
                        Stick = request.CustomerInfo.Stick,
                        Door = request.CustomerInfo.Door,
                        Bell = request.CustomerInfo.Bell
                    };
                    _context.DeliveryAddresses.Add(deliveryAddress);
                    await _context.SaveChangesAsync();
                }

                // Create the order
                var order = new Order
                {
                    OrderNumber = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                    Status = request.Status,
                    Total = request.TotalAmount,
                    PaymentMethod = request.PaymentMethod,
                    OrderMethod = request.OrderMethod,
                    SpecialNotes = request.SpecialNotes,
                    CustomerInfo = customerInfo,
                    DeliveryAddress = deliveryAddress,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Create order details
                foreach (var item in request.Items)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        ItemDetails = JsonSerializer.Serialize(new
                        {
                            ItemId = item.Id,
                            Quantity = item.Quantity,
                            Price = item.Price,
                            Notes = item.Notes,
                            SelectedOptions = item.SelectedOptions
                        })
                    };

                    _context.OrderDetails.Add(orderDetail);
                }
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                // Log recent orders after creation
                var recentOrders = await _context.Orders
                    .Include(o => o.CustomerInfo)
                    .Include(o => o.OrderDetails)
                    .OrderByDescending(o => o.Id)
                    .Take(5)
                    .ToListAsync();

                _logger.LogInformation("Recent Orders after cash order creation: {Orders}", 
                    JsonSerializer.Serialize(recentOrders, new JsonSerializerOptions { WriteIndented = true }));

                _logger.LogInformation("Successfully created cash order with ID: {OrderId}", order.Id);

                return Ok(new { 
                    OrderId = order.Id, 
                    OrderNumber = order.OrderNumber,
                    Status = order.Status,
                    Total = order.Total,
                    PaymentMethod = order.PaymentMethod,
                    OrderMethod = order.OrderMethod,
                    CreatedAt = order.CreatedAt
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating cash order");
                
                // Log inner exception if it exists
                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner exception: {InnerException}", ex.InnerException.Message);
                    _logger.LogError("Inner exception stack trace: {StackTrace}", ex.InnerException.StackTrace);
                }
                
                return StatusCode(500, new { 
                    Error = "An error occurred while creating the order", 
                    Details = ex.Message,
                    InnerException = ex.InnerException?.Message
                });
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
        public string Status { get; set; } = string.Empty;

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required]
        public string OrderMethod { get; set; } = string.Empty;

        [JsonPropertyName("specialNotes")]
        public string? SpecialNotes { get; set; }

        [Required]
        public List<OrderItemRequest> Items { get; set; } = new();

        [Required]
        public CustomerInfoRequest CustomerInfo { get; set; } = new();
    }

    public class CreateCashOrderRequest
    {
        [Required]
        public string Status { get; set; } = string.Empty;

        [Required]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("specialNotes")]
        public string? SpecialNotes { get; set; }

        [Required]
        public string OrderMethod { get; set; } = string.Empty;

        [Required]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required]
        public List<OrderItemRequest> Items { get; set; } = new();

        [Required]
        public CustomerInfoRequest CustomerInfo { get; set; } = new();
    }

    public class CustomerInfoRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

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
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? Notes { get; set; }

        public List<int> SelectedOptions { get; set; } = new();

        public List<SelectedItemRequest>? SelectedItems { get; set; }
    }

    public class SelectedItemRequest
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
} 