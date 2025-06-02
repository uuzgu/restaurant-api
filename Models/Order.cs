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

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("customer_info_id")]
        public int? CustomerInfoId { get; set; }

        [Required]
        [Column("order_method")]
        public string OrderMethod { get; set; }

        [Required]
        [Column("order_number")]
        public string OrderNumber { get; set; }

        [Required]
        [Column("payment_method")]
        public string PaymentMethod { get; set; }

        [Column("special_notes")]
        public string? SpecialNotes { get; set; }

        [Required]
        [Column("status")]
        public string Status { get; set; }

        [Column("stripe_session_id")]
        public string? StripeSessionId { get; set; }

        [Required]
        [Column("total")]
        public string Total { get; set; }

        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        // Navigation properties
        public virtual CustomerOrderInfo? CustomerInfo { get; set; }
        public virtual OrderDetails? OrderDetails { get; set; }
    }

    [Table("order_details")]
    public class OrderDetails
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("order_id")]
        public int OrderId { get; set; }

        [Required]
        [Column("item_details")]
        public string ItemDetails { get; set; }

        // Navigation property
        public virtual Order Order { get; set; }
    }
} 