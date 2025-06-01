using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models
{
    public class DeliveryAddress
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("postcode_id")]
        public int PostcodeId { get; set; }

        [Required]
        [Column("street")]
        public string Street { get; set; }

        [Column("house")]
        public string? House { get; set; }

        [Column("stairs")]
        public string? Stairs { get; set; }

        [Column("stick")]
        public string? Stick { get; set; }

        [Column("door")]
        public string? Door { get; set; }

        [Column("bell")]
        public string? Bell { get; set; }

        // Navigation property
        [ForeignKey("PostcodeId")]
        public Postcode Postcode { get; set; }
    }
} 