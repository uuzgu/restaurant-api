using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("id", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    is_periodic = table.Column<int>(type: "INTEGER", nullable: false),
                    start_date = table.Column<string>(type: "TEXT", nullable: true),
                    end_date = table.Column<string>(type: "TEXT", nullable: true),
                    discount_ratio = table.Column<decimal>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    is_used = table.Column<int>(type: "INTEGER", nullable: false),
                    created_at = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "customerOrder_info",
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
                    comment = table.Column<string>(type: "TEXT", nullable: true),
                    create_date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "offers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    discount_percentage = table.Column<decimal>(type: "TEXT", nullable: false),
                    start_date = table.Column<string>(type: "TEXT", nullable: false),
                    end_date = table.Column<string>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostcodeMinimumOrders",
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

            migrationBuilder.CreateTable(
                name: "postcodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    code = table.Column<string>(type: "TEXT", nullable: false),
                    district = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_postcodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "selection_groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    type = table.Column<string>(type: "TEXT", nullable: false),
                    is_required = table.Column<bool>(type: "INTEGER", nullable: false),
                    min_select = table.Column<int>(type: "INTEGER", nullable: false),
                    max_select = table.Column<int>(type: "INTEGER", nullable: false),
                    threshold = table.Column<int>(type: "INTEGER", nullable: false),
                    display_order = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_selection_groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    price = table.Column<decimal>(type: "TEXT", nullable: false),
                    category_id = table.Column<int>(type: "INTEGER", nullable: false),
                    image_url = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_items_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "coupon_schedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    coupon_id = table.Column<int>(type: "INTEGER", nullable: false),
                    monday = table.Column<int>(type: "INTEGER", nullable: false),
                    tuesday = table.Column<int>(type: "INTEGER", nullable: false),
                    wednesday = table.Column<int>(type: "INTEGER", nullable: false),
                    thursday = table.Column<int>(type: "INTEGER", nullable: false),
                    friday = table.Column<int>(type: "INTEGER", nullable: false),
                    saturday = table.Column<int>(type: "INTEGER", nullable: false),
                    sunday = table.Column<int>(type: "INTEGER", nullable: false),
                    begin_time = table.Column<string>(type: "TEXT", nullable: false),
                    end_time = table.Column<string>(type: "TEXT", nullable: false),
                    created_at = table.Column<string>(type: "TEXT", nullable: false),
                    updated_at = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coupon_schedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_coupon_schedule_Coupons_coupon_id",
                        column: x => x.coupon_id,
                        principalTable: "Coupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "coupons_history",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    coupon_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    used_at = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coupons_history", x => x.Id);
                    table.ForeignKey(
                        name: "FK_coupons_history_Coupons_coupon_id",
                        column: x => x.coupon_id,
                        principalTable: "Coupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "orders",
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
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "category_selection_groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    category_id = table.Column<int>(type: "INTEGER", nullable: false),
                    selection_group_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_selection_groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_category_selection_groups_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_category_selection_groups_selection_groups_selection_group_id",
                        column: x => x.selection_group_id,
                        principalTable: "selection_groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "selection_options",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    price = table.Column<decimal>(type: "TEXT", nullable: false),
                    display_order = table.Column<int>(type: "INTEGER", nullable: false),
                    selection_group_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_selection_options", x => x.Id);
                    table.ForeignKey(
                        name: "FK_selection_options_selection_groups_selection_group_id",
                        column: x => x.selection_group_id,
                        principalTable: "selection_groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_allergens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    item_id = table.Column<int>(type: "INTEGER", nullable: false),
                    allergen_code = table.Column<string>(type: "TEXT", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_allergens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_item_allergens_items_item_id",
                        column: x => x.item_id,
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    item_id = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    is_mandatory = table.Column<bool>(type: "INTEGER", nullable: false),
                    can_exclude = table.Column<bool>(type: "INTEGER", nullable: false),
                    extra_cost = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_ingredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_item_ingredients_items_item_id",
                        column: x => x.item_id,
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_offers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    item_id = table.Column<int>(type: "INTEGER", nullable: false),
                    offer_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_item_offers_items_item_id",
                        column: x => x.item_id,
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_item_offers_offers_offer_id",
                        column: x => x.offer_id,
                        principalTable: "offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_selection_groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    item_id = table.Column<int>(type: "INTEGER", nullable: false),
                    selection_group_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_selection_groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_item_selection_groups_items_item_id",
                        column: x => x.item_id,
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_item_selection_groups_selection_groups_selection_group_id",
                        column: x => x.selection_group_id,
                        principalTable: "selection_groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "promotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    title = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false),
                    start_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    end_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false),
                    item_id = table.Column<int>(type: "INTEGER", nullable: true),
                    display_name = table.Column<string>(type: "TEXT", nullable: false),
                    display_price = table.Column<decimal>(type: "TEXT", nullable: false),
                    is_bundle = table.Column<bool>(type: "INTEGER", nullable: false),
                    discount_percentage = table.Column<decimal>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_promotions_items_item_id",
                        column: x => x.item_id,
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "order_details",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    order_id = table.Column<int>(type: "INTEGER", nullable: false),
                    item_details = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("id", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_details_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.InsertData(
                table: "offers",
                columns: new[] { "Id", "description", "discount_percentage", "end_date", "is_active", "name", "start_date" },
                values: new object[,]
                {
                    { 1, "20% off on all pizzas", 20.0m, "2025-05-21 14:32:26.987536", true, "Happy Hour", "2025-04-21 14:32:26.987483" },
                    { 2, "15% off with student ID", 15.0m, "2025-07-20 14:32:26.987655", true, "Student Discount", "2025-04-21 14:32:26.987655" }
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

            migrationBuilder.CreateIndex(
                name: "IX_category_selection_groups_category_id",
                table: "category_selection_groups",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_category_selection_groups_selection_group_id",
                table: "category_selection_groups",
                column: "selection_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_coupon_schedule_coupon_id",
                table: "coupon_schedule",
                column: "coupon_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_coupons_history_coupon_id",
                table: "coupons_history",
                column: "coupon_id");

            migrationBuilder.CreateIndex(
                name: "IX_delivery_addresses_postcode_id",
                table: "delivery_addresses",
                column: "postcode_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_allergens_item_id",
                table: "item_allergens",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_ingredients_item_id",
                table: "item_ingredients",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_offers_item_id",
                table: "item_offers",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_offers_offer_id",
                table: "item_offers",
                column: "offer_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_selection_groups_item_id",
                table: "item_selection_groups",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_selection_groups_selection_group_id",
                table: "item_selection_groups",
                column: "selection_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_items_category_id",
                table: "items",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_details_order_id",
                table: "order_details",
                column: "order_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_customer_info_id",
                table: "orders",
                column: "customer_info_id");

            migrationBuilder.CreateIndex(
                name: "IX_promotions_item_id",
                table: "promotions",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_selection_options_selection_group_id",
                table: "selection_options",
                column: "selection_group_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "category_selection_groups");

            migrationBuilder.DropTable(
                name: "coupon_schedule");

            migrationBuilder.DropTable(
                name: "coupons_history");

            migrationBuilder.DropTable(
                name: "delivery_addresses");

            migrationBuilder.DropTable(
                name: "item_allergens");

            migrationBuilder.DropTable(
                name: "item_ingredients");

            migrationBuilder.DropTable(
                name: "item_offers");

            migrationBuilder.DropTable(
                name: "item_selection_groups");

            migrationBuilder.DropTable(
                name: "order_details");

            migrationBuilder.DropTable(
                name: "PostcodeMinimumOrders");

            migrationBuilder.DropTable(
                name: "promotions");

            migrationBuilder.DropTable(
                name: "selection_options");

            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "postcodes");

            migrationBuilder.DropTable(
                name: "offers");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "items");

            migrationBuilder.DropTable(
                name: "selection_groups");

            migrationBuilder.DropTable(
                name: "customerOrder_info");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
