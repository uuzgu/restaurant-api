using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantApi.Migrations
{
    public partial class FixOrderCustomerInfoForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the existing foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_orders_customer_info_customer_info_id",
                table: "orders");

            // Add the correct foreign key
            migrationBuilder.AddForeignKey(
                name: "FK_orders_customerOrder_info_customer_info_id",
                table: "orders",
                column: "customer_info_id",
                principalTable: "customerOrder_info",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the correct foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_orders_customerOrder_info_customer_info_id",
                table: "orders");

            // Add back the old foreign key
            migrationBuilder.AddForeignKey(
                name: "FK_orders_customer_info_customer_info_id",
                table: "orders",
                column: "customer_info_id",
                principalTable: "customer_info",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
} 