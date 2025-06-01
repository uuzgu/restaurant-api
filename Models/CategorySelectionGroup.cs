using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models
{
    public class CategorySelectionGroup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("category_id")]
        public int CategoryId { get; set; }

        [Required]
        [Column("selection_group_id")]
        public int SelectionGroupId { get; set; }

        // Navigation properties
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [ForeignKey("SelectionGroupId")]
        public SelectionGroup SelectionGroup { get; set; }
    }
} 