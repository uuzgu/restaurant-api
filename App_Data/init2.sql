-- Create postcodes table
CREATE TABLE postcodes (
    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
    code TEXT NOT NULL, 
    district TEXT
);

-- Create postcode_addresses table
CREATE TABLE postcode_addresses (
    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
    postcode_id INTEGER NOT NULL, 
    street TEXT NOT NULL, 
    FOREIGN KEY (postcode_id) REFERENCES postcodes (Id) ON DELETE CASCADE
);

-- Create delivery_addresses table
CREATE TABLE delivery_addresses (
    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
    postcode_id INTEGER NOT NULL, 
    street TEXT NOT NULL, 
    house TEXT, 
    stairs TEXT, 
    stick TEXT, 
    door TEXT, 
    bell TEXT, 
    FOREIGN KEY (postcode_id) REFERENCES postcodes (Id) ON DELETE CASCADE
);

-- Create PostcodeMinimumOrders table
CREATE TABLE "PostcodeMinimumOrders" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_PostcodeMinimumOrders" PRIMARY KEY AUTOINCREMENT,
    "Postcode" TEXT NOT NULL,
    "MinimumOrderValue" TEXT NOT NULL
);

-- Create items table
CREATE TABLE "items" (
    "id" INTEGER NOT NULL CONSTRAINT "PK_items" PRIMARY KEY AUTOINCREMENT,
    "category_id" INTEGER NOT NULL,
    "description" TEXT NULL,
    "image_url" TEXT NULL,
    "name" TEXT NOT NULL,
    "price" INTEGER NOT NULL,
    CONSTRAINT "FK_items_categories_category_id" FOREIGN KEY ("category_id") REFERENCES "categories" ("id") ON DELETE RESTRICT
); 