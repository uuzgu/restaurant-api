using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Models;
using Stripe;
using Stripe.Checkout;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestaurantApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StripeController : ControllerBase
    {
        private readonly RestaurantContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<StripeController> _logger;

        public StripeController(RestaurantContext context, IConfiguration configuration, ILogger<StripeController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutSessionRequest request)
        {
            try
            {
                var stripeApiKey = _configuration["Stripe:SecretKey"];
                if (string.IsNullOrEmpty(stripeApiKey))
                {
                    return StatusCode(500, "Stripe API key is not configured");
                }

                StripeConfiguration.ApiKey = stripeApiKey;

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

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = request.Items.Select(item => new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "eur",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Name,
                            },
                            UnitAmount = (long)(item.Price * 100), // Convert to cents
                        },
                        Quantity = item.Quantity,
                    }).ToList(),
                    Mode = "payment",
                    SuccessUrl = $"{_configuration["FrontendUrl"]}/payment-success?session_id={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"{_configuration["FrontendUrl"]}/payment-cancel",
                    CustomerEmail = request.CustomerInfo.Email,
                    Metadata = new Dictionary<string, string>
                    {
                        { "orderId", order.Id.ToString() },
                        { "orderNumber", order.OrderNumber }
                    }
                };

                var service = new SessionService();
                var session = await service.CreateAsync(options);

                return Ok(new { sessionId = session.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating checkout session");
                return StatusCode(500, $"Error creating checkout session: {ex.Message}");
            }
        }

        [HttpGet("payment-success")]
        public async Task<IActionResult> PaymentSuccess([FromQuery] string session_id)
        {
            try
            {
                var service = new SessionService();
                var session = await service.GetAsync(session_id);

                if (session == null)
                {
                    return NotFound("Session not found");
                }

                // Update order status in database
                var order = await _context.Orders.FindAsync(int.Parse(session.Metadata["orderId"]));
                if (order == null)
                {
                    return NotFound("Order not found");
                }

                // Update order status if payment is successful
                if (session.PaymentStatus == "paid")
                {
                    order.Status = "completed";
                    order.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }

                return Ok(new
                {
                    orderId = order.Id,
                    orderNumber = order.OrderNumber,
                    total = order.Total,
                    status = order.Status,
                    paymentMethod = order.PaymentMethod
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment success");
                return StatusCode(500, "An error occurred while processing the payment");
            }
        }

        [HttpPost("payment-cancel")]
        public async Task<ActionResult> HandlePaymentCancel([FromBody] PaymentSuccessRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.SessionId))
                {
                    return BadRequest("Session ID is required");
                }

                var service = new SessionService();
                var session = await service.GetAsync(request.SessionId);

                if (session == null)
                {
                    return NotFound("Session not found");
                }

                // Update order status in database
                var order = await _context.Orders.FindAsync(int.Parse(session.Metadata["orderId"]));
                if (order == null)
                {
                    return NotFound("Order not found");
                }

                order.Status = "cancelled";
                order.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Payment cancelled successfully" });
            }
            catch (StripeException ex)
            {
                return StatusCode(500, $"Stripe error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentRequest request)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderDetails)
                    .FirstOrDefaultAsync(o => o.Id == request.OrderId);

                if (order == null)
                {
                    return NotFound("Order not found");
                }

                var stripeApiKey = _configuration["Stripe:SecretKey"];
                if (string.IsNullOrEmpty(stripeApiKey))
                {
                    return StatusCode(500, "Stripe API key is not configured");
                }

                StripeConfiguration.ApiKey = stripeApiKey;

                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(decimal.Parse(order.Total) * 100), // Convert to cents
                    Currency = "eur",
                    PaymentMethodTypes = new List<string> { "card" },
                    Metadata = new Dictionary<string, string>
                    {
                        { "orderId", order.Id.ToString() },
                        { "orderNumber", order.OrderNumber }
                    }
                };

                var service = new PaymentIntentService();
                var intent = await service.CreateAsync(options);

                return Ok(new { clientSecret = intent.ClientSecret });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating payment intent: {ex.Message}");
            }
        }
    }

    public class PaymentSuccessRequest
    {
        [Required]
        [JsonPropertyName("sessionId")]
        public string SessionId { get; set; } = string.Empty;
    }

    public class CreateCheckoutSessionRequest
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

        [Required]
        [JsonPropertyName("paymentMethod")]
        public required string PaymentMethod { get; set; }

        [Required]
        [JsonPropertyName("status")]
        public required string Status { get; set; }

        [JsonPropertyName("specialNotes")]
        public string? SpecialNotes { get; set; }

        [Required]
        [JsonPropertyName("totalAmount")]
        public required decimal TotalAmount { get; set; }
    }

    public class CreatePaymentIntentRequest
    {
        public int OrderId { get; set; }
    }
}
