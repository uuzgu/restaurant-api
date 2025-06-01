using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerInfoIdToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_customer_info_CustomerInfoId",
                table: "orders");

            migrationBuilder.AlterColumn<int>(
                name: "customer_info_id",
                table: "orders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_customer_info_customer_info_id",
                table: "orders",
                column: "customer_info_id",
                principalTable: "customer_info",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_customer_info_customer_info_id",
                table: "orders");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerInfoId",
                table: "orders",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_customer_info_CustomerInfoId",
                table: "orders",
                column: "CustomerInfoId",
                principalTable: "customer_info",
                principalColumn: "Id");
        }
    }
}
