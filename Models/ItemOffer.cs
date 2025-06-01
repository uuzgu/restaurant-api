using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models
{
    [Table("item_offers")]
    public class ItemOffer
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        
        [Column("item_id")]
        public int ItemId { get; set; }
        
        [Column("offer_id")]
        public int OfferId { get; set; }

        // Navigation properties
        public Item? Item { get; set; }
        public Offer? Offer { get; set; }
    }
} 