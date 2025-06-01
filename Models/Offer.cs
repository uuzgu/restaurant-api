using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models
{
    [Table("offers")]
    public class Offer
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        
        [Column("name")]
        public string Name { get; set; } = string.Empty;
        
        [Column("description")]
        public string? Description { get; set; }
        
        [Column("discount_percentage")]
        public decimal DiscountPercentage { get; set; }
        
        [Column("start_date")]
        public string StartDate { get; set; } = string.Empty;
        
        [Column("end_date")]
        public string EndDate { get; set; } = string.Empty;
        
        [Column("is_active")]
        public bool IsActive { get; set; }

        // Navigation property
        public ICollection<ItemOffer> ItemOffers { get; set; } = new List<ItemOffer>();
    }
} 