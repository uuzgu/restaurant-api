using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models
{
    [Table("items")]
    public class Item
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Required]
        [Column("name")]
        public required string Name { get; set; }
        
        [Column("description")]
        public string? Description { get; set; }
        
        [Column("price")]
        public decimal Price { get; set; }
        
        [Column("category_id")]
        public int CategoryId { get; set; }
        
        [Column("image_url")]
        public string? ImageUrl { get; set; }

        // Navigation properties
        public Category? Category { get; set; }
        public ICollection<ItemOffer> ItemOffers { get; set; } = new List<ItemOffer>();
        public ICollection<ItemAllergen> ItemAllergens { get; set; } = new List<ItemAllergen>();
        public ICollection<ItemSelectionGroup> ItemSelectionGroups { get; set; } = new List<ItemSelectionGroup>();
    }
}
