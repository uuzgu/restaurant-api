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
                                .ThenInclude(sg => sg.Options)
                        .FirstOrDefaultAsync(i => i.Id == item.Id);

                    if (itemWithOptions != null)
                    {
                        foreach (var selectionGroup in itemWithOptions.ItemSelectionGroups.Select(isg => isg.SelectionGroup))
                        {
                            if (selectionGroup.IsRequired && !selectionGroup.Options.Any(o => item.SelectedOptions.Contains(o.Id)))
                            {
                                return BadRequest(new { Error = $"Required option not selected for {itemWithOptions.Name} in {selectionGroup.Name}" });
                            }
                        }
                    }
                }

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Create order details
                var orderDetails = new OrderDetails
                {
                    OrderId = order.Id,
                    ItemDetails = System.Text.Json.JsonSerializer.Serialize(
                        request.Items.Select(item => new
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

                return Ok(new { OrderId = order.Id, OrderNumber = order.OrderNumber });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("create-cash-order")]
        public async Task<IActionResult> CreateCashOrder([FromBody] CreateCashOrderRequest request)
        {
            try
            {
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
                var orderDetails = new OrderDetails
                {
                    OrderId = newOrder.Id,
                    ItemDetails = System.Text.Json.JsonSerializer.Serialize(
                        request.Items.Select(item => new
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Quantity = item.Quantity,
                            Price = item.Price,
                            SelectedItems = item.SelectedItems?.Select(selectedItem => new
                            {
                                Id = selectedItem.Id,
                                Name = selectedItem.Name,
                                Quantity = selectedItem.Quantity,
                                Price = selectedItem.Price
                            }).ToList()
                        }).ToList()
                    )
                };
                _context.OrderDetails.Add(orderDetails);
                await _context.SaveChangesAsync();

                return Ok(new { 
                    orderId = newOrder.Id,
                    orderNumber = newOrder.OrderNumber
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating cash order");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        private string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.CustomerInfo)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }
    }

    public class CreateCashOrderRequest
    {
        [Required]
        [JsonPropertyName("items")]
        public required List<OrderItemRequest> Items { get; set; }

        [Required]
        [JsonPropertyName("customerInfo")]
        public required CustomerInfoRequest CustomerInfo { get; set; }

        [Required]
        [JsonPropertyName("orderMethod")]
        public required string OrderMethod { get; set; }

        [JsonPropertyName("specialNotes")]
        public string? SpecialNotes { get; set; }

        [Required]
        [JsonPropertyName("totalAmount")]
        public required decimal TotalAmount { get; set; }
    }
} 