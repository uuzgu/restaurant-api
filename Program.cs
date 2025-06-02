using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Models;
using RestaurantApi.Services;
using System.Text.Json.Serialization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.SetIsOriginAllowed(origin => true) // Allow any origin
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.EnableEndpointRouting = true;
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// Add response compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

// Add memory cache
builder.Services.AddMemoryCache();

// Add DbContext
var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") 
    ?? (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production"
        ? "Data Source=/app/App_Data/restaurant.db"
        : "Data Source=App_Data/restaurant.db");
var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
logger.LogInformation("Using connection string: {ConnectionString}", connectionString);

builder.Services.AddDbContext<RestaurantContext>(options =>
{
    options.UseSqlite(connectionString);
    options.EnableSensitiveDataLogging();
    options.EnableDetailedErrors();
});

// Add Services
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<DataMigrationService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS before other middleware
app.UseCors("AllowAll");

// Add routing middleware
app.UseRouting();

// Configure HTTPS redirection
app.UseHttpsRedirection();

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Enable endpoint routing
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapControllerRoute(
        name: "api",
        pattern: "api/{controller}/{action=Index}/{id?}");
});

// Log all registered controllers and their routes
var controllerTypes = app.Services.GetRequiredService<IEnumerable<Type>>()
    .Where(t => t.IsClass && !t.IsAbstract && typeof(ControllerBase).IsAssignableFrom(t));

foreach (var controllerType in controllerTypes)
{
    var routeAttribute = controllerType.GetCustomAttributes(typeof(RouteAttribute), true)
        .FirstOrDefault() as RouteAttribute;
    var route = routeAttribute?.Template ?? "No route template";
    logger.LogInformation("Registered controller: {Controller} with route: {Route}", 
        controllerType.Name, route);
}

// Ensure App_Data directory exists and has proper permissions
var appDataPath = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production"
    ? "/app/App_Data"
    : Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
logger.LogInformation("App_Data path: {AppDataPath}", appDataPath);

if (!Directory.Exists(appDataPath))
{
    logger.LogInformation("Creating App_Data directory");
    Directory.CreateDirectory(appDataPath);
    // Set directory permissions to allow writing
    try
    {
        var directoryInfo = new DirectoryInfo(appDataPath);
        directoryInfo.Attributes &= ~FileAttributes.ReadOnly;
        logger.LogInformation("Successfully set directory permissions");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Could not set directory permissions");
    }
}

// Ensure database is created and migrated
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<RestaurantContext>();
        var scopeLogger = services.GetRequiredService<ILogger<Program>>();
        
        scopeLogger.LogInformation("Starting database initialization");
        
        // First ensure the database exists
        context.Database.EnsureCreated();
        scopeLogger.LogInformation("Database created successfully");
        
        // Then try to apply any pending migrations
        try
        {
            var pendingMigrations = context.Database.GetPendingMigrations().ToList();
            if (pendingMigrations.Any())
            {
                scopeLogger.LogInformation("Applying {Count} pending migrations", pendingMigrations.Count);
                context.Database.Migrate();
                scopeLogger.LogInformation("Migrations applied successfully");
            }
            else
            {
                scopeLogger.LogInformation("No pending migrations");
            }
        }
        catch (Exception ex)
        {
            scopeLogger.LogError(ex, "Could not apply migrations");
            // If migrations fail, we'll continue with the existing database
        }
        
        // Verify database connection and seed data
        try
        {
            var canConnect = context.Database.CanConnect();
            scopeLogger.LogInformation("Database connection test: {Result}", canConnect ? "Success" : "Failed");
            
            if (canConnect)
            {
                // Check if we need to seed data
                var categoryCount = context.Categories.Count();
                var itemCount = context.Items.Count();
                scopeLogger.LogInformation("Current database state - Categories: {CategoryCount}, Items: {ItemCount}", 
                    categoryCount, itemCount);

                if (categoryCount == 0)
                {
                    scopeLogger.LogInformation("Database is empty, but we have existing data in the database file. Please ensure the database file is properly copied to App_Data directory.");
                }
                else
                {
                    scopeLogger.LogInformation("Database already contains data, skipping seeding");
                }

                // Verify data
                var finalCategoryCount = context.Categories.Count();
                var finalItemCount = context.Items.Count();
                scopeLogger.LogInformation("Final database state - Categories: {CategoryCount}, Items: {ItemCount}", 
                    finalCategoryCount, finalItemCount);

                // Log sample data for verification
                var sampleCategories = context.Categories.Take(3).ToList();
                var sampleItems = context.Items.Take(3).ToList();
                scopeLogger.LogInformation("Sample Categories: {Categories}", 
                    string.Join(", ", sampleCategories.Select(c => $"{c.Id}: {c.Name}")));
                scopeLogger.LogInformation("Sample Items: {Items}", 
                    string.Join(", ", sampleItems.Select(i => $"{i.Id}: {i.Name} (Category: {i.CategoryId})")));
            }
            else
            {
                scopeLogger.LogError("Could not connect to the database");
            }
        }
        catch (Exception ex)
        {
            scopeLogger.LogError(ex, "An error occurred during database initialization");
            throw;
        }
    }
    catch (Exception ex)
    {
        var scopeLogger = services.GetRequiredService<ILogger<Program>>();
        scopeLogger.LogError(ex, "An error occurred while initializing the database");
        // Don't throw the exception, allow the application to start even if database initialization fails
    }
}

app.Run();
