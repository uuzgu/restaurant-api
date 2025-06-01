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

        [JsonPropertyName("postalCode")]
        public string? PostalCode { get; set; }

        [JsonPropertyName("street")]
        public string? Street { get; set; }

        [JsonPropertyName("house")]
        public string? House { get; set; }

        [JsonPropertyName("stairs")]
        public string? Stairs { get; set; }

        [JsonPropertyName("stick")]
        public string? Stick { get; set; }

        [JsonPropertyName("door")]
        public string? Door { get; set; }

        [JsonPropertyName("bell")]
        public string? Bell { get; set; }

        [JsonPropertyName("comment")]
        public string? Comment { get; set; }

        [JsonPropertyName("createDate")]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    }
} 