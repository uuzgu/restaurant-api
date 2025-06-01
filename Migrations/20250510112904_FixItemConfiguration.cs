using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantApi.Migrations
{
    /// <inheritdoc />
    public partial class FixItemConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "id",
                table: "items");

            migrationBuilder.AddPrimaryKey(
                name: "PK_items",
                table: "items",
                column: "id");

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Appetizers" },
                    { 2, "Main Courses" },
                    { 3, "Desserts" },
                    { 4, "Drinks" }
                });

            migrationBuilder.InsertData(
                table: "items",
                columns: new[] { "id", "category_id", "description", "image_url", "name", "price" },
                values: new object[,]
                {
                    { 1, 1, "Toasted bread with garlic butter", null, "Garlic Bread", 5 },
                    { 2, 1, "Fresh romaine lettuce with Caesar dressing", null, "Caesar Salad", 8 },
                    { 3, 2, "Fresh salmon with lemon butter sauce", null, "Grilled Salmon", 24 },
                    { 4, 3, "Rich chocolate cake with ganache", null, "Chocolate Cake", 7 },
                    { 5, 4, "Freshly squeezed orange juice", null, "Fresh Orange Juice", 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_items",
                table: "items");

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "items",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.AddPrimaryKey(
                name: "id",
                table: "items",
                column: "id");
        }
    }
}
