using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models
{
    [Table("coupon_schedule")]
    public class CouponSchedule
    {
        public int Id { get; set; }

        [Required]
        [Column("coupon_id")]
        public int CouponId { get; set; }

        [ForeignKey("CouponId")]
        public Coupon Coupon { get; set; }

        [Required]
        [Column("monday")]
        public int Monday { get; set; }

        [Required]
        [Column("tuesday")]
        public int Tuesday { get; set; }

        [Required]
        [Column("wednesday")]
        public int Wednesday { get; set; }

        [Required]
        [Column("thursday")]
        public int Thursday { get; set; }

        [Required]
        [Column("friday")]
        public int Friday { get; set; }

        [Required]
        [Column("saturday")]
        public int Saturday { get; set; }

        [Required]
        [Column("sunday")]
        public int Sunday { get; set; }

        [Required]
        [Column("begin_time")]
        public string BeginTime { get; set; }

        [Required]
        [Column("end_time")]
        public string EndTime { get; set; }

        [Column("created_at")]
        public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        [Column("updated_at")]
        public string UpdatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
} 