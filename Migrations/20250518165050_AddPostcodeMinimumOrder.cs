using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPostcodeMinimumOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "postcode_minimum_orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Postcode = table.Column<string>(type: "TEXT", nullable: false),
                    MinimumOrderValue = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostcodeMinimumOrders", x => x.Id);
                });

            // Seed minimum order values
            migrationBuilder.InsertData(
                table: "postcode_minimum_orders",
                columns: new[] { "Postcode", "MinimumOrderValue" },
                values: new object[,]
                {
                    { "1010", 20.00m },
                    { "1020", 25.00m },
                    { "1030", 30.00m },
                    { "1040", 25.00m },
                    { "1050", 20.00m },
                    { "1060", 25.00m },
                    { "1070", 30.00m },
                    { "1080", 25.00m },
                    { "1090", 20.00m },
                    { "1100", 25.00m },
                    { "1110", 30.00m },
                    { "1120", 25.00m },
                    { "1130", 20.00m },
                    { "1140", 25.00m },
                    { "1150", 30.00m },
                    { "1160", 25.00m },
                    { "1170", 20.00m },
                    { "1180", 25.00m },
                    { "1190", 30.00m },
                    { "1200", 25.00m },
                    { "1210", 20.00m },
                    { "1220", 25.00m },
                    { "1230", 30.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "postcode_minimum_orders");
        }
    }
}
