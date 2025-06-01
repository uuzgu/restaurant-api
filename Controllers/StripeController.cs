using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Models;
using RestaurantApi.Data;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestaurantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        // POST: api/stripe/create-checkout-session
        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutSessionRequest request)
        {
            try
            {
                _logger.LogInformation("Received checkout session request: {@Request}", request);

                if (request.Items == null || !request.Items.Any())
                {
                    return BadRequest("No items in the order");
                }

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
                    Status = request.Status,
                    Total = request.TotalAmount.ToString(),
                    PaymentMethod = request.PaymentMethod,
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

                // For cash payments, return order details without creating a Stripe session
                if (request.PaymentMethod.ToLower() == "cash")
                {
                    return Ok(new { 
                        orderId = newOrder.Id,
                        orderNumber = newOrder.OrderNumber
                    });
                }

                // Create Stripe checkout session only for card payments
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Currency = "eur",
                                UnitAmount = (long)(request.TotalAmount * 100),
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = "Order Total",
                                },
                            },
                            Quantity = 1,
                        },
                    },
                    Mode = "payment",
                    SuccessUrl = $"{_configuration["FrontendUrl"]}/payment/success?session_id={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"{_configuration["FrontendUrl"]}/payment/cancel",
                    CustomerEmail = request.CustomerInfo.Email,
                    ShippingAddressCollection = request.OrderMethod.ToLower() == "delivery" ? new SessionShippingAddressCollectionOptions
                    {
                        AllowedCountries = new List<string> { "HU" },
                    } : null,
                    ShippingOptions = request.OrderMethod.ToLower() == "delivery" ? new List<SessionShippingOptionOptions>
                    {
                        new SessionShippingOptionOptions
                        {
                            ShippingRateData = new SessionShippingOptionShippingRateDataOptions
                            {
                                Type = "fixed_amount",
                                FixedAmount = new SessionShippingOptionShippingRateDataFixedAmountOptions
                                {
                                    Amount = 0,
                                    Currency = "eur",
                                },
                                DisplayName = "Free Delivery",
                                DeliveryEstimate = new SessionShippingOptionShippingRateDataDeliveryEstimateOptions
                                {
                                    Minimum = new SessionShippingOptionShippingRateDataDeliveryEstimateMinimumOptions
                                    {
                                        Unit = "hour",
                                        Value = 1,
                                    },
                                    Maximum = new SessionShippingOptionShippingRateDataDeliveryEstimateMaximumOptions
                                    {
                                        Unit = "hour",
                                        Value = 2,
                                    },
                                },
                            },
                        },
                    } : null,
                    Metadata = new Dictionary<string, string>
                    {
                        { "orderId", newOrder.Id.ToString() }
                    }
                };

                var service = new SessionService();
                var session = await service.CreateAsync(options);

                // Update order with Stripe session ID
                newOrder.StripeSessionId = session.Id;
                await _context.SaveChangesAsync();

                return Ok(new { 
                    url = session.Url, 
                    sessionId = session.Id,
                    orderId = newOrder.Id,
                    orderNumber = newOrder.OrderNumber
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating checkout session");
                _logger.LogError("Inner exception: {InnerException}", ex.InnerException?.Message);
                _logger.LogError("Stack trace: {StackTrace}", ex.StackTrace);
                return StatusCode(500, new { error = ex.Message, innerException = ex.InnerException?.Message });
            }
        }

        // GET: api/stripe/payment-success
        [HttpGet("payment-success")]
        public async Task<IActionResult> PaymentSuccess([FromQuery] string session_id)
        {
            try
            {
                if (string.IsNullOrEmpty(session_id))
                {
                    return BadRequest("Session ID is required");
                }

                var service = new SessionService();
                var session = await service.GetAsync(session_id);

                if (session == null)
                {
                    return NotFound("Session not found");
                }

                // Get the order from the database using the session ID
                var order = await _context.Orders
                    .Include(o => o.CustomerInfo)
                    .FirstOrDefaultAsync(o => o.StripeSessionId == session_id);

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

        // POST: api/stripe/payment-cancel
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
        public required List<StripeCheckoutItem> Items { get; set; }

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

    public class CustomerInfoRequest
    {
        [Required]
        [JsonPropertyName("firstName")]
        public required string FirstName { get; set; }

        [Required]
        [JsonPropertyName("lastName")]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [Required]
        [Phone]
        [JsonPropertyName("phone")]
        public required string Phone { get; set; }

        [JsonPropertyName("postalCode")]
        public string? PostalCode { get; set; }

        [JsonPropertyName("street")]
        public string? Street { get; set; }

        [JsonPropertyName("house")]
        public string? House { get; set; }

        [JsonPropertyName("stairs")]
        public string? Stairs { get; set; }

        [JsonPropertyName("stick")]
        public string? Stick { get; set; }

        [JsonPropertyName("door")]
        public string? Door { get; set; }

        [JsonPropertyName("bell")]
        public string? Bell { get; set; }
    }

    public class CreatePaymentIntentRequest
    {
        public int OrderId { get; set; }
    }
}
