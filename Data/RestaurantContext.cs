using Microsoft.EntityFrameworkCore;
using RestaurantApi.Models;

namespace RestaurantApi.Data
{
    public class RestaurantContext : DbContext
    {
        public RestaurantContext(DbContextOptions<RestaurantContext> options)
            : base(options)
        { }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemIngredient> ItemIngredients { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<ItemOffer> ItemOffers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<CustomerOrderInfo> CustomerOrderInfos { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<ItemAllergen> ItemAllergens { get; set; }
        public DbSet<SelectionGroup> SelectionGroups { get; set; }
        public DbSet<SelectionOption> SelectionOptions { get; set; }
        public DbSet<ItemSelectionGroup> ItemSelectionGroups { get; set; }
        public DbSet<CategorySelectionGroup> CategorySelectionGroups { get; set; }
        public DbSet<Postcode> Postcodes { get; set; }
        public DbSet<DeliveryAddress> DeliveryAddresses { get; set; }
        public DbSet<PostcodeMinimumOrder> PostcodeMinimumOrders { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CouponHistory> CouponHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure CouponHistory entity
            modelBuilder.Entity<CouponHistory>()
                .HasOne(ch => ch.Coupon)
                .WithMany()
                .HasForeignKey(ch => ch.CouponId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");
                entity.HasKey(e => e.Id).HasName("id");
                entity.Property(e => e.OrderNumber).HasColumnName("order_number").IsRequired();
                entity.Property(e => e.Status).HasColumnName("status").IsRequired();
                entity.Property(e => e.Total).HasColumnName("total").IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").IsRequired();
                entity.Property(e => e.PaymentMethod).HasColumnName("payment_method").IsRequired();
                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            // Configure OrderDetails entity
            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.ToTable("order_details");
                entity.HasKey(e => e.Id).HasName("id");
                entity.Property(e => e.OrderId).HasColumnName("order_id");
                entity.Property(e => e.ItemDetails).HasColumnName("item_details").IsRequired();
                entity.HasOne(e => e.Order)
                    .WithOne(o => o.OrderDetails)
                    .HasForeignKey<OrderDetails>(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
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

            // Seed Items
            modelBuilder.Entity<Item>().HasData(
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
                new Item { Id = 2, CategoryId = -5, Name = "Salad", Description = "Salad with fresh vegetables and cheese", ImageUrl = "https://restaurant-images33.s3.eu-north-1.amazonaws.com/salad.jpg", Price = 8 },
                new Item { Id = 3, CategoryId = -2, Name = "Vegan Pizza", Description = "A delightful plant-based pizza with a variety of fresh toppings", ImageUrl = "https://restaurant-images33.s3.eu-north-1.amazonaws.com/veganpizza.jpg", Price = 12 },
                new Item { Id = 4, CategoryId = -3, Name = "Bowl", Description = "Filled with fresh ingredients, perfect for a healthy and satisfying meal.", ImageUrl = "https://restaurant-images33.s3.eu-north-1.amazonaws.com/bowll.jpg", Price = 10 },
                new Item { Id = 5, CategoryId = -7, Name = "Water", Description = "Pure, crisp, and refreshing still water", Price = 1 },
                new Item { Id = 6, CategoryId = -7, Name = "Ayran", Description = "Traditional Turkish yogurt-based drink, cool and refreshing with a creamy texture", Price = 2 },
                new Item { Id = 7, CategoryId = -7, Name = "Sprite", Description = "A light, refreshing lemon-lime soda with a crisp, bubbly taste to quench your thirst", Price = 3 },
                new Item { Id = 8, CategoryId = -2, Name = "Margarita Pizza", Description = "Classic pizza with rich tomato sauce, fresh mozzarella, and basil", ImageUrl = "https://restaurant-images33.s3.eu-north-1.amazonaws.com/margarita.jpg", Price = 9 },
                new Item { Id = 9, CategoryId = -7, Name = "Soda", Description = "A premium Turkish soda, light and refreshing, perfect for pairing with your meal.", Price = 2 },
                new Item { Id = 10, CategoryId = -4, Name = "Baconburger", Description = "A delicious burger made with a juicy beef patty, crispy bacon, melted cheese, and fresh onions, all nestled in a soft bun", ImageUrl = "https://restaurant-images33.s3.eu-north-1.amazonaws.com/baconburger.jpg", Price = 6 },
                new Item { Id = 11, CategoryId = -4, Name = "Cheeseburger", Description = "Classic American cheeseburger", ImageUrl = "https://restaurant-images33.s3.eu-north-1.amazonaws.com/cheeseburger.jpg", Price = 7 },
                new Item { Id = 12, CategoryId = -5, Name = "Chicken Salad", Description = "A healthy salad with grilled chicken, fresh veggies, and dressing", ImageUrl = "https://restaurant-images33.s3.eu-north-1.amazonaws.com/chickensalad.jpg", Price = 9 },
                new Item { Id = 13, CategoryId = -2, Name = "Chicken Pizza", Description = "A savory pizza topped with grilled chicken and fresh vegetables", ImageUrl = "https://restaurant-images33.s3.eu-north-1.amazonaws.com/chickenpizza.jpg", Price = 10 },
                new Item { Id = 14, CategoryId = -2, Name = "Sausage Pizza", Description = "A flavorful pizza with spicy sausage and cheese", ImageUrl = "https://restaurant-images33.s3.eu-north-1.amazonaws.com/sausagepizza.jpg", Price = 10 },
                new Item { Id = 15, CategoryId = -6, Name = "Bacon Breakfast Menu", Description = "A delicious breakfast with bacon, eggs, and fresh vegetables", ImageUrl = "https://restaurant-images33.s3.eu-north-1.amazonaws.com/baconbreakfast.jpg", Price = 12 },
                new Item { Id = 16, CategoryId = -8, Name = "Thai Coconut Chicken Soup", Description = "Spicy, sweet, and fragrant with lemongrass, lime, and tender chicken", ImageUrl = "https://restaurant-images33.s3.eu-north-1.amazonaws.com/thaisoup.jpg", Price = 7 },
                new Item { Id = 17, CategoryId = -8, Name = "Creamy Wild Mushroom Soup", Description = "Rich, earthy, and velvety with sautéed mushrooms and a hint of garlic", ImageUrl = "https://restaurant-images33.s3.eu-north-1.amazonaws.com/mushroomsoup.jpg", Price = 6 },
                new Item { Id = 18, CategoryId = -8, Name = "Roasted Tomato Basil Soup", Description = "Smooth, tangy, and topped with a swirl of cream and fresh basil", ImageUrl = "https://restaurant-images33.s3.eu-north-1.amazonaws.com/tomatosoup.jpg", Price = 6 },
                new Item { Id = 19, CategoryId = -9, Name = "Molten Chocolate Lava Cake", Description = "Warm, gooey center served with a scoop of vanilla ice cream", ImageUrl = "https://restaurant-images33.s3.eu-north-1.amazonaws.com/moltendessert.jpg", Price = 5 },
                new Item { Id = 20, CategoryId = -9, Name = "Lemon Meringue Tart", Description = "Zesty lemon curd topped with pillowy toasted meringue in a buttery crust", ImageUrl = "https://restaurant-images33.s3.eu-north-1.amazonaws.com/lemontartdessert.jpg", Price = 5 },
                new Item { Id = 21, CategoryId = -9, Name = "Strawberry Shortcake Parfait", Description = "Layers of fluffy cake, whipped cream, and fresh strawberries", ImageUrl = "https://restaurant-images33.s3.eu-north-1.amazonaws.com/strawberrydessert.jpg", Price = 6 }
            );

            // Configure Item entity
            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("items");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnName("name").IsRequired();
                entity.Property(e => e.Price).HasColumnName("price").IsRequired();
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.ImageUrl).HasColumnName("image_url");
                entity.Property(e => e.CategoryId).HasColumnName("category_id").IsRequired();
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Items)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(e => e.ItemAllergens)
                    .WithOne(e => e.Item)
                    .HasForeignKey(e => e.ItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");
                entity.HasKey(e => e.Id).HasName("id");
                entity.Property(e => e.Name).HasColumnName("name").IsRequired();
            });

            // Configure Offer entity
            modelBuilder.Entity<Offer>(entity =>
            {
                entity.ToTable("offers");
                entity.HasKey(e => e.Id).HasName("Id");
                entity.Property(e => e.Name).HasColumnName("name").IsRequired();
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.DiscountPercentage).HasColumnName("discount_percentage").IsRequired();
                entity.Property(e => e.StartDate).HasColumnName("start_date").IsRequired();
                entity.Property(e => e.EndDate).HasColumnName("end_date").IsRequired();
                entity.Property(e => e.IsActive).HasColumnName("is_active").IsRequired();
            });

            // Configure ItemOffer entity
            modelBuilder.Entity<ItemOffer>(entity =>
            {
                entity.ToTable("item_offers");
                entity.HasKey(e => e.Id).HasName("Id");
                entity.Property(e => e.ItemId).HasColumnName("item_id");
                entity.Property(e => e.OfferId).HasColumnName("offer_id");
                entity.HasOne(e => e.Item)
                    .WithMany(i => i.ItemOffers)
                    .HasForeignKey(e => e.ItemId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Offer)
                    .WithMany(o => o.ItemOffers)
                    .HasForeignKey(e => e.OfferId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id).HasName("Id");
                entity.Property(e => e.Email).HasColumnName("Email").IsRequired();
                entity.Property(e => e.Password).HasColumnName("Password").IsRequired();
                entity.Property(e => e.FirstName).HasColumnName("FirstName").IsRequired();
                entity.Property(e => e.LastName).HasColumnName("LastName").IsRequired();
                entity.Property(e => e.Phone).HasColumnName("Phone");
                entity.Property(e => e.Address).HasColumnName("Address");
                entity.Property(e => e.PostalCode).HasColumnName("PostalCode");
                entity.Property(e => e.House).HasColumnName("House");
                entity.Property(e => e.Door).HasColumnName("Door");
                entity.Property(e => e.Stairs).HasColumnName("Stairs");
                entity.Property(e => e.Bell).HasColumnName("Bell");
                entity.Property(e => e.Comment).HasColumnName("Comment");
            });

            // Configure CustomerOrderInfo entity
            modelBuilder.Entity<CustomerOrderInfo>(entity =>
            {
                entity.ToTable("customerOrder_info");
                entity.HasKey(e => e.Id).HasName("id");
                entity.Property(e => e.FirstName).HasColumnName("first_name").IsRequired();
                entity.Property(e => e.LastName).HasColumnName("last_name").IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").IsRequired();
                entity.Property(e => e.Phone).HasColumnName("phone");
                entity.Property(e => e.PostalCode).HasColumnName("postal_code");
                entity.Property(e => e.Street).HasColumnName("street");
                entity.Property(e => e.House).HasColumnName("house");
                entity.Property(e => e.Stairs).HasColumnName("stairs");
                entity.Property(e => e.Stick).HasColumnName("stick");
                entity.Property(e => e.Door).HasColumnName("door");
                entity.Property(e => e.Bell).HasColumnName("bell");
                entity.Property(e => e.Comment).HasColumnName("comment");
                entity.Property(e => e.CreateDate).HasColumnName("create_date");
            });

            // Seed Offers
            modelBuilder.Entity<Offer>().HasData(
                new Offer 
                { 
                    Id = 1, 
                    Name = "Happy Hour", 
                    Description = "20% off on all pizzas",
                    DiscountPercentage = 20.0m,
                    StartDate = "2025-04-21 14:32:26.987483",
                    EndDate = "2025-05-21 14:32:26.987536",
                    IsActive = true
                },
                new Offer 
                { 
                    Id = 2, 
                    Name = "Student Discount", 
                    Description = "15% off with student ID",
                    DiscountPercentage = 15.0m,
                    StartDate = "2025-04-21 14:32:26.987655",
                    EndDate = "2025-07-20 14:32:26.987655",
                    IsActive = true
                }
            );

            // Seed Item Ingredients
            modelBuilder.Entity<ItemIngredient>().HasData(
                new ItemIngredient { Id = 1, ItemId = 3, Name = "Dough" },
                new ItemIngredient { Id = 2, ItemId = 3, Name = "Tomato Sauce" },
                new ItemIngredient { Id = 3, ItemId = 3, Name = "Mozzarella" },
                new ItemIngredient { Id = 4, ItemId = 3, Name = "Basil" }
            );

            // Seed Item Offers
            modelBuilder.Entity<ItemOffer>().HasData(
                new ItemOffer { Id = 1, ItemId = 3, OfferId = 1 },
                new ItemOffer { Id = 2, ItemId = 8, OfferId = 1 },
                new ItemOffer { Id = 3, ItemId = 13, OfferId = 1 },
                new ItemOffer { Id = 4, ItemId = 14, OfferId = 1 }
            );

            // Configure Promotion entity
            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.ToTable("promotions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).HasColumnName("title").IsRequired();
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.StartDate).HasColumnName("start_date").IsRequired();
                entity.Property(e => e.EndDate).HasColumnName("end_date").IsRequired();
                entity.Property(e => e.IsActive).HasColumnName("is_active").IsRequired();
                entity.Property(e => e.ItemId).HasColumnName("item_id");
                entity.Property(e => e.DisplayName).HasColumnName("display_name").IsRequired();
                entity.Property(e => e.DisplayPrice).HasColumnName("display_price").IsRequired();
                entity.Property(e => e.IsBundle).HasColumnName("is_bundle").IsRequired();
                entity.Property(e => e.DiscountPercentage).HasColumnName("discount_percentage");
                
                entity.HasOne(e => e.Item)
                    .WithMany()
                    .HasForeignKey(e => e.ItemId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure ItemAllergen entity
            modelBuilder.Entity<ItemAllergen>(entity =>
            {
                entity.ToTable("item_allergens");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ItemId).HasColumnName("item_id");
                entity.Property(e => e.AllergenCode).HasColumnName("allergen_code");
                entity.HasOne(e => e.Item)
                    .WithMany(i => i.ItemAllergens)
                    .HasForeignKey(e => e.ItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure SelectionGroup entity
            modelBuilder.Entity<SelectionGroup>(entity =>
            {
                entity.ToTable("selection_groups");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnName("name").IsRequired();
                entity.Property(e => e.Type).HasColumnName("type").IsRequired();
                entity.Property(e => e.IsRequired).HasColumnName("is_required").IsRequired();
                entity.Property(e => e.MinSelect).HasColumnName("min_select").IsRequired();
                entity.Property(e => e.MaxSelect).HasColumnName("max_select").IsRequired();
                entity.Property(e => e.Threshold).HasColumnName("threshold").IsRequired();
                entity.Property(e => e.DisplayOrder).HasColumnName("display_order").IsRequired();
            });

            // Configure SelectionOption entity
            modelBuilder.Entity<SelectionOption>(entity =>
            {
                entity.ToTable("selection_options");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnName("name").IsRequired();
                entity.Property(e => e.Price).HasColumnName("price").IsRequired();
                entity.Property(e => e.DisplayOrder).HasColumnName("display_order").IsRequired();
                entity.Property(e => e.SelectionGroupId).HasColumnName("selection_group_id").IsRequired();
                
                entity.HasOne(e => e.SelectionGroup)
                    .WithMany(g => g.Options)
                    .HasForeignKey(e => e.SelectionGroupId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure ItemSelectionGroup entity
            modelBuilder.Entity<ItemSelectionGroup>(entity =>
            {
                entity.ToTable("item_selection_groups");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ItemId).HasColumnName("item_id").IsRequired();
                entity.Property(e => e.SelectionGroupId).HasColumnName("selection_group_id").IsRequired();
                
                entity.HasOne(e => e.Item)
                    .WithMany(i => i.ItemSelectionGroups)
                    .HasForeignKey(e => e.ItemId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.SelectionGroup)
                    .WithMany(g => g.ItemSelectionGroups)
                    .HasForeignKey(e => e.SelectionGroupId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure CategorySelectionGroup entity
            modelBuilder.Entity<CategorySelectionGroup>(entity =>
            {
                entity.ToTable("category_selection_groups");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CategoryId).HasColumnName("category_id").IsRequired();
                entity.Property(e => e.SelectionGroupId).HasColumnName("selection_group_id").IsRequired();
                
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.CategorySelectionGroups)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.SelectionGroup)
                    .WithMany(g => g.CategorySelectionGroups)
                    .HasForeignKey(e => e.SelectionGroupId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Postcode entity
            modelBuilder.Entity<Postcode>(entity =>
            {
                entity.ToTable("postcodes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).HasColumnName("code").IsRequired();
                entity.Property(e => e.District).HasColumnName("district");
            });

            // Configure DeliveryAddress entity
            modelBuilder.Entity<DeliveryAddress>(entity =>
            {
                entity.ToTable("delivery_addresses");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PostcodeId).HasColumnName("postcode_id").IsRequired();
                entity.Property(e => e.Street).HasColumnName("street").IsRequired();
                entity.Property(e => e.House).HasColumnName("house");
                entity.Property(e => e.Stairs).HasColumnName("stairs");
                entity.Property(e => e.Stick).HasColumnName("stick");
                entity.Property(e => e.Door).HasColumnName("door");
                entity.Property(e => e.Bell).HasColumnName("bell");
                
                entity.HasOne(e => e.Postcode)
                    .WithMany(p => p.DeliveryAddresses)
                    .HasForeignKey(e => e.PostcodeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure PostcodeMinimumOrder entity
            modelBuilder.Entity<PostcodeMinimumOrder>(entity =>
            {
                entity.ToTable("PostcodeMinimumOrders");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Postcode).HasColumnName("Postcode").IsRequired();
                entity.Property(e => e.MinimumOrderValue).HasColumnName("MinimumOrderValue").IsRequired();
            });
        }
    }
}
