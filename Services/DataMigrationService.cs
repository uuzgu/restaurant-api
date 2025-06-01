using System.Linq;
using RestaurantApi.Data;
using RestaurantApi.Models;

namespace RestaurantApi.Services
{
    public class DataMigrationService
    {
        private readonly RestaurantContext _context;
        public DataMigrationService(RestaurantContext context)
        {
            _context = context;
        }

        public void MigrateToSelectionGroups()
        {
            // Migrate Item Ingredients
            var items = _context.Items.ToList();
            foreach (var item in items)
            {
                var ingredients = _context.ItemIngredients.Where(ii => ii.ItemId == item.Id).ToList();
                if (ingredients.Any())
                {
                    var group = new SelectionGroup
                    {
                        Name = "Ingredients",
                        Type = "ingredient",
                        IsRequired = false,
                        MinSelect = 0,
                        MaxSelect = ingredients.Count,
                        Threshold = 0,
                        DisplayOrder = 0
                    };
                    _context.SelectionGroups.Add(group);
                    _context.SaveChanges();

                    foreach (var ing in ingredients)
                    {
                        var option = new SelectionOption
                        {
                            Name = ing.Name,
                            Price = ing.ExtraCost,
                            DisplayOrder = 0,
                            SelectionGroupId = group.Id
                        };
                        _context.SelectionOptions.Add(option);
                    }
                    _context.SaveChanges();

                    var link = new ItemSelectionGroup
                    {
                        ItemId = item.Id,
                        SelectionGroupId = group.Id
                    };
                    _context.ItemSelectionGroups.Add(link);
                    _context.SaveChanges();
                }
            }
        }
    }
} 