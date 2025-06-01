using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models
{
    public class ItemAllergen
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        [StringLength(1)]
        public string AllergenCode { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }
    }
} 