using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Models;
using System.Linq;

namespace RestaurantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly RestaurantContext _context;

        public ItemsController(RestaurantContext context)
        {
            _context = context;
        }

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            try
            {
                var items = await _context.Items
                    .Include(i => i.Category)
                    .Include(i => i.ItemAllergens)
                    .ToListAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            try
            {
                var item = await _context.Items
                    .Include(i => i.Category)
                    .Include(i => i.ItemAllergens)
                    .FirstOrDefaultAsync(i => i.Id == id);
                    
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/items/{id}/options
        [HttpGet("{id}/options")]
        public async Task<ActionResult<ItemOptions>> GetItemOptions(int id)
        {
            try
            {
                // Check if the item exists and include all related data
                var item = await _context.Items
                    .Include(i => i.ItemOffers)
                        .ThenInclude(io => io.Offer)
                    .Include(i => i.ItemAllergens)
                    .Include(i => i.ItemSelectionGroups)
                        .ThenInclude(isg => isg.SelectionGroup)
                            .ThenInclude(sg => sg.Options)
                    .Include(i => i.Category)
                        .ThenInclude(c => c.CategorySelectionGroups)
                            .ThenInclude(csg => csg.SelectionGroup)
                                .ThenInclude(sg => sg.Options)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (item == null)
                {
                    return NotFound($"Item with ID {id} not found");
                }

                // Create a new ItemOptions object with clean data
                var itemOptions = new ItemOptions
                {
                    ItemOffers = item.ItemOffers.Select(io => new ItemOfferWithDetails
                    {
                        Id = io.Id,
                        ItemId = io.ItemId,
                        OfferId = io.OfferId,
                        Offer = new Offer
                        {
                            Id = io.Offer.Id,
                            Name = io.Offer.Name,
                            Description = io.Offer.Description,
                            DiscountPercentage = io.Offer.DiscountPercentage,
                            StartDate = io.Offer.StartDate,
                            EndDate = io.Offer.EndDate,
                            IsActive = io.Offer.IsActive
                        }
                    }).ToList(),
                    SelectionGroups = item.ItemSelectionGroups.Select(isg => new SelectionGroupWithOptions
                    {
                        Id = isg.SelectionGroup.Id,
                        Name = isg.SelectionGroup.Name,
                        Type = isg.SelectionGroup.Type,
                        IsRequired = isg.SelectionGroup.IsRequired,
                        MinSelect = isg.SelectionGroup.MinSelect,
                        MaxSelect = isg.SelectionGroup.MaxSelect,
                        Threshold = isg.SelectionGroup.Threshold,
                        DisplayOrder = isg.SelectionGroup.DisplayOrder,
                        Options = isg.SelectionGroup.Options.Select(o => new SelectionOption
                        {
                            Id = o.Id,
                            Name = o.Name,
                            Price = o.Price,
                            DisplayOrder = o.DisplayOrder,
                            SelectionGroupId = o.SelectionGroupId
                        }).ToList()
                    }).ToList(),
                    CategorySelectionGroups = item.Category.CategorySelectionGroups.Select(csg => new SelectionGroupWithOptions
                    {
                        Id = csg.SelectionGroup.Id,
                        Name = csg.SelectionGroup.Name,
                        Type = csg.SelectionGroup.Type,
                        IsRequired = csg.SelectionGroup.IsRequired,
                        MinSelect = csg.SelectionGroup.MinSelect,
                        MaxSelect = csg.SelectionGroup.MaxSelect,
                        Threshold = csg.SelectionGroup.Threshold,
                        DisplayOrder = csg.SelectionGroup.DisplayOrder,
                        Options = csg.SelectionGroup.Options.Select(o => new SelectionOption
                        {
                            Id = o.Id,
                            Name = o.Name,
                            Price = o.Price,
                            DisplayOrder = o.DisplayOrder,
                            SelectionGroupId = o.SelectionGroupId
                        }).ToList()
                    }).ToList()
                };

                return Ok(itemOptions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/items/types
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<CategoryWithItemCount>>> GetItemsGroupedByType()
        {
            try
            {
                var itemsGroupedByType = await _context.Items
                    .Include(i => i.Category)
                    .GroupBy(item => item.Category.Name)
                    .Select(group => new CategoryWithItemCount
                    {
                        Category = group.Key,
                        ItemCount = group.Count()
                    })
                    .ToListAsync();

                return Ok(itemsGroupedByType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    // Helper class to return category and item count
    public class CategoryWithItemCount
    {
        public string? Category { get; set; }
        public int ItemCount { get; set; }
    }
}
