-- Enable foreign key support
PRAGMA foreign_keys = ON;

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

-- Create coupons table
CREATE TABLE coupons (
    id INTEGER PRIMARY KEY AUTOINCREMENT, 
    code TEXT NOT NULL UNIQUE, 
    type TEXT NOT NULL, 
    is_periodic INTEGER NOT NULL DEFAULT 0, 
    start_date TEXT, 
    end_date TEXT, 
    email TEXT, 
    is_used INTEGER DEFAULT 0, 
    created_at TEXT DEFAULT CURRENT_TIMESTAMP, 
    discount_ratio REAL DEFAULT 0
);

-- Create coupon_schedule table
CREATE TABLE coupon_schedule (
    id INTEGER PRIMARY KEY AUTOINCREMENT, 
    coupon_id INTEGER NOT NULL, 
    monday INTEGER NOT NULL DEFAULT 0, 
    tuesday INTEGER NOT NULL DEFAULT 0, 
    wednesday INTEGER NOT NULL DEFAULT 0, 
    thursday INTEGER NOT NULL DEFAULT 0, 
    friday INTEGER NOT NULL DEFAULT 0, 
    saturday INTEGER NOT NULL DEFAULT 0, 
    sunday INTEGER NOT NULL DEFAULT 0, 
    begin_time TEXT NOT NULL, 
    end_time TEXT NOT NULL, 
    created_at TEXT DEFAULT CURRENT_TIMESTAMP, 
    updated_at TEXT DEFAULT CURRENT_TIMESTAMP, 
    FOREIGN KEY (coupon_id) REFERENCES coupons(id), 
    CHECK (begin_time < end_time)
);

-- Create coupons_history table
CREATE TABLE coupons_history (
    id INTEGER PRIMARY KEY AUTOINCREMENT, 
    coupon_id INTEGER NOT NULL, 
    email TEXT, 
    used_at TEXT DEFAULT CURRENT_TIMESTAMP, 
    FOREIGN KEY (coupon_id) REFERENCES coupons(id)
);

-- Create customerOrder_info table
CREATE TABLE "customerOrder_info" (
    "Id" INTEGER NOT NULL CONSTRAINT "id" PRIMARY KEY AUTOINCREMENT,
    "first_name" TEXT NOT NULL,
    "last_name" TEXT NOT NULL,
    "email" TEXT NOT NULL,
    "phone" TEXT NULL,
    "postal_code" TEXT NULL,
    "street" TEXT NULL,
    "house" TEXT NULL,
    "stairs" TEXT NULL,
    "stick" TEXT NULL,
    "door" TEXT NULL,
    "bell" TEXT NULL,
    "comment" TEXT NULL,
    create_date DATETIME
);

-- Create orders table
CREATE TABLE "orders" (
    id INTEGER NOT NULL CONSTRAINT id PRIMARY KEY AUTOINCREMENT, 
    created_at TEXT NOT NULL, 
    customer_info_id INTEGER NULL, 
    order_method TEXT NOT NULL, 
    order_number TEXT NOT NULL, 
    payment_method TEXT NOT NULL, 
    special_notes TEXT NULL, 
    status TEXT NOT NULL, 
    stripe_session_id TEXT NULL, 
    total TEXT NOT NULL, 
    updated_at TEXT NOT NULL, 
    user_id INTEGER NULL, 
    CONSTRAINT FK_orders_customerOrder_info_customer_info_id FOREIGN KEY (customer_info_id) REFERENCES customerOrder_info(Id), 
    CONSTRAINT FK_orders_users_user_id FOREIGN KEY (user_id) REFERENCES users(Id)
);

-- Create order_details table
CREATE TABLE order_details (
    id INTEGER PRIMARY KEY AUTOINCREMENT, 
    order_id INTEGER NOT NULL, 
    item_details TEXT NOT NULL, 
    FOREIGN KEY (order_id) REFERENCES orders(id) ON DELETE CASCADE
);

-- Insert categories data
INSERT INTO categories (id, name) VALUES
(0, 'Promotions'),
(1, 'Pizza'),
(2, 'Bowls'),
(3, 'Hamburgers'),
(4, 'Salads'),
(5, 'Breakfast'),
(6, 'Drinks'),
(7, 'Soups'),
(8, 'Desserts');

-- Insert selection_groups data
INSERT INTO selection_groups (id, name, type, is_required, min_select, max_select, threshold, display_order) VALUES
(1, 'Pizza Size', 'SINGLE', 1, 1, 1, 0, 1),
(2, 'Crust Type', 'SINGLE', 1, 1, 1, 0, 2),
(3, 'Extra Toppings', 'MULTIPLE', 0, 0, 5, 0, 3),
(4, 'Drink Size', 'SINGLE', 1, 1, 1, 0, 1),
(5, 'Ice Options', 'SINGLE', 0, 0, 1, 0, 2),
(6, 'Drink Options', 'MULTIPLE', 1, 1, 5, 0, 4),
(7, 'Side Options', 'MULTIPLE', 0, 0, 3, 0, 5),
(8, 'Bowl Size', 'SINGLE', 1, 1, 1, 0, 1),
(9, 'Base Choice', 'SINGLE', 1, 1, 1, 0, 2),
(10, 'Protein Choice', 'SINGLE', 1, 1, 1, 0, 3),
(11, 'Vegetable Toppings', 'MULTIPLE', 0, 0, 4, 0, 4),
(12, 'Sauce Choice', 'SINGLE', 1, 1, 1, 0, 5),
(13, 'Burger Size', 'SINGLE', 1, 1, 1, 0, 1),
(14, 'Burger Toppings', 'MULTIPLE', 0, 0, 5, 0, 2),
(15, 'Cooking Preference', 'SINGLE', 1, 1, 1, 0, 3),
(16, 'Salad Size', 'SINGLE', 1, 1, 1, 0, 1),
(17, 'Salad Add-ons', 'MULTIPLE', 0, 0, 3, 0, 2),
(18, 'Dressing Choice', 'SINGLE', 1, 1, 1, 0, 3),
(19, 'Breakfast Sides', 'MULTIPLE', 0, 0, 2, 0, 1),
(20, 'Drink Size', 'SINGLE', 1, 1, 1, 0, 1),
(21, 'Drink Add-ons', 'MULTIPLE', 0, 0, 2, 0, 2),
(22, 'Soup Size', 'SINGLE', 1, 1, 1, 0, 1),
(23, 'Soup Toppings', 'MULTIPLE', 0, 0, 2, 0, 2),
(24, 'Dessert Size', 'SINGLE', 1, 1, 1, 0, 1),
(25, 'Dessert Toppings', 'MULTIPLE', 0, 0, 2, 0, 2),
(26, 'Vegan Ingredients', 'MULTIPLE', 0, 0, 10, 0, 0),
(27, 'Drink Options', 'MULTIPLE', 0, 0, 999, 0, 4),
(28, 'Side Options', 'MULTIPLE', 0, 0, 999, 0, 5),
(34, 'Exclude Ingredients', 'EXCLUSIONS', 0, 0, 999, 0, 2),
(35, 'Sauces', 'MULTIPLE', 0, 0, 5, 2, 4);

-- Insert selection_options data
INSERT INTO selection_options (id, selection_group_id, name, price, display_order) VALUES
(35, 1, 'Small', '0.0', 1),
(36, 1, 'Medium', '2.0', 2),
(37, 1, 'Large', '4.0', 3),
(38, 2, 'Regular', '0.0', 1),
(39, 2, 'Thin', '1.0', 2),
(40, 2, 'Thick', '1.0', 3),
(41, 3, 'Extra Cheese', '1.5', 1),
(42, 3, 'Pepperoni', '2.0', 2),
(43, 3, 'Mushrooms', '1.5', 3),
(44, 3, 'Onions', '1.0', 4),
(45, 3, 'Bell Peppers', '1.5', 5),
(46, 4, 'Small', '0.0', 1),
(47, 4, 'Medium', '1.0', 2),
(48, 4, 'Large', '2.0', 3),
(49, 5, 'No Ice', '0.0', 1),
(50, 5, 'Light Ice', '0.0', 2),
(51, 5, 'Regular Ice', '0.0', 3),
(52, 6, 'Cola', '1', 1),
(53, 6, 'Fanta', '2', 2),
(54, 7, 'French Fries', '2', 1),
(55, 7, 'Onion Rings', '3', 2),
(56, 7, 'Salad', '4', 3),
(57, 8, 'Small', '0.0', 1),
(58, 8, 'Medium', '2.0', 2),
(59, 8, 'Large', '4.0', 3),
(60, 9, 'Brown Rice', '0.0', 1),
(61, 9, 'Quinoa', '1.0', 2),
(62, 9, 'Mixed Greens', '0.0', 3),
(63, 10, 'Grilled Chicken', '2.0', 1),
(64, 10, 'Tofu', '1.0', 2),
(65, 10, 'Salmon', '3.0', 3),
(66, 11, 'Avocado', '1.5', 1),
(67, 11, 'Roasted Vegetables', '1.0', 2),
(68, 11, 'Edamame', '1.0', 3),
(69, 11, 'Corn', '0.5', 4),
(70, 12, 'Teriyaki', '0.0', 1),
(71, 12, 'Miso', '0.0', 2),
(72, 12, 'Spicy Mayo', '0.0', 3),
(73, 13, 'Regular', '0.0', 1),
(74, 13, 'Double', '4.0', 2),
(75, 14, 'Lettuce', '0.0', 1),
(76, 14, 'Tomato', '0.0', 2),
(77, 14, 'Onion', '0.0', 3),
(78, 14, 'Pickles', '0.0', 4),
(79, 14, 'Cheese', '1.0', 5),
(80, 15, 'Medium Rare', '0.0', 1),
(81, 15, 'Medium', '0.0', 2),
(82, 15, 'Well Done', '0.0', 3),
(83, 16, 'Small', '0.0', 1),
(84, 16, 'Medium', '2.0', 2),
(85, 16, 'Large', '4.0', 3),
(86, 17, 'Croutons', '0.5', 1),
(87, 17, 'Bacon Bits', '1.0', 2),
(88, 17, 'Extra Cheese', '1.0', 3),
(89, 18, 'Caesar', '0.0', 1),
(90, 18, 'Ranch', '0.0', 2),
(91, 18, 'Balsamic', '0.0', 3),
(92, 19, 'Hash Browns', '2.0', 1),
(93, 19, 'Fresh Fruit', '2.0', 2),
(94, 20, 'Small', '0.0', 1),
(95, 20, 'Medium', '1.0', 2),
(96, 20, 'Large', '2.0', 3),
(97, 21, 'Extra Ice', '0.0', 1),
(98, 21, 'Lemon', '0.0', 2),
(99, 22, 'Small', '0.0', 1),
(100, 22, 'Medium', '2.0', 2),
(101, 22, 'Large', '4.0', 3),
(102, 23, 'Croutons', '0.5', 1),
(103, 23, 'Cheese', '1.0', 2),
(104, 24, 'Regular', '0.0', 1),
(105, 24, 'Large', '2.0', 2),
(106, 25, 'Whipped Cream', '0.5', 1),
(107, 25, 'Chocolate Sauce', '0.5', 2),
(108, 26, 'Tofu', '2.0', 1),
(109, 26, 'Tempeh', '2.5', 2),
(110, 26, 'Seitan', '3.0', 3),
(111, 26, 'Vegan Cheese', '1.5', 4),
(112, 26, 'Mushrooms', '1.0', 5),
(113, 26, 'Bell Peppers', '0.5', 6),
(114, 26, 'Onions', '0.5', 7),
(115, 26, 'Olives', '1.0', 8),
(116, 26, 'Spinach', '1.0', 9),
(117, 26, 'Artichoke Hearts', '2.0', 10),
(143, 34, 'Mushrooms', '0', 1),
(144, 34, 'Peppers', '0', 2),
(145, 34, 'Onions', '0', 3),
(146, 35, 'BBQ Sauce', '0.5', 1),
(147, 35, 'Garlic Sauce', '0.5', 2),
(148, 35, 'Hot Sauce', '0.5', 3),
(149, 35, 'Sweet Chili Sauce', '0.5', 4),
(150, 35, 'Ranch Sauce', '0.5', 5);

-- Insert category_selection_groups data
INSERT INTO category_selection_groups (id, category_id, selection_group_id, display_order) VALUES
(11, 1, 1, 10),
(12, 1, 2, 20),
(13, 1, 3, 30),
(44, 1, 35, 4);

-- Insert postcodes data
INSERT INTO postcodes (Id, code, district) VALUES
(1, '1200', 'Brigittenau'),
(2, '1210', 'Floridsdorf');

-- Insert postcode_addresses data
INSERT INTO postcode_addresses (Id, postcode_id, street) VALUES
(1, 1, 'Adalbert-Stifter-Straße'),
(2, 1, 'Wallensteinstraße'),
(3, 1, 'Leipziger Straße'),
(4, 1, 'Klosterneuburger Straße'),
(5, 1, 'Raffaelgasse'),
(6, 2, 'Prager Straße'),
(7, 2, 'Brünner Straße'),
(8, 2, 'Donaufelder Straße'),
(9, 2, 'Angyalföldstraße'),
(10, 2, 'Floridusgasse');

-- Insert delivery_addresses data
INSERT INTO delivery_addresses (Id, postcode_id, street, house, stairs, stick, door, bell) VALUES
(1, 1, 'Adalbert-Stifter-Straße', NULL, NULL, NULL, NULL, NULL),
(2, 1, 'Wallensteinstraße', NULL, NULL, NULL, NULL, NULL),
(3, 1, 'Leipziger Straße', NULL, NULL, NULL, NULL, NULL),
(4, 1, 'Klosterneuburger Straße', NULL, NULL, NULL, NULL, NULL),
(5, 1, 'Raffaelgasse', NULL, NULL, NULL, NULL, NULL),
(6, 2, 'Prager Straße', NULL, NULL, NULL, NULL, NULL),
(7, 2, 'Brünner Straße', NULL, NULL, NULL, NULL, NULL),
(8, 2, 'Donaufelder Straße', NULL, NULL, NULL, NULL, NULL),
(9, 2, 'Angyalföldstraße', NULL, NULL, NULL, NULL, NULL),
(10, 2, 'Floridusgasse', NULL, NULL, NULL, NULL, NULL);

-- Insert PostcodeMinimumOrders data
INSERT INTO PostcodeMinimumOrders (Id, Postcode, MinimumOrderValue) VALUES
(1, '1200', '10.0'),
(2, '1210', '20.0');

-- Insert items data
INSERT INTO items (id, category_id, description, image_url, name, price) VALUES
(1, 6, 'Classic carbonated soft drink', NULL, 'Cola', 4),
(2, 4, 'Salad with fresh vegetables and cheese', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/salad.jpg', 'Salad', 8),
(3, 1, 'A delightful plant-based pizza with a variety of fresh toppings', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/veganpizza.jpg', 'Vegan Pizza', 12),
(4, 2, 'Filled with fresh ingredients, perfect for a healthy and satisfying meal.', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/bowll.jpg', 'Bowl', 10),
(5, 6, 'Pure, crisp, and refreshing still water', NULL, 'Water', 1),
(6, 6, 'Traditional Turkish yogurt-based drink, cool and refreshing with a creamy texture', NULL, 'Ayran', 2),
(7, 6, 'A light, refreshing lemon-lime soda with a crisp, bubbly taste to quench your thirst', NULL, 'Sprite', 3),
(8, 1, 'Classic pizza with rich tomato sauce, fresh mozzarella, and basil', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/margarita.jpg', 'Margarita Pizza', 9),
(9, 6, 'A premium Turkish soda, light and refreshing, perfect for pairing with your meal.', NULL, 'Soda', 2),
(10, 3, 'A delicious burger made with a juicy beef patty, crispy bacon, melted cheese, and fresh onions, all nestled in a soft bun', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/baconburger.jpg', 'Baconburger', 6),
(11, 3, 'Classic American cheeseburger', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/cheeseburger.jpg', 'Cheeseburger', 7),
(12, 4, 'A healthy salad with grilled chicken, fresh veggies, and dressing', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/chickensalad.jpg', 'Chicken Salad', 9),
(13, 1, 'A savory pizza topped with grilled chicken and fresh vegetables', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/chickenpizza.jpg', 'Chicken Pizza', 10),
(14, 1, 'A flavorful pizza with spicy sausage and cheese', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/sausagepizza.jpg', 'Sausage Pizza', 10),
(15, 5, 'A delicious breakfast with bacon, eggs, and fresh vegetables', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/baconbreakfast.jpg', 'Bacon Breakfast Menu', 12),
(16, 7, 'Spicy, sweet, and fragrant with lemongrass, lime, and tender chicken', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/thaisoup.jpg', 'Thai Coconut Chicken Soup', 7),
(17, 7, 'Rich, earthy, and velvety with sautéed mushrooms and a hint of garlic', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/mushroomsoup.jpg', 'Creamy Wild Mushroom Soup', 6),
(18, 7, 'Smooth, tangy, and topped with a swirl of cream and fresh basil', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/tomatosoup.jpg', 'Roasted Tomato Basil Soup', 6),
(19, 8, 'Warm, gooey center served with a scoop of vanilla ice cream', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/moltendessert.jpg', 'Molten Chocolate Lava Cake', 5),
(20, 8, 'Zesty lemon curd topped with pillowy toasted meringue in a buttery crust', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/lemontartdessert.jpg', 'Lemon Meringue Tart', 5),
(21, 8, 'Layers of fluffy cake, whipped cream, and fresh strawberries', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/strawberrydessert.jpg', 'Strawberry Shortcake Parfait', 6),
(22, 0, 'A delightful plant-based pizza with a variety of fresh toppings', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/veganpizza.jpg', 'Vegan Pizza (Promo)', 10),
(23, 0, 'Classic pizza with rich tomato sauce, fresh mozzarella, and basil', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/margarita.jpg', 'Margarita Pizza (Promo)', 7),
(24, 0, 'A savory pizza topped with grilled chicken and fresh vegetables', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/chickenpizza.jpg', 'Chicken Pizza (Promo)', 8),
(25, 0, 'A flavorful pizza with spicy sausage and cheese', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/sausagepizza.jpg', 'Sausage Pizza (Promo)', 8),
(26, 0, 'Large pizza with 3 toppings of your choice', NULL, 'Special Pizza Deal', 20),
(27, 0, '2 medium pizzas with 2 toppings each', NULL, 'Family Pizza Pack', 20),
(28, 0, 'Juicy beef burger with fries', NULL, 'Classic Burger Combo', 15),
(29, 0, 'Double beef patty with cheese and special sauce', NULL, 'Double Cheeseburger Special', 15),
(47, 1, 'Customize your perfect pizza with your favorite toppings', 'https://restaurant-images33.s3.eu-north-1.amazonaws.com/sausagepizza.jpg', 'Create Your Pizza', 12.99);

-- Insert item_selection_groups data
INSERT INTO item_selection_groups (id, item_id, selection_group_id, display_order) VALUES
(1, 3, 26, 100),
(2, 3, 6, 110),
(3, 3, 7, 120),
(10, 3, 34, 2);

-- Insert item_allergens data
INSERT INTO item_allergens (id, item_id, allergen_code) VALUES
(1, 3, 'G'),
(2, 3, 'S'),
(3, 4, 'S'),
(4, 6, 'L'),
(5, 8, 'G'),
(6, 8, 'L'),
(7, 10, 'G'),
(8, 10, 'L'),
(9, 10, 'E'),
(10, 13, 'G'),
(11, 13, 'L'),
(12, 19, 'L'),
(13, 19, 'E'),
(14, 14, 'G');

-- Insert offers data
INSERT INTO offers (Id, name, description, discount_percentage, start_date, end_date, is_active) VALUES
(1, 'Happy Hour', '20% off on all pizzas', '20.0', '2025-04-21 14:32:26.987483', '2025-05-21 14:32:26.987536', 1),
(2, 'Student Discount', '15% off with student ID', '15.0', '2025-04-21 14:32:26.987655', '2025-07-20 14:32:26.987655', 1);

-- Insert item_offers data
INSERT INTO item_offers (Id, item_id, offer_id) VALUES
(1, 3, 1),
(2, 8, 1),
(3, 13, 1),
(4, 14, 1);

-- Insert promotions data
INSERT INTO promotions (id, title, description, start_date, end_date, is_active, item_id, display_name, display_price, is_bundle, discount_percentage) VALUES
(2, 'Summer Special', 'Get 25% off on all pizzas', '2025-05-01T00:00:00Z', '2025-08-31T23:59:59Z', 1, 3, 'Summer Pizza Deal', 12.99, 0, 25);

-- Insert coupons data
INSERT INTO coupons (id, code, type, is_periodic, start_date, end_date, email, is_used, created_at, discount_ratio) VALUES
(1, 'SUMMER2025', 'PERIODIC', 1, '2025-05-01 00:00:00', '2025-07-01 23:59:59', NULL, 0, '2025-05-01 00:00:00', 0.25),
(2, 'WELCOME10', 'ONE_TIME', 0, NULL, NULL, NULL, 0, '2025-05-01 00:00:00', 0.5);

-- Insert coupon_schedule data
INSERT INTO coupon_schedule (id, coupon_id, monday, tuesday, wednesday, thursday, friday, saturday, sunday, begin_time, end_time, created_at, updated_at) VALUES
(1, 1, 1, 1, 1, 1, 1, 1, 1, '11:00:00', '23:00:00', '2025-05-29 18:31:00', '2025-05-29 18:31:00'),
(2, 2, 0, 0, 0, 0, 0, 1, 1, '12:00:00', '22:00:00', '2025-05-29 18:31:00', '2025-05-29 18:31:00');

-- Insert coupons_history data
INSERT INTO coupons_history (id, coupon_id, email, used_at) VALUES
(1, 1, NULL, '2025-05-01 00:00:00'),
(2, 2, NULL, '2025-05-01 00:00:00');

-- Insert customerOrder_info data
INSERT INTO customerOrder_info (Id, first_name, last_name, email, phone, postal_code, street, house, stairs, stick, door, bell, comment, create_date) VALUES
(1, 'John', 'Doe', 'john.doe@example.com', '555-1234', '1200', 'Adalbert-Stifter-Straße', NULL, NULL, NULL, NULL, NULL, NULL, '2025-05-01 10:00:00'),
(2, 'Jane', 'Doe', 'jane.doe@example.com', '555-5678', '1210', 'Prager Straße', NULL, NULL, NULL, NULL, NULL, NULL, '2025-05-01 11:00:00');

-- Insert orders data
INSERT INTO orders (id, created_at, customer_info_id, order_method, order_number, payment_method, special_notes, status, stripe_session_id, total, updated_at, user_id) VALUES
(1, '2025-05-01 10:00:00', 1, 'Online', 'ORD001', 'Credit Card', NULL, 'Completed', NULL, '20.00', '2025-05-01 10:00:00', NULL),
(2, '2025-05-01 11:00:00', 2, 'In-Store', 'ORD002', 'Cash', 'No onions', 'Completed', NULL, '15.00', '2025-05-01 11:00:00', NULL);

-- Insert order_details data
INSERT INTO order_details (id, order_id, item_details) VALUES
(1, 1, '{"item_id": 3, "quantity": 2}'),
(2, 2, '{"item_id": 10, "quantity": 1}'); 