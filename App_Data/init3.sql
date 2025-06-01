-- Create item_selection_groups table
CREATE TABLE "item_selection_groups" (
    "id" INTEGER NOT NULL CONSTRAINT "PK_item_selection_groups" PRIMARY KEY AUTOINCREMENT,
    "item_id" INTEGER NOT NULL,
    "selection_group_id" INTEGER NOT NULL,
    "display_order" INTEGER NOT NULL,
    CONSTRAINT "FK_item_selection_groups_items_item_id" FOREIGN KEY ("item_id") REFERENCES "items" ("id") ON DELETE CASCADE,
    CONSTRAINT "FK_item_selection_groups_selection_groups_selection_group_id" FOREIGN KEY ("selection_group_id") REFERENCES "selection_groups" ("id") ON DELETE RESTRICT
);

-- Create item_allergens table
CREATE TABLE item_allergens (
    id INTEGER PRIMARY KEY AUTOINCREMENT, 
    item_id INTEGER NOT NULL, 
    allergen_code TEXT NOT NULL, 
    FOREIGN KEY (item_id) REFERENCES items(id) ON DELETE CASCADE
);

-- Create offers table
CREATE TABLE "offers" (
    "Id" INTEGER NOT NULL CONSTRAINT "Id" PRIMARY KEY AUTOINCREMENT,
    "name" TEXT NOT NULL,
    "description" TEXT NULL,
    "discount_percentage" TEXT NOT NULL,
    "start_date" TEXT NOT NULL,
    "end_date" TEXT NOT NULL,
    "is_active" INTEGER NOT NULL
);

-- Create item_offers table
CREATE TABLE "item_offers" (
    "Id" INTEGER NOT NULL CONSTRAINT "Id" PRIMARY KEY AUTOINCREMENT,
    "item_id" INTEGER NOT NULL,
    "offer_id" INTEGER NOT NULL,
    CONSTRAINT "FK_item_offers_items_item_id" FOREIGN KEY ("item_id") REFERENCES "items" ("id") ON DELETE CASCADE,
    CONSTRAINT "FK_item_offers_offers_offer_id" FOREIGN KEY ("offer_id") REFERENCES "offers" ("Id") ON DELETE CASCADE
);

-- Create promotions table
CREATE TABLE promotions (
    id INTEGER PRIMARY KEY AUTOINCREMENT, 
    title TEXT NOT NULL, 
    description TEXT, 
    start_date TEXT NOT NULL, 
    end_date TEXT NOT NULL, 
    is_active INTEGER NOT NULL, 
    item_id INTEGER, 
    display_name TEXT NOT NULL, 
    display_price REAL NOT NULL, 
    is_bundle INTEGER NOT NULL, 
    discount_percentage REAL, 
    FOREIGN KEY (item_id) REFERENCES items(id) ON DELETE SET NULL
); 