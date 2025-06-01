using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models
{
    [Table("orders")]
    public class Order
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("order_number")]
        public string OrderNumber { get; set; } = string.Empty;

        [Column("status")]
        public string Status { get; set; } = "pending";

        [Column("total")]
        public string Total { get; set; } = "0";

        [Column("payment_method")]
        public string PaymentMethod { get; set; } = "card";

        [Column("order_method")]
        public string OrderMethod { get; set; } = "delivery";

        [Column("special_notes")]
        public string? SpecialNotes { get; set; }

        [Column("customer_info_id")]
        public int? CustomerInfoId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("stripe_session_id")]
        public string? StripeSessionId { get; set; }

        // Navigation properties
        public CustomerOrderInfo? CustomerInfo { get; set; }
        public OrderDetails? OrderDetails { get; set; }
    }

    [Table("order_details")]
    public class OrderDetails
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("item_details")]
        public string ItemDetails { get; set; } = string.Empty;

        // Navigation property
        public Order Order { get; set; } = null!;
    }
} 