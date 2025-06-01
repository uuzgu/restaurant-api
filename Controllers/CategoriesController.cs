using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Models;

namespace RestaurantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly RestaurantContext _context;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(RestaurantContext context, ILogger<CategoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            try
            {
                _logger.LogInformation("Fetching all categories");
                var categories = await _context.Categories
                    .Select(c => new { id = c.Id, name = c.Name })
                    .ToListAsync();
                
                // Define the desired order of categories
                var orderedCategories = categories.OrderBy(c => c.id switch
                {
                    1 => 1,  // Pizza
                    2 => 2,  // Bowl
                    3 => 3,  // Cheeseburger
                    4 => 4,  // Salad
                    5 => 5,  // Breakfast
                    6 => 6,  // Drinks
                    7 => 7,  // Soup
                    8 => 8,  // Dessert
                    0 => 9,  // Promotions
                    _ => 10  // Any other categories
                });
                
                _logger.LogInformation("Found {Count} categories", categories.Count);
                return Ok(orderedCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching categories");
                return StatusCode(500, "An error occurred while fetching categories");
            }
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            try
            {
                _logger.LogInformation("Fetching category with ID: {Id}", id);
                var category = await _context.Categories.FindAsync(id);

                if (category == null)
                {
                    _logger.LogWarning("Category with ID {Id} not found", id);
                    return NotFound();
                }

                _logger.LogInformation("Found category: {Category}", category.Name);
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching category with ID: {Id}", id);
                return StatusCode(500, "An error occurred while fetching the category");
            }
        }
    }
} 