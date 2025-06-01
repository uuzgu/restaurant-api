using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Services;

namespace RestaurantApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MigrationController : ControllerBase
    {
        private readonly DataMigrationService _migrationService;
        public MigrationController(DataMigrationService migrationService)
        {
            _migrationService = migrationService;
        }

        [HttpPost("migrate-selection-groups")]
        public IActionResult MigrateSelectionGroups()
        {
            _migrationService.MigrateToSelectionGroups();
            return Ok("Migration completed!");
        }
    }
} 