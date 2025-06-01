using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantApi.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureCustomerInfoAndAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerInfoId",
                table: "orders",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "order_method",
                table: "orders",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "special_notes",
                table: "orders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "customer_info",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    first_name = table.Column<string>(type: "TEXT", nullable: false),
                    last_name = table.Column<string>(type: "TEXT", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    phone = table.Column<string>(type: "TEXT", nullable: true),
                    postal_code = table.Column<string>(type: "TEXT", nullable: true),
                    street = table.Column<string>(type: "TEXT", nullable: true),
                    house = table.Column<string>(type: "TEXT", nullable: true),
                    stairs = table.Column<string>(type: "TEXT", nullable: true),
                    stick = table.Column<string>(type: "TEXT", nullable: true),
                    door = table.Column<string>(type: "TEXT", nullable: true),
                    bell = table.Column<string>(type: "TEXT", nullable: true),
                    comment = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("id", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_orders_CustomerInfoId",
                table: "orders",
                column: "CustomerInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_customer_info_CustomerInfoId",
                table: "orders",
                column: "CustomerInfoId",
                principalTable: "customer_info",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_customer_info_CustomerInfoId",
                table: "orders");

            migrationBuilder.DropTable(
                name: "customer_info");

            migrationBuilder.DropIndex(
                name: "IX_orders_CustomerInfoId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "CustomerInfoId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "order_method",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "special_notes",
                table: "orders");
        }
    }
}
