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
        var allowedOrigins = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")?.Split(',') 
            ?? new[] { 
                "http://localhost:3000",
                "https://restaurant-ui-utkus-projects-cabada99.vercel.app"
            };
        
        builder.WithOrigins(allowedOrigins)
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

// Add security headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline' 'unsafe-eval'; style-src 'self' 'unsafe-inline';");
    await next();
});

// Use CORS before other middleware
app.UseCors("AllowAll");

// Configure HTTPS redirection
app.UseHttpsRedirection();

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Log all registered controllers and their routes
var logger = app.Services.GetRequiredService<ILogger<Program>>();
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
            }
        }
        catch (Exception ex)
        {
            scopeLogger.LogError(ex, "Error verifying database connection and data");
        }
    }
    catch (Exception ex)
    {
        var scopeLogger = services.GetRequiredService<ILogger<Program>>();
        scopeLogger.LogError(ex, "An error occurred while initializing the database");
    }
}

app.Run();
