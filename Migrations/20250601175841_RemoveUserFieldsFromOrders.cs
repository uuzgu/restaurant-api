using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserFieldsFromOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create a temporary table without the user_id column
            migrationBuilder.CreateTable(
                name: "ef_temp_orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    order_number = table.Column<string>(type: "TEXT", nullable: false),
                    status = table.Column<string>(type: "TEXT", nullable: false),
                    total = table.Column<string>(type: "TEXT", nullable: false),
                    payment_method = table.Column<string>(type: "TEXT", nullable: false),
                    order_method = table.Column<string>(type: "TEXT", nullable: false),
                    special_notes = table.Column<string>(type: "TEXT", nullable: true),
                    customer_info_id = table.Column<int>(type: "INTEGER", nullable: true),
                    created_at = table.Column<string>(type: "TEXT", nullable: false),
                    updated_at = table.Column<string>(type: "TEXT", nullable: false),
                    stripe_session_id = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("id", x => x.id);
                    table.ForeignKey(
                        name: "FK_orders_customerOrder_info_customer_info_id",
                        column: x => x.customer_info_id,
                        principalTable: "customerOrder_info",
                        principalColumn: "Id");
                });

            // Copy data to the temporary table
            migrationBuilder.Sql("INSERT INTO ef_temp_orders SELECT id, order_number, status, total, payment_method, order_method, special_notes, customer_info_id, created_at, updated_at, stripe_session_id FROM orders;");

            // Drop the original table
            migrationBuilder.DropTable(name: "orders");

            // Rename the temporary table to the original name
            migrationBuilder.RenameTable(
                name: "ef_temp_orders",
                newName: "orders");

            // Create the index on customer_info_id
            migrationBuilder.CreateIndex(
                name: "IX_orders_customer_info_id",
                table: "orders",
                column: "customer_info_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Add back the user_id column
            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "orders",
                type: "INTEGER",
                nullable: true);

            // Recreate the foreign key
            migrationBuilder.AddForeignKey(
                name: "FK_orders_users_user_id",
                table: "orders",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
