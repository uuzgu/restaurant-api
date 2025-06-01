using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly RestaurantContext _context;

        public CouponsController(RestaurantContext context)
        {
            _context = context;
        }

        // GET: api/Coupons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Coupon>>> GetCoupons()
        {
            return await _context.Coupons.ToListAsync();
        }

        // GET: api/Coupons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Coupon>> GetCoupon(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id);

            if (coupon == null)
            {
                return NotFound();
            }

            return coupon;
        }

        // POST: api/Coupons
        [HttpPost]
        public async Task<ActionResult<Coupon>> CreateCoupon(Coupon coupon)
        {
            if (string.IsNullOrEmpty(coupon.Code))
            {
                return BadRequest("Coupon code is required");
            }

            // Check if coupon code already exists
            if (await _context.Coupons.AnyAsync(c => c.Code == coupon.Code))
            {
                return BadRequest("Coupon code already exists");
            }

            // Validate dates for periodic coupons
            if (coupon.IsPeriodic == 1)
            {
                if (string.IsNullOrEmpty(coupon.StartDate) || string.IsNullOrEmpty(coupon.EndDate))
                {
                    return BadRequest("Start date and end date are required for periodic coupons");
                }

                if (!DateTime.TryParse(coupon.StartDate, out DateTime startDate) || 
                    !DateTime.TryParse(coupon.EndDate, out DateTime endDate))
                {
                    return BadRequest("Invalid date format");
                }

                if (startDate >= endDate)
                {
                    return BadRequest("Start date must be before end date");
                }
            }

            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCoupon), new { id = coupon.Id }, coupon);
        }

        // PUT: api/Coupons/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCoupon(int id, Coupon coupon)
        {
            if (id != coupon.Id)
            {
                return BadRequest();
            }

            var existingCoupon = await _context.Coupons.FindAsync(id);
            if (existingCoupon == null)
            {
                return NotFound();
            }

            // Check if code is being changed and if it already exists
            if (existingCoupon.Code != coupon.Code)
            {
                if (await _context.Coupons.AnyAsync(c => c.Code == coupon.Code))
                {
                    return BadRequest("Coupon code already exists");
                }
            }

            // Validate dates for periodic coupons
            if (coupon.IsPeriodic == 1)
            {
                if (string.IsNullOrEmpty(coupon.StartDate) || string.IsNullOrEmpty(coupon.EndDate))
                {
                    return BadRequest("Start date and end date are required for periodic coupons");
                }

                if (!DateTime.TryParse(coupon.StartDate, out DateTime startDate) || 
                    !DateTime.TryParse(coupon.EndDate, out DateTime endDate))
                {
                    return BadRequest("Invalid date format");
                }

                if (startDate >= endDate)
                {
                    return BadRequest("Start date must be before end date");
                }
            }

            _context.Entry(existingCoupon).CurrentValues.SetValues(coupon);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CouponExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Coupons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }

            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Coupons/validate-schedule
        [HttpPost("validate-schedule")]
        public async Task<IActionResult> ValidateCouponSchedule([FromBody] CouponValidationRequest request)
        {
            if (string.IsNullOrEmpty(request.Code))
            {
                return BadRequest("Coupon code is required");
            }

            var coupon = await _context.Coupons
                .Include(c => c.Schedule)
                .FirstOrDefaultAsync(c => c.Code == request.Code);

            if (coupon == null)
            {
                return BadRequest("Coupon not found");
            }

            if (coupon.Schedule == null)
            {
                return BadRequest("Coupon schedule not found");
            }

            // 1. Check schedule validity
            var currentDay = DateTime.Now.DayOfWeek;
            var currentTime = DateTime.Now.TimeOfDay;
            bool isDayValid = currentDay switch
            {
                DayOfWeek.Monday => coupon.Schedule.Monday == 1,
                DayOfWeek.Tuesday => coupon.Schedule.Tuesday == 1,
                DayOfWeek.Wednesday => coupon.Schedule.Wednesday == 1,
                DayOfWeek.Thursday => coupon.Schedule.Thursday == 1,
                DayOfWeek.Friday => coupon.Schedule.Friday == 1,
                DayOfWeek.Saturday => coupon.Schedule.Saturday == 1,
                DayOfWeek.Sunday => coupon.Schedule.Sunday == 1,
                _ => false
            };
            if (!isDayValid)
            {
                return BadRequest("Coupon is not valid for today");
            }

            var beginTime = TimeSpan.Parse(coupon.Schedule.BeginTime);
            var endTime = TimeSpan.Parse(coupon.Schedule.EndTime);
            if (currentTime < beginTime || currentTime > endTime)
            {
                return BadRequest($"Coupon is only valid between {beginTime:hh\\:mm} and {endTime:hh\\:mm}");
            }

            // 2. Check general validity
            // (a) One-time coupon: check if used
            if (coupon.IsPeriodic == 0)
            {
                var isUsed = await _context.CouponHistory.AnyAsync(ch => ch.CouponId == coupon.Id);
                if (isUsed)
                {
                    return BadRequest("Coupon is used");
                }
            }
            // (b) Periodic: check date range
            if (coupon.IsPeriodic == 1)
            {
                if (string.IsNullOrEmpty(coupon.StartDate) || string.IsNullOrEmpty(coupon.EndDate))
                {
                    return BadRequest("Start date and end date are required for periodic coupons");
                }
                if (!DateTime.TryParse(coupon.StartDate, out DateTime startDate) ||
                    !DateTime.TryParse(coupon.EndDate, out DateTime endDate))
                {
                    return BadRequest("Invalid coupon date format");
                }
                var currentDate = DateTime.Now;
                if (currentDate < startDate || currentDate > endDate)
                {
                    return BadRequest("Coupon is not valid for current date");
                }
            }
            // (c) One-time: check email
            if (coupon.IsPeriodic == 0 && !string.IsNullOrEmpty(coupon.Email))
            {
                if (string.IsNullOrEmpty(request.Email))
                {
                    return BadRequest("Email is required for this coupon");
                }
                if (coupon.Email.ToLower() != request.Email.ToLower())
                {
                    return BadRequest("Coupon is not valid for this email");
                }
            }

            // 3. Success
            return Ok(new
            {
                isValid = true,
                coupon.Id,
                coupon.Code,
                coupon.Type,
                coupon.IsPeriodic,
                coupon.StartDate,
                coupon.EndDate,
                coupon.Email,
                coupon.IsUsed,
                coupon.CreatedAt,
                coupon.DiscountRatio,
                schedule = new
                {
                    beginTime = coupon.Schedule.BeginTime,
                    endTime = coupon.Schedule.EndTime,
                    validDays = new
                    {
                        monday = coupon.Schedule.Monday == 1,
                        tuesday = coupon.Schedule.Tuesday == 1,
                        wednesday = coupon.Schedule.Wednesday == 1,
                        thursday = coupon.Schedule.Thursday == 1,
                        friday = coupon.Schedule.Friday == 1,
                        saturday = coupon.Schedule.Saturday == 1,
                        sunday = coupon.Schedule.Sunday == 1
                    }
                }
            });
        }

        // POST: api/Coupons/use
        [HttpPost("use")]
        public async Task<IActionResult> UseCoupon([FromBody] CouponUseRequest request)
        {
            if (string.IsNullOrEmpty(request.Code))
            {
                return BadRequest("Coupon code is required");
            }

            var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Code == request.Code);

            if (coupon == null)
            {
                return NotFound("Coupon not found");
            }

            // For one-time coupons, check if already used
            if (coupon.IsPeriodic == 0)
            {
                var isUsed = await _context.CouponHistory
                    .AnyAsync(ch => ch.CouponId == coupon.Id);

                if (isUsed)
                {
                    return BadRequest("Coupon has already been used");
                }

                // Record the usage
                var history = new CouponHistory
                {
                    CouponId = coupon.Id,
                    Email = request.Email
                };

                _context.CouponHistory.Add(history);
                await _context.SaveChangesAsync();
            }

            return Ok(coupon);
        }

        // GET: api/Coupons/5/history
        [HttpGet("{id}/history")]
        public async Task<ActionResult<IEnumerable<CouponHistory>>> GetCouponHistory(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }

            var history = await _context.CouponHistory
                .Where(ch => ch.CouponId == id)
                .ToListAsync();

            return history;
        }

        private bool CouponExists(int id)
        {
            return _context.Coupons.Any(e => e.Id == id);
        }
    }

    public class CouponValidationRequest
    {
        public string Code { get; set; }
        public string Email { get; set; }
    }

    public class CouponUseRequest
    {
        public string Code { get; set; }
        public string Email { get; set; }
    }
} 