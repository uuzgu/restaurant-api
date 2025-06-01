using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models
{
    public class SelectionOption
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("price")]
        public decimal Price { get; set; }

        [Required]
        [Column("display_order")]
        public int DisplayOrder { get; set; }

        [Required]
        [Column("selection_group_id")]
        public int SelectionGroupId { get; set; }

        // Navigation properties
        [ForeignKey("SelectionGroupId")]
        public SelectionGroup SelectionGroup { get; set; }
    }
} 