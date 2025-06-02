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
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = string.Empty;
        
        [Column(TypeName = "nvarchar(500)")]
        public string? Description { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        [Column("category_id")]
        public int CategoryId { get; set; }
        
        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [Column("is_available")]
        public bool IsAvailable { get; set; } = true;

        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<ItemAllergen> ItemAllergens { get; set; } = new List<ItemAllergen>();
        public virtual ICollection<ItemSelectionGroup> ItemSelectionGroups { get; set; } = new List<ItemSelectionGroup>();
        
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public virtual ICollection<ItemOffer> ItemOffers { get; set; } = new List<ItemOffer>();

        public virtual ICollection<SelectionOption> SelectionOptions { get; set; } = new List<SelectionOption>();
    }
}
