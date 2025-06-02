using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models
{
    [Table("selection_options")]
    public class SelectionOption
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = string.Empty;
        
        [Column("price")]
        public decimal Price { get; set; }
        
        [Column("display_order")]
        public int DisplayOrder { get; set; }
        
        [Column("selection_group_id")]
        public int SelectionGroupId { get; set; }

        // Navigation properties
        [ForeignKey("SelectionGroupId")]
        public virtual SelectionGroup SelectionGroup { get; set; } = null!;
        public virtual ICollection<Selection> Selections { get; set; } = new List<Selection>();
    }
} 