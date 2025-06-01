using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models
{
    public class ItemSelectionGroup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("item_id")]
        public int ItemId { get; set; }

        [Required]
        [Column("selection_group_id")]
        public int SelectionGroupId { get; set; }

        // Navigation properties
        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        [ForeignKey("SelectionGroupId")]
        public SelectionGroup SelectionGroup { get; set; }
    }
} 