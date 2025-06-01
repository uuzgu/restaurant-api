using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostcodeController : ControllerBase
    {
        private readonly RestaurantContext _context;

        public PostcodeController(RestaurantContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Postcode>>> GetPostcodes()
        {
            return await _context.Postcodes
                .OrderBy(p => p.Code)
                .ToListAsync();
        }

        [HttpGet("{postcodeId}/addresses")]
        public async Task<ActionResult<IEnumerable<DeliveryAddress>>> GetAddressesByPostcode(int postcodeId)
        {
            var postcode = await _context.Postcodes.FindAsync(postcodeId);
            if (postcode == null)
            {
                return NotFound();
            }
            var addresses = await _context.DeliveryAddresses
                .Where(a => a.PostcodeId == postcodeId)
                .ToListAsync();
            return addresses;
        }
    }
} 