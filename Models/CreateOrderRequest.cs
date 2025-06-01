using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestaurantApi.Models
{
    public class CreateOrderRequest
    {
        [Required]
        [JsonPropertyName("items")]
        public required List<OrderItemRequest> Items { get; set; }

        [Required]
        [JsonPropertyName("customerInfo")]
        public required CustomerInfoRequest CustomerInfo { get; set; }

        [Required]
        [JsonPropertyName("orderMethod")]
        public required string OrderMethod { get; set; }

        [Required]
        [JsonPropertyName("paymentMethod")]
        public required string PaymentMethod { get; set; }

        [Required]
        [JsonPropertyName("status")]
        public required string Status { get; set; }

        [JsonPropertyName("specialNotes")]
        public string? SpecialNotes { get; set; }

        [Required]
        [JsonPropertyName("totalAmount")]
        public required decimal TotalAmount { get; set; }
    }

    public class OrderItemRequest
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("selectedOptions")]
        public List<int> SelectedOptions { get; set; } = new List<int>();

        [JsonPropertyName("selectedItems")]
        public List<SelectedItem>? SelectedItems { get; set; }
    }

    public class CustomerInfoRequest
    {
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

        [Required]
        [Phone]
        [JsonPropertyName("phone")]
        public required string Phone { get; set; }

        [JsonPropertyName("comment")]
        public string? Comment { get; set; }

        [JsonPropertyName("address")]
        public AddressRequest? Address { get; set; }
    }

    public class AddressRequest
    {
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
    }
} 