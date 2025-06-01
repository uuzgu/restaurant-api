using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models
{
    public class Postcode
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("code")]
        public string Code { get; set; }

        [Column("district")]
        public string District { get; set; }

        // Navigation property
        public ICollection<DeliveryAddress> DeliveryAddresses { get; set; }
    }
} 