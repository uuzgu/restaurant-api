using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models
{
    public class Coupon
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Type { get; set; }  // e.g., "PERCENTAGE", "FIXED_AMOUNT"

        [Required]
        [Column("is_periodic")]
        public int IsPeriodic { get; set; }  // 1 for periodic, 0 for one-time use

        [Column("start_date")]
        public string? StartDate { get; set; }  // For periodic coupons

        [Column("end_date")]
        public string? EndDate { get; set; }  // For periodic coupons

        [Column("discount_ratio")]
        public decimal DiscountRatio { get; set; }  // Discount percentage as decimal (e.g., 0.10 for 10%)

        public string? Email { get; set; }

        [Column("is_used")]
        public int IsUsed { get; set; }  // 1 for used, 0 for unused

        [Column("created_at")]
        public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        public CouponSchedule Schedule { get; set; }
    }
} 