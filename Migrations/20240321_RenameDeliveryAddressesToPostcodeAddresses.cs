using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantApi.Migrations
{
    public partial class RenameDeliveryAddressesToPostcodeAddresses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create the new table
            migrationBuilder.CreateTable(
                name: "postcode_addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    postcode_id = table.Column<int>(type: "INTEGER", nullable: false),
                    street = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_postcode_addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_postcode_addresses_postcodes_postcode_id",
                        column: x => x.postcode_id,
                        principalTable: "postcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Copy data from old table to new table
            migrationBuilder.Sql(
                @"INSERT INTO postcode_addresses (postcode_id, street)
                  SELECT postcode_id, street FROM delivery_addresses");

            // Drop the old table
            migrationBuilder.DropTable(
                name: "delivery_addresses");

            // Create index on postcode_id
            migrationBuilder.CreateIndex(
                name: "IX_postcode_addresses_postcode_id",
                table: "postcode_addresses",
                column: "postcode_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Create the old table
            migrationBuilder.CreateTable(
                name: "delivery_addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    postcode_id = table.Column<int>(type: "INTEGER", nullable: false),
                    street = table.Column<string>(type: "TEXT", nullable: false),
                    house = table.Column<string>(type: "TEXT", nullable: true),
                    stairs = table.Column<string>(type: "TEXT", nullable: true),
                    stick = table.Column<string>(type: "TEXT", nullable: true),
                    door = table.Column<string>(type: "TEXT", nullable: true),
                    bell = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery_addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_delivery_addresses_postcodes_postcode_id",
                        column: x => x.postcode_id,
                        principalTable: "postcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Copy data back from new table to old table
            migrationBuilder.Sql(
                @"INSERT INTO delivery_addresses (postcode_id, street)
                  SELECT postcode_id, street FROM postcode_addresses");

            // Drop the new table
            migrationBuilder.DropTable(
                name: "postcode_addresses");

            // Create index on postcode_id
            migrationBuilder.CreateIndex(
                name: "IX_delivery_addresses_postcode_id",
                table: "delivery_addresses",
                column: "postcode_id");
        }
    }
} 