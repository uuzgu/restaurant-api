-- Create categories table
CREATE TABLE "categories" (
    "id" INTEGER NOT NULL CONSTRAINT "id" PRIMARY KEY AUTOINCREMENT,
    "name" TEXT NOT NULL
);

-- Create selection_groups table
CREATE TABLE "selection_groups" (
    "id" INTEGER NOT NULL CONSTRAINT "PK_selection_groups" PRIMARY KEY AUTOINCREMENT,
    "name" TEXT NOT NULL,
    "type" TEXT NOT NULL,
    "is_required" INTEGER NOT NULL,
    "min_select" INTEGER NOT NULL,
    "max_select" INTEGER NULL,
    "threshold" INTEGER NOT NULL,
    "display_order" INTEGER NOT NULL
);

-- Create selection_options table
CREATE TABLE "selection_options" (
    "id" INTEGER NOT NULL CONSTRAINT "PK_selection_options" PRIMARY KEY AUTOINCREMENT,
    "selection_group_id" INTEGER NOT NULL,
    "name" TEXT NOT NULL,
    "price" TEXT NOT NULL,
    "display_order" INTEGER NOT NULL,
    CONSTRAINT "FK_selection_options_selection_groups_selection_group_id" FOREIGN KEY ("selection_group_id") REFERENCES "selection_groups" ("id") ON DELETE CASCADE
);

-- Create category_selection_groups table
CREATE TABLE "category_selection_groups" (
    "id" INTEGER NOT NULL CONSTRAINT "PK_category_selection_groups" PRIMARY KEY AUTOINCREMENT,
    "category_id" INTEGER NOT NULL,
    "selection_group_id" INTEGER NOT NULL,
    "display_order" INTEGER NOT NULL,
    CONSTRAINT "FK_category_selection_groups_categories_category_id" FOREIGN KEY ("category_id") REFERENCES "categories" ("id") ON DELETE CASCADE,
    CONSTRAINT "FK_category_selection_groups_selection_groups_selection_group_id" FOREIGN KEY ("selection_group_id") REFERENCES "selection_groups" ("id") ON DELETE RESTRICT
); 