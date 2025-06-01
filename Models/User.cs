using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public string? PostalCode { get; set; }

        public string? Address { get; set; }

        public string? House { get; set; }

        public string? Stairs { get; set; }

        public string? Door { get; set; }

        public string? Bell { get; set; }

        public string? Comment { get; set; }

    }
}
