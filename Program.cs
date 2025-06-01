using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Models;
using RestaurantApi.Services;
using System.Text.Json.Serialization;

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
        builder.WithOrigins(
                "http://localhost:3000",
                "https://restaurant-ui.vercel.app",
                "https://restaurant-ui-gules.vercel.app"
            )
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Add DbContext
var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") 
    ?? builder.Configuration.GetConnectionString("DefaultConnection");
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
app.UseSwagger();
app.UseSwaggerUI();

// Use CORS before other middleware
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Ensure App_Data directory exists and has proper permissions
var appDataPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
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
                scopeLogger.LogInformation("Found {Count} categories in the database", categoryCount);

                if (categoryCount == 0)
                {
                    scopeLogger.LogInformation("Seeding initial data");
                    // Seed categories
                    context.Categories.AddRange(
                        new Category { Id = -1, Name = "Promotions" },
                        new Category { Id = -2, Name = "Pizza" },
                        new Category { Id = -3, Name = "Bowl" },
                        new Category { Id = -4, Name = "Cheeseburger" },
                        new Category { Id = -5, Name = "Salad" },
                        new Category { Id = -6, Name = "Breakfast" },
                        new Category { Id = -7, Name = "Drinks" },
                        new Category { Id = -8, Name = "Soup" },
                        new Category { Id = -9, Name = "Dessert" }
                    );

                    // Seed items
                    context.Items.AddRange(
                        new Item
                        {
                            Id = -1,
                            Name = "Special Combo",
                            Description = "Get a pizza and drink at a special price!",
                            Price = 15.99m,
                            CategoryId = -1,
                            ImageUrl = "/images/categories/promotionsCategory.png"
                        },
                        new Item
                        {
                            Id = -2,
                            Name = "Family Bundle",
                            Description = "Perfect for the whole family - 2 pizzas and 4 drinks",
                            Price = 29.99m,
                            CategoryId = -1,
                            ImageUrl = "/images/categories/promotionsCategory.png"
                        },
                        new Item { Id = 1, CategoryId = -7, Name = "Cola", Description = "Classic carbonated soft drink", Price = 4 },
                        new Item { Id = 2, CategoryId = -5, Name = "Salad", Description = "Salad with fresh vegetables and cheese", ImageUrl = "https://restaurant-images33.s3.eu-north-1.amazonaws.com/salad.jpg", Price = 8 }
                    );

                    context.SaveChanges();
                    scopeLogger.LogInformation("Initial data seeded successfully");
                }
            }
        }
        catch (Exception ex)
        {
            scopeLogger.LogError(ex, "Database connection test or seeding failed");
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
