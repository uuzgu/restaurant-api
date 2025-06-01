using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace RestaurantApi.Models
{
    [Table("categories")]
    public class Category
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("name")]
        public string Name { get; set; } = string.Empty;
        
        // Navigation property
        public ICollection<Item> Items { get; set; } = new List<Item>();
        public ICollection<CategorySelectionGroup> CategorySelectionGroups { get; set; }
    }
} 