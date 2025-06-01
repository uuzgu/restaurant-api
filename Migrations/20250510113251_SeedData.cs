using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 1,
                column: "name",
                value: "Pizza");

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 2,
                column: "name",
                value: "Bowls");

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 3,
                column: "name",
                value: "Hamburgers");

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 4,
                column: "name",
                value: "Salads");

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 5, "Breakfast" },
                    { 6, "Drinks" },
                    { 7, "Soups" },
                    { 8, "Desserts" }
                });

            migrationBuilder.InsertData(
                table: "drink_options",
                columns: new[] { "Id", "item_id", "name", "price" },
                values: new object[,]
                {
                    { 5, 3, "Cola", 1.0m },
                    { 6, 3, "Fanta", 2.0m }
                });

            migrationBuilder.InsertData(
                table: "item_ingredients",
                columns: new[] { "Id", "can_exclude", "extra_cost", "is_mandatory", "item_id", "name" },
                values: new object[,]
                {
                    { 1, false, 0m, false, 3, "Dough" },
                    { 2, false, 0m, false, 3, "Tomato Sauce" },
                    { 3, false, 0m, false, 3, "Mozzarella" },
                    { 4, false, 0m, false, 3, "Basil" }
                });

            migrationBuilder.UpdateData(
                table: "items",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "category_id", "description", "name", "price" },
                values: new object[] { 6, "Classic carbonated soft drink", "Cola", 4 });

            migrationBuilder.UpdateData(
                table: "items",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "category_id", "description", "image_url", "name" },
                values: new object[] { 4, "Salad with fresh vegetables and cheese", "https://restaurant-images33.s3.eu-north-1.amazonaws.com/salad.jpg", "Salad" });

            migrationBuilder.UpdateData(
                table: "items",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "category_id", "description", "image_url", "name", "price" },
                values: new object[] { 1, "A delightful plant-based pizza with a variety of fresh toppings", "https://restaurant-images33.s3.eu-north-1.amazonaws.com/veganpizza.jpg", "Vegan Pizza", 12 });

            migrationBuilder.UpdateData(
                table: "items",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "category_id", "description", "image_url", "name", "price" },
                values: new object[] { 2, "Filled with fresh ingredients, perfect for a healthy and satisfying meal.", "https://restaurant-images33.s3.eu-north-1.amazonaws.com/bowll.jpg", "Bowl", 10 });

            migrationBuilder.UpdateData(
                table: "items",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "category_id", "description", "name", "price" },
                values: new object[] { 6, "Pure, crisp, and refreshing still water", "Water", 1 });

            migrationBuilder.InsertData(
                table: "items",
                columns: new[] { "id", "category_id", "description", "image_url", "name", "price" },
                values: new object[,]
                {
                    { 8, 1, "Classic pizza with rich tomato sauce, fresh mozzarella, and basil", "https://restaurant-images33.s3.eu-north-1.amazonaws.com/margarita.jpg", "Margarita Pizza", 9 },
                    { 10, 3, "A delicious burger made with a juicy beef patty, crispy bacon, melted cheese, and fresh onions, all nestled in a soft bun", "https://restaurant-images33.s3.eu-north-1.amazonaws.com/baconburger.jpg", "Baconburger", 6 },
                    { 11, 3, "Classic American cheeseburger", "https://restaurant-images33.s3.eu-north-1.amazonaws.com/cheeseburger.jpg", "Cheeseburger", 7 },
                    { 12, 4, "A healthy salad with grilled chicken, fresh veggies, and dressing", "https://restaurant-images33.s3.eu-north-1.amazonaws.com/chickensalad.jpg", "Chicken Salad", 9 },
                    { 13, 1, "A savory pizza topped with grilled chicken and fresh vegetables", "https://restaurant-images33.s3.eu-north-1.amazonaws.com/chickenpizza.jpg", "Chicken Pizza", 10 },
                    { 14, 1, "A flavorful pizza with spicy sausage and cheese", "https://restaurant-images33.s3.eu-north-1.amazonaws.com/sausagepizza.jpg", "Sausage Pizza", 10 }
                });

            migrationBuilder.InsertData(
                table: "offers",
                columns: new[] { "Id", "description", "discount_percentage", "end_date", "is_active", "name", "start_date" },
                values: new object[,]
                {
                    { 1, "20% off on all pizzas", 20.0m, "2025-05-21 14:32:26.987536", true, "Happy Hour", "2025-04-21 14:32:26.987483" },
                    { 2, "15% off with student ID", 15.0m, "2025-07-20 14:32:26.987655", true, "Student Discount", "2025-04-21 14:32:26.987655" }
                });

            migrationBuilder.InsertData(
                table: "side_options",
                columns: new[] { "Id", "item_id", "name", "price" },
                values: new object[,]
                {
                    { 1, 3, "French Fries", 2.0m },
                    { 2, 3, "Onion Rings", 3.0m },
                    { 3, 3, "Salad", 4.0m }
                });

            migrationBuilder.InsertData(
                table: "item_offers",
                columns: new[] { "Id", "item_id", "offer_id" },
                values: new object[,]
                {
                    { 1, 3, 1 },
                    { 2, 8, 1 },
                    { 3, 13, 1 },
                    { 4, 14, 1 }
                });

            migrationBuilder.InsertData(
                table: "items",
                columns: new[] { "id", "category_id", "description", "image_url", "name", "price" },
                values: new object[,]
                {
                    { 6, 6, "Traditional Turkish yogurt-based drink, cool and refreshing with a creamy texture", null, "Ayran", 2 },
                    { 7, 6, "A light, refreshing lemon-lime soda with a crisp, bubbly taste to quench your thirst", null, "Sprite", 3 },
                    { 9, 6, "A premium Turkish soda, light and refreshing, perfect for pairing with your meal.", null, "Soda", 2 },
                    { 15, 5, "A delicious breakfast with bacon, eggs, and fresh vegetables", "https://restaurant-images33.s3.eu-north-1.amazonaws.com/baconbreakfast.jpg", "Bacon Breakfast Menu", 12 },
                    { 16, 7, "Spicy, sweet, and fragrant with lemongrass, lime, and tender chicken", "https://restaurant-images33.s3.eu-north-1.amazonaws.com/thaisoup.jpg", "Thai Coconut Chicken Soup", 7 },
                    { 17, 7, "Rich, earthy, and velvety with sautéed mushrooms and a hint of garlic", "https://restaurant-images33.s3.eu-north-1.amazonaws.com/mushroomsoup.jpg", "Creamy Wild Mushroom Soup", 6 },
                    { 18, 7, "Smooth, tangy, and topped with a swirl of cream and fresh basil", "https://restaurant-images33.s3.eu-north-1.amazonaws.com/tomatosoup.jpg", "Roasted Tomato Basil Soup", 6 },
                    { 19, 8, "Warm, gooey center served with a scoop of vanilla ice cream", "https://restaurant-images33.s3.eu-north-1.amazonaws.com/moltendessert.jpg", "Molten Chocolate Lava Cake", 5 },
                    { 20, 8, "Zesty lemon curd topped with pillowy toasted meringue in a buttery crust", "https://restaurant-images33.s3.eu-north-1.amazonaws.com/lemontartdessert.jpg", "Lemon Meringue Tart", 5 },
                    { 21, 8, "Layers of fluffy cake, whipped cream, and fresh strawberries", "https://restaurant-images33.s3.eu-north-1.amazonaws.com/strawberrydessert.jpg", "Strawberry Shortcake Parfait", 6 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "drink_options",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "drink_options",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "item_ingredients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "item_ingredients",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "item_ingredients",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "item_ingredients",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "item_offers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "item_offers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "item_offers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "item_offers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "offers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "side_options",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "side_options",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "side_options",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "offers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 1,
                column: "name",
                value: "Appetizers");

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 2,
                column: "name",
                value: "Main Courses");

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 3,
                column: "name",
                value: "Desserts");

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 4,
                column: "name",
                value: "Drinks");

            migrationBuilder.UpdateData(
                table: "items",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "category_id", "description", "name", "price" },
                values: new object[] { 1, "Toasted bread with garlic butter", "Garlic Bread", 5 });

            migrationBuilder.UpdateData(
                table: "items",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "category_id", "description", "image_url", "name" },
                values: new object[] { 1, "Fresh romaine lettuce with Caesar dressing", null, "Caesar Salad" });

            migrationBuilder.UpdateData(
                table: "items",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "category_id", "description", "image_url", "name", "price" },
                values: new object[] { 2, "Fresh salmon with lemon butter sauce", null, "Grilled Salmon", 24 });

            migrationBuilder.UpdateData(
                table: "items",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "category_id", "description", "image_url", "name", "price" },
                values: new object[] { 3, "Rich chocolate cake with ganache", null, "Chocolate Cake", 7 });

            migrationBuilder.UpdateData(
                table: "items",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "category_id", "description", "name", "price" },
                values: new object[] { 4, "Freshly squeezed orange juice", "Fresh Orange Juice", 4 });
        }
    }
}
