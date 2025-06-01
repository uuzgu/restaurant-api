using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Models
{
    public class PostcodeMinimumOrder
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Postcode { get; set; }
        
        [Required]
        public decimal MinimumOrderValue { get; set; }
    }
} 