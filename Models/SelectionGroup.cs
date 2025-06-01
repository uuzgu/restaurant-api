using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models
{
    public class SelectionGroup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("type")]
        public string Type { get; set; } // "ingredient", "drink", "side", etc.

        [Required]
        [Column("is_required")]
        public bool IsRequired { get; set; }

        [Required]
        [Column("min_select")]
        public int MinSelect { get; set; }

        [Required]
        [Column("max_select")]
        public int MaxSelect { get; set; }

        [Required]
        [Column("threshold")]
        public int Threshold { get; set; } // First N options are free

        [Required]
        [Column("display_order")]
        public int DisplayOrder { get; set; }

        // Navigation properties
        public ICollection<SelectionOption> Options { get; set; }
        public ICollection<ItemSelectionGroup> ItemSelectionGroups { get; set; }
        public ICollection<CategorySelectionGroup> CategorySelectionGroups { get; set; }
    }
} 