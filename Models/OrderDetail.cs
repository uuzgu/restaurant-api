using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models;

[Table("order_details")]
public class OrderDetail
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column("order_id")]
    public int OrderId { get; set; }

    [Required]
    [Column("item_id")]
    public int ItemId { get; set; }

    [Required]
    [Column("quantity")]
    public int Quantity { get; set; }

    [Required]
    [Column("price")]
    public decimal Price { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    // Navigation properties
    [ForeignKey("OrderId")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ItemId")]
    public virtual Item Item { get; set; } = null!;

    public virtual ICollection<Selection> Selections { get; set; } = new List<Selection>();
} 