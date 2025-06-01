using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models
{
    [Table("item_ingredients")]
    public class ItemIngredient
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("item_id")]
        public int ItemId { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("is_mandatory")]
        public bool IsMandatory { get; set; }

        [Column("can_exclude")]
        public bool CanExclude { get; set; }

        [Column("extra_cost")]
        public decimal ExtraCost { get; set; }

        // Navigation Property
        public Item? Item { get; set; }
    }
}
