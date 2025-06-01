using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace RestaurantApi.Models
{
    public class CustomerOrderInfo
    {
        [Key]
        [Column("Id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required]
        [Column("first_name")]
        [JsonPropertyName("firstName")]
        public required string FirstName { get; set; }

        [Required]
        [Column("last_name")]
        [JsonPropertyName("lastName")]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Column("email")]
        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [Phone]
        [Column("phone")]
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [Column("postal_code")]
        [JsonPropertyName("postalCode")]
        public string? PostalCode { get; set; }

        [Column("street")]
        [JsonPropertyName("street")]
        public string? Street { get; set; }

        [Column("house")]
        [JsonPropertyName("house")]
        public string? House { get; set; }

        [Column("stairs")]
        [JsonPropertyName("stairs")]
        public string? Stairs { get; set; }

        [Column("stick")]
        [JsonPropertyName("stick")]
        public string? Stick { get; set; }

        [Column("door")]
        [JsonPropertyName("door")]
        public string? Door { get; set; }

        [Column("bell")]
        [JsonPropertyName("bell")]
        public string? Bell { get; set; }

        [Column("comment")]
        [JsonPropertyName("comment")]
        public string? Comment { get; set; }

        [Column("create_date")]
        [JsonPropertyName("createDate")]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    }
} 