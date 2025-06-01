using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestaurantApi.Models
{
    public class CustomerOrderInfo
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required]
        [JsonPropertyName("firstName")]
        public required string FirstName { get; set; }

        [Required]
        [JsonPropertyName("lastName")]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [Phone]
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("address")]
        public Address? Address { get; set; }

        [JsonPropertyName("comment")]
        public string? Comment { get; set; }

        [JsonPropertyName("createDate")]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    }
} 