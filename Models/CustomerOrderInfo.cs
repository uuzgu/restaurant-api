using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models
{
    [Table("customerOrder_info")]
    public class CustomerOrderInfo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Required]
        [Column("name")]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Column("email")]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [Column("phone")]
        public string Phone { get; set; } = string.Empty;
        
        [Required]
        [Column("address")]
        public string Address { get; set; } = string.Empty;
        
        // Additional fields referenced in controllers
        [Column("firstName")]
        public string FirstName { get; set; } = string.Empty;
        [Column("lastName")]
        public string LastName { get; set; } = string.Empty;
        [Column("comment")]
        public string Comment { get; set; } = string.Empty;
        [Column("postalCode")]
        public string PostalCode { get; set; } = string.Empty;
        [Column("street")]
        public string Street { get; set; } = string.Empty;
        [Column("house")]
        public string House { get; set; } = string.Empty;
        [Column("stairs")]
        public string Stairs { get; set; } = string.Empty;
        [Column("stick")]
        public string Stick { get; set; } = string.Empty;
        [Column("door")]
        public string Door { get; set; } = string.Empty;
        [Column("bell")]
        public string Bell { get; set; } = string.Empty;
        
        [Required]
        [Column("createDate")]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
} 