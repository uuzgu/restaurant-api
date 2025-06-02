using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models
{
    [Table("postcode_minimum_orders")]
    public class PostcodeMinimumOrder
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Required]
        [Column("postcode")]
        public string Postcode { get; set; }
        
        [Required]
        [Column("minimum_order_value")]
        public decimal MinimumOrderValue { get; set; }

        // Navigation property
        public virtual Postcode? PostcodeInfo { get; set; }
    }
} 