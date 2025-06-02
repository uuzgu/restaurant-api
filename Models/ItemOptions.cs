using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Models
{
    public class ItemOptions
    {
        // public List<ItemIngredient> Ingredients { get; set; } = new List<ItemIngredient>();
        public List<ItemOfferWithDetails> ItemOffers { get; set; } = new List<ItemOfferWithDetails>();
        public List<SelectionGroupWithOptions> SelectionGroups { get; set; } = new List<SelectionGroupWithOptions>();
        public List<SelectionGroupWithOptions> CategorySelectionGroups { get; set; } = new List<SelectionGroupWithOptions>();
    }

    public class ItemOfferWithDetails
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int OfferId { get; set; }
        public Offer Offer { get; set; } = null!;
    }

    public class SelectionGroupWithOptions
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Type { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public int MinSelect { get; set; }
        public int MaxSelect { get; set; }
        public int Threshold { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsOptional { get; set; }
        public List<SelectionOption> Options { get; set; } = new List<SelectionOption>();
    }
} 