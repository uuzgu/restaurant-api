using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Models;
using Stripe;
using Stripe.Checkout;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace RestaurantApi.Controllers
{
    [ApiController]
    [Route("api/stripe")]
    public class StripeWebhookController : ControllerBase
    {
        private readonly RestaurantContext _context;
        private readonly string _webhookSecret;
        private const string CHECKOUT_SESSION_COMPLETED = "checkout.session.completed";
        private const string CHECKOUT_SESSION_EXPIRED = "checkout.session.expired";

        public StripeWebhookController(RestaurantContext context, IConfiguration configuration)
        {
            _context = context;
            _webhookSecret = configuration["Stripe:WebhookSecret"];
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signature = Request.Headers["Stripe-Signature"];
            
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    signature,
                    _webhookSecret
                );
                
                Console.WriteLine($"Received Stripe event: {stripeEvent.Type}");

                switch (stripeEvent.Type)
                {
                    case CHECKOUT_SESSION_COMPLETED:
                        var session = stripeEvent.Data.Object as Session;
                        if (session != null)
                        {
                            Console.WriteLine($"Processing completed checkout session: {session.Id}");
                            Console.WriteLine($"Session metadata: {string.Join(", ", session.Metadata.Select(kv => $"{kv.Key}={kv.Value}"))}");
                            
                            if (session.Metadata.TryGetValue("orderId", out string orderIdStr) && 
                                int.TryParse(orderIdStr, out int orderId))
                            {
                                Console.WriteLine($"Found order ID in metadata: {orderId}");
                                var order = await _context.Orders.FindAsync(orderId);
                                if (order != null)
                                {
                                    order.Status = "paid";
                                    order.UpdatedAt = DateTime.UtcNow;
                                    await _context.SaveChangesAsync();
                                    Console.WriteLine($"Order {order.OrderNumber} marked as paid");
                                }
                                else
                                {
                                    Console.WriteLine($"Order {orderId} not found in database");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No order ID found in session metadata");
                            }
                        }
                        break;

                    case CHECKOUT_SESSION_EXPIRED:
                        var expiredSession = stripeEvent.Data.Object as Session;
                        if (expiredSession != null)
                        {
                            Console.WriteLine($"Processing expired checkout session: {expiredSession.Id}");
                            Console.WriteLine($"Session metadata: {string.Join(", ", expiredSession.Metadata.Select(kv => $"{kv.Key}={kv.Value}"))}");
                            
                            if (expiredSession.Metadata.TryGetValue("orderId", out string expiredOrderIdStr) && 
                                int.TryParse(expiredOrderIdStr, out int expiredOrderId))
                            {
                                Console.WriteLine($"Found order ID in metadata: {expiredOrderId}");
                                var order = await _context.Orders.FindAsync(expiredOrderId);
                                if (order != null)
                                {
                                    order.Status = "expired";
                                    order.UpdatedAt = DateTime.UtcNow;
                                    await _context.SaveChangesAsync();
                                    Console.WriteLine($"Order {order.OrderNumber} marked as expired");
                                }
                                else
                                {
                                    Console.WriteLine($"Order {expiredOrderId} not found in database");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No order ID found in session metadata");
                            }
                        }
                        break;

                    default:
                        Console.WriteLine($"Unhandled event type: {stripeEvent.Type}");
                        break;
                }

                return Ok();
            }
            catch (StripeException ex)
            {
                Console.WriteLine($"Stripe error: {ex.Message}");
                return BadRequest();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing webhook: {ex.Message}");
                return StatusCode(500);
            }
        }
    }
} 