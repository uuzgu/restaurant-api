using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models
{
    [Table("coupons_history")]
    public class CouponHistory
    {
        public int Id { get; set; }

        [Required]
        [Column("coupon_id")]
        public int CouponId { get; set; }

        [ForeignKey("CouponId")]
        public Coupon Coupon { get; set; }

        public string? Email { get; set; }

        [Column("used_at")]
        public string? UsedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
} 