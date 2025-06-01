using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantApi.Migrations
{
    public partial class MigrateIngredientsToSelectionGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create a new selection group for ingredients
            migrationBuilder.Sql(@"
                INSERT INTO selection_groups (name, type, is_required, min_select, max_select, threshold, display_order)
                VALUES ('Ingredients', 'MULTIPLE', 0, 0, 999, 0, 0);
            ");

            // Get the ID of the newly created selection group
            var ingredientsGroupId = migrationBuilder.Sql(@"
                SELECT last_insert_rowid();
            ");

            // Move all ingredients to selection options
            migrationBuilder.Sql(@"
                INSERT INTO selection_options (selection_group_id, name, price, display_order)
                SELECT 
                    (SELECT id FROM selection_groups WHERE name = 'Ingredients'),
                    name,
                    extra_cost,
                    Id
                FROM item_ingredients;
            ");

            // Link ingredients selection group to all items
            migrationBuilder.Sql(@"
                INSERT INTO item_selection_groups (item_id, selection_group_id, display_order)
                SELECT DISTINCT
                    item_id,
                    (SELECT id FROM selection_groups WHERE name = 'Ingredients'),
                    0
                FROM item_ingredients;
            ");

            // Drop the old tables
            migrationBuilder.DropTable(
                name: "item_ingredients");

            migrationBuilder.DropTable(
                name: "drink_options");

            migrationBuilder.DropTable(
                name: "side_options");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Recreate the old tables
            migrationBuilder.CreateTable(
                name: "item_ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    item_id = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    is_mandatory = table.Column<int>(type: "INTEGER", nullable: false),
                    can_exclude = table.Column<int>(type: "INTEGER", nullable: false),
                    extra_cost = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_item_ingredients_items_item_id",
                        column: x => x.item_id,
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "drink_options",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    item_id = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    price = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_drink_options_items_item_id",
                        column: x => x.item_id,
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "side_options",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    item_id = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    price = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_side_options_items_item_id",
                        column: x => x.item_id,
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Move data back from selection groups
            migrationBuilder.Sql(@"
                INSERT INTO item_ingredients (item_id, name, is_mandatory, can_exclude, extra_cost)
                SELECT 
                    isg.item_id,
                    so.name,
                    0,
                    1,
                    so.price
                FROM item_selection_groups isg
                JOIN selection_groups sg ON sg.id = isg.selection_group_id
                JOIN selection_options so ON so.selection_group_id = sg.id
                WHERE sg.name = 'Ingredients';
            ");

            // Remove the ingredients selection group
            migrationBuilder.Sql(@"
                DELETE FROM selection_groups WHERE name = 'Ingredients';
            ");

            // Create indexes
            migrationBuilder.CreateIndex(
                name: "IX_item_ingredients_item_id",
                table: "item_ingredients",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_drink_options_item_id",
                table: "drink_options",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_side_options_item_id",
                table: "side_options",
                column: "item_id");
        }
    }
} 