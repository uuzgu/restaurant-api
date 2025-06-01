PRAGMA foreign_keys=OFF;
BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "__EFMigrationsLock" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK___EFMigrationsLock" PRIMARY KEY,
    "Timestamp" TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);
INSERT INTO __EFMigrationsHistory VALUES('20250510105859_InitialCreate','9.0.2');
INSERT INTO __EFMigrationsHistory VALUES('20250510112904_FixItemConfiguration','9.0.2');
INSERT INTO __EFMigrationsHistory VALUES('20250510113251_SeedData','9.0.2');
INSERT INTO __EFMigrationsHistory VALUES('20250510121811_ConfigureCustomerInfoAndAddress','9.0.2');
INSERT INTO __EFMigrationsHistory VALUES('20250510123540_AddStripeSessionId','9.0.2');
INSERT INTO __EFMigrationsHistory VALUES('20250510124049_AddCustomerInfoIdToOrder','9.0.2');
INSERT INTO __EFMigrationsHistory VALUES('20250510124124_MakeCustomerInfoIdNullable','9.0.2');
INSERT INTO __EFMigrationsHistory VALUES('20250510200641_AddPromotionsTables','9.0.2');
INSERT INTO __EFMigrationsHistory VALUES('20250510202158_UpdatePromotionModels','9.0.2');
INSERT INTO __EFMigrationsHistory VALUES('20250510202411_UpdatePromotionColumnNames','9.0.2');
INSERT INTO __EFMigrationsHistory VALUES('20250510202747_UpdatePromotionColumnCases','9.0.2');
INSERT INTO __EFMigrationsHistory VALUES('20250510214406_DropPromotionItemsTable','9.0.2');
INSERT INTO __EFMigrationsHistory VALUES('20250510220034_CreatePromotionsTable','9.0.2');
INSERT INTO __EFMigrationsHistory VALUES('20250510220948_AddPromotionsTable','9.0.2');
INSERT INTO __EFMigrationsHistory VALUES('20250513193927_AddSelectionGroups','9.0.2');
INSERT INTO __EFMigrationsHistory VALUES('20250513195941_RemoveDrinkAndSideOptions','9.0.2');
INSERT INTO __EFMigrationsHistory VALUES('20250515202002_AddIsOptionalToSelectionGroups','8.0.4');
INSERT INTO __EFMigrationsHistory VALUES('20250515203852_RemoveIsOptionalColumn','8.0.4');
INSERT INTO __EFMigrationsHistory VALUES('20250517155550_AddPostcodesAndDeliveryAddresses','9.0.2');
INSERT INTO __EFMigrationsHistory VALUES('20250518115720_AddPostcodeAddresses','9.0.2');
INSERT INTO __EFMigrationsHistory VALUES('20250518163320_MakeStripeSessionIdNullable','9.0.2');
INSERT INTO __EFMigrationsHistory VALUES('20250518165050_AddPostcodeMinimumOrder','9.0.2');
CREATE TABLE IF NOT EXISTS "categories" (
    "id" INTEGER NOT NULL CONSTRAINT "id" PRIMARY KEY AUTOINCREMENT,
    "name" TEXT NOT NULL
);
INSERT INTO categories VALUES(0,'Promotions');
INSERT INTO categories VALUES(1,'Pizza');
INSERT INTO categories VALUES(2,'Bowls');
INSERT INTO categories VALUES(3,'Hamburgers');
INSERT INTO categories VALUES(4,'Salads');
INSERT INTO categories VALUES(5,'Breakfast');
INSERT INTO categories VALUES(6,'Drinks');
INSERT INTO categories VALUES(7,'Soups');
INSERT INTO categories VALUES(8,'Desserts');
CREATE TABLE IF NOT EXISTS "offers" (
    "Id" INTEGER NOT NULL CONSTRAINT "Id" PRIMARY KEY AUTOINCREMENT,
    "name" TEXT NOT NULL,
    "description" TEXT NULL,
    "discount_percentage" TEXT NOT NULL,
    "start_date" TEXT NOT NULL,
    "end_date" TEXT NOT NULL,
    "is_active" INTEGER NOT NULL
);
INSERT INTO offers VALUES(1,'Happy Hour','20% off on all pizzas','20.0','2025-04-21 14:32:26.987483','2025-05-21 14:32:26.987536',1);
INSERT INTO offers VALUES(2,'Student Discount','15% off with student ID','15.0','2025-04-21 14:32:26.987655','2025-07-20 14:32:26.987655',1);
CREATE TABLE IF NOT EXISTS "users" (
    "Id" INTEGER NOT NULL CONSTRAINT "Id" PRIMARY KEY AUTOINCREMENT,
    "FirstName" TEXT NOT NULL,
    "LastName" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "Password" TEXT NOT NULL,
    "Phone" TEXT NULL,
    "PostalCode" TEXT NULL,
    "Address" TEXT NULL,
    "House" TEXT NULL,
    "Stairs" TEXT NULL,
    "Door" TEXT NULL,
    "Bell" TEXT NULL,
    "Comment" TEXT NULL
);
CREATE TABLE IF NOT EXISTS "item_offers" (
    "Id" INTEGER NOT NULL CONSTRAINT "Id" PRIMARY KEY AUTOINCREMENT,
    "item_id" INTEGER NOT NULL,
    "offer_id" INTEGER NOT NULL,
    CONSTRAINT "FK_item_offers_items_item_id" FOREIGN KEY ("item_id") REFERENCES "items" ("id") ON DELETE CASCADE,
    CONSTRAINT "FK_item_offers_offers_offer_id" FOREIGN KEY ("offer_id") REFERENCES "offers" ("Id") ON DELETE CASCADE
);
INSERT INTO item_offers VALUES(1,3,1);
INSERT INTO item_offers VALUES(2,8,1);
INSERT INTO item_offers VALUES(3,13,1);
INSERT INTO item_offers VALUES(4,14,1);
CREATE TABLE IF NOT EXISTS "items" (
    "id" INTEGER NOT NULL CONSTRAINT "PK_items" PRIMARY KEY AUTOINCREMENT,
    "category_id" INTEGER NOT NULL,
    "description" TEXT NULL,
    "image_url" TEXT NULL,
    "name" TEXT NOT NULL,
    "price" INTEGER NOT NULL,
    CONSTRAINT "FK_items_categories_category_id" FOREIGN KEY ("category_id") REFERENCES "categories" ("id") ON DELETE RESTRICT
);
INSERT INTO items VALUES(1,6,'Classic carbonated soft drink',NULL,'Cola',4);
INSERT INTO items VALUES(2,4,'Salad with fresh vegetables and cheese','https://restaurant-images33.s3.eu-north-1.amazonaws.com/salad.jpg','Salad',8);
INSERT INTO items VALUES(3,1,'A delightful plant-based pizza with a variety of fresh toppings','https://restaurant-images33.s3.eu-north-1.amazonaws.com/veganpizza.jpg','Vegan Pizza',12);
INSERT INTO items VALUES(4,2,'Filled with fresh ingredients, perfect for a healthy and satisfying meal.','https://restaurant-images33.s3.eu-north-1.amazonaws.com/bowll.jpg','Bowl',10);
INSERT INTO items VALUES(5,6,'Pure, crisp, and refreshing still water',NULL,'Water',1);
INSERT INTO items VALUES(6,6,'Traditional Turkish yogurt-based drink, cool and refreshing with a creamy texture',NULL,'Ayran',2);
INSERT INTO items VALUES(7,6,'A light, refreshing lemon-lime soda with a crisp, bubbly taste to quench your thirst',NULL,'Sprite',3);
INSERT INTO items VALUES(8,1,'Classic pizza with rich tomato sauce, fresh mozzarella, and basil','https://restaurant-images33.s3.eu-north-1.amazonaws.com/margarita.jpg','Margarita Pizza',9);
INSERT INTO items VALUES(9,6,'A premium Turkish soda, light and refreshing, perfect for pairing with your meal.',NULL,'Soda',2);
INSERT INTO items VALUES(10,3,'A delicious burger made with a juicy beef patty, crispy bacon, melted cheese, and fresh onions, all nestled in a soft bun','https://restaurant-images33.s3.eu-north-1.amazonaws.com/baconburger.jpg','Baconburger',6);
INSERT INTO items VALUES(11,3,'Classic American cheeseburger','https://restaurant-images33.s3.eu-north-1.amazonaws.com/cheeseburger.jpg','Cheeseburger',7);
INSERT INTO items VALUES(12,4,'A healthy salad with grilled chicken, fresh veggies, and dressing','https://restaurant-images33.s3.eu-north-1.amazonaws.com/chickensalad.jpg','Chicken Salad',9);
INSERT INTO items VALUES(13,1,'A savory pizza topped with grilled chicken and fresh vegetables','https://restaurant-images33.s3.eu-north-1.amazonaws.com/chickenpizza.jpg','Chicken Pizza',10);
INSERT INTO items VALUES(14,1,'A flavorful pizza with spicy sausage and cheese','https://restaurant-images33.s3.eu-north-1.amazonaws.com/sausagepizza.jpg','Sausage Pizza',10);
INSERT INTO items VALUES(15,5,'A delicious breakfast with bacon, eggs, and fresh vegetables','https://restaurant-images33.s3.eu-north-1.amazonaws.com/baconbreakfast.jpg','Bacon Breakfast Menu',12);
INSERT INTO items VALUES(16,7,'Spicy, sweet, and fragrant with lemongrass, lime, and tender chicken','https://restaurant-images33.s3.eu-north-1.amazonaws.com/thaisoup.jpg','Thai Coconut Chicken Soup',7);
INSERT INTO items VALUES(17,7,'Rich, earthy, and velvety with sautéed mushrooms and a hint of garlic','https://restaurant-images33.s3.eu-north-1.amazonaws.com/mushroomsoup.jpg','Creamy Wild Mushroom Soup',6);
INSERT INTO items VALUES(18,7,'Smooth, tangy, and topped with a swirl of cream and fresh basil','https://restaurant-images33.s3.eu-north-1.amazonaws.com/tomatosoup.jpg','Roasted Tomato Basil Soup',6);
INSERT INTO items VALUES(19,8,'Warm, gooey center served with a scoop of vanilla ice cream','https://restaurant-images33.s3.eu-north-1.amazonaws.com/moltendessert.jpg','Molten Chocolate Lava Cake',5);
INSERT INTO items VALUES(20,8,'Zesty lemon curd topped with pillowy toasted meringue in a buttery crust','https://restaurant-images33.s3.eu-north-1.amazonaws.com/lemontartdessert.jpg','Lemon Meringue Tart',5);
INSERT INTO items VALUES(21,8,'Layers of fluffy cake, whipped cream, and fresh strawberries','https://restaurant-images33.s3.eu-north-1.amazonaws.com/strawberrydessert.jpg','Strawberry Shortcake Parfait',6);
INSERT INTO items VALUES(22,0,'A delightful plant-based pizza with a variety of fresh toppings','https://restaurant-images33.s3.eu-north-1.amazonaws.com/veganpizza.jpg','Vegan Pizza (Promo)',10);
INSERT INTO items VALUES(23,0,'Classic pizza with rich tomato sauce, fresh mozzarella, and basil','https://restaurant-images33.s3.eu-north-1.amazonaws.com/margarita.jpg','Margarita Pizza (Promo)',7);
INSERT INTO items VALUES(24,0,'A savory pizza topped with grilled chicken and fresh vegetables','https://restaurant-images33.s3.eu-north-1.amazonaws.com/chickenpizza.jpg','Chicken Pizza (Promo)',8);
INSERT INTO items VALUES(25,0,'A flavorful pizza with spicy sausage and cheese','https://restaurant-images33.s3.eu-north-1.amazonaws.com/sausagepizza.jpg','Sausage Pizza (Promo)',8);
INSERT INTO items VALUES(26,0,'Large pizza with 3 toppings of your choice',NULL,'Special Pizza Deal',20);
INSERT INTO items VALUES(27,0,'2 medium pizzas with 2 toppings each',NULL,'Family Pizza Pack',20);
INSERT INTO items VALUES(28,0,'Juicy beef burger with fries',NULL,'Classic Burger Combo',15);
INSERT INTO items VALUES(29,0,'Double beef patty with cheese and special sauce',NULL,'Double Cheeseburger Special',15);
INSERT INTO items VALUES(47,1,'Customize your perfect pizza with your favorite toppings','https://restaurant-images33.s3.eu-north-1.amazonaws.com/sausagepizza.jpg','Create Your Pizza',12.989999999999999324);
CREATE TABLE IF NOT EXISTS "customerOrder_info" (
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
    "comment" TEXT NULL
, create_date DATETIME);
INSERT INTO customerOrder_info VALUES(1,'uuu','ttt','cevvalu@gmail.com','5553335555','11','vien','123','','1','1','1',NULL,NULL);
INSERT INTO customerOrder_info VALUES(2,'uuu','ttt','cevvalu@gmail.com','5553335555','11','vien','123','','1','1','1',NULL,NULL);
INSERT INTO customerOrder_info VALUES(3,'uuu','ttt','cevvalu@gmail.com','5553335555','11','vien','123','','1','1','1',NULL,NULL);
INSERT INTO customerOrder_info VALUES(4,'uuu','ttt','cevvalu@gmail.com','5553335555','11','vien','123','','1','1','1',NULL,NULL);
INSERT INTO customerOrder_info VALUES(5,'uuu','ttt','cevvalu@gmail.com','5553335555','11','vien','123','','1','1','1',NULL,NULL);
INSERT INTO customerOrder_info VALUES(6,'uuu','ttt','cevvalu@gmail.com','5553335555','11','vien','123','','1','1','1',NULL,NULL);
INSERT INTO customerOrder_info VALUES(7,'John','Doe','john@example.com','+1234567890','1234','Test Street','1','1','1','1','1',NULL,NULL);
INSERT INTO customerOrder_info VALUES(8,'John','Doe','john@example.com','+1234567890','1234','Test Street','1','1','1','1','1',NULL,NULL);
INSERT INTO customerOrder_info VALUES(9,'ahmet','memet','cevvalu@gmail.com','5334565656','1200','Wallensteinstraße','44',NULL,NULL,'4',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(10,'ahmet','memet','cevvalu@gmail.com','5334565656','1200','Adalbert-Stifter-Straße','5',NULL,NULL,'5',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(11,'ahmet','memet','cevvalu@gmail.com','5334565656','1210','Brünner Straße','4',NULL,NULL,'4',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(12,'ahmet','memet','cevvalu@gmail.com','5334565656','1200','Adalbert-Stifter-Straße','5',NULL,NULL,'5',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(13,'utku','cevval','cevvalu@gmail.com','5553335555','1200','Adalbert-Stifter-Straße','5',NULL,NULL,'5',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(14,'utku','cevval','cevvalu@gmail.com','5553335555','1210','Brünner Straße','4',NULL,NULL,'4',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(15,'utku','cevval','cevvalu@gmail.com','5553335555','1200','Adalbert-Stifter-Straße','4',NULL,NULL,'4',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(16,'utku','cevval','cevvalu@gmail.com','5553335555','1210','Donaufelder Straße','5',NULL,NULL,'5',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(17,'utku','cevval','cevvalu@gmail.com','5553335555','1210','Prager Straße','5',NULL,NULL,'6',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(18,'utku','cevval','cevvalu@gmail.com','5553335555','1200','Wallensteinstraße','4','4',NULL,'4',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(19,'utku','cevval','cevvalu@gmail.com','5553335555','1200','Wallensteinstraße','4',NULL,NULL,'4',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(20,'utku','cevval','cevvalu@gmail.com','5553335555','1210','Wallensteinstraße','5',NULL,NULL,'5',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(21,'utku','cevval','cevvalu@gmail.com','5553335555','1210','Wallensteinstraße','5',NULL,NULL,'5',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(22,'utku','cevval','cevvalu@gmail.com','5553335555','1210','Wallensteinstraße','5',NULL,NULL,'5',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(23,'utku','cevval','cevvalu@gmail.com','5553335555','1210','Wallensteinstraße','5',NULL,NULL,'5',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(24,'utku','cevval','cevvalu@gmail.com','5553335555','1210','Wallensteinstraße','5',NULL,NULL,'5',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(25,'utku','cevval','cevvalu@gmail.com','5553335555','1210','Wallensteinstraße','5',NULL,NULL,'5',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(26,'utku','cevval','cevvalu@gmail.com','5553335555','1210','Wallensteinstraße','5',NULL,NULL,'5',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(27,'utku','cevval','cevvalu@gmail.com','5553335555','1200','Adalbert-Stifter-Straße','5',NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(28,'utku','cevval','cevvalu@gmail.com','5553335555','1200','Adalbert-Stifter-Straße','5',NULL,NULL,'5',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(29,'utku','cevval','cevvalu@gmail.com','5553335555','1200','Wallensteinstraße','4',NULL,NULL,'4',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(30,'utku','cevval','cevvalu@gmail.com','5553335555','1200','Adalbert-Stifter-Straße','4',NULL,NULL,'4',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(31,'utku','cevval','cevvalu@gmail.com','5553335555','1200','Wallensteinstraße','3',NULL,NULL,'3',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(32,'utku','cevval','cevvalu@gmail.com','5553335555','1200','Wallensteinstraße','4',NULL,NULL,'4',NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(33,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(34,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(35,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(36,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(37,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(38,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(39,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(40,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(41,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(42,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(43,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(44,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(45,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(46,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(47,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(48,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(49,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO customerOrder_info VALUES(50,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2025-06-01 10:50:03.516975');
INSERT INTO customerOrder_info VALUES(51,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2025-06-01 10:50:30.568451');
INSERT INTO customerOrder_info VALUES(52,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2025-06-01 10:50:40.731763');
INSERT INTO customerOrder_info VALUES(53,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2025-06-01 10:52:41.194805');
INSERT INTO customerOrder_info VALUES(54,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2025-06-01 11:01:12.123666');
INSERT INTO customerOrder_info VALUES(55,'utku','cevval','cevvalu@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2025-06-01 11:06:29.234078');
INSERT INTO customerOrder_info VALUES(56,'test12','cevval','test12@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2025-06-01 11:21:26.38726');
INSERT INTO customerOrder_info VALUES(57,'test12','cevval','test12@gmail.com','5553335555',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2025-06-01 11:21:38.46665');
CREATE TABLE promotions (id INTEGER PRIMARY KEY AUTOINCREMENT, title TEXT NOT NULL, description TEXT, start_date TEXT NOT NULL, end_date TEXT NOT NULL, is_active INTEGER NOT NULL, item_id INTEGER, display_name TEXT NOT NULL, display_price REAL NOT NULL, is_bundle INTEGER NOT NULL, discount_percentage REAL, FOREIGN KEY (item_id) REFERENCES items(id) ON DELETE SET NULL);
INSERT INTO promotions VALUES(2,'Summer Special','Get 25% off on all pizzas','2025-05-01T00:00:00Z','2025-08-31T23:59:59Z',1,3,'Summer Pizza Deal',12.989999999999999324,0,25.0);
CREATE TABLE item_allergens (id INTEGER PRIMARY KEY AUTOINCREMENT, item_id INTEGER NOT NULL, allergen_code TEXT NOT NULL, FOREIGN KEY (item_id) REFERENCES items(id) ON DELETE CASCADE);
INSERT INTO item_allergens VALUES(1,3,'G');
INSERT INTO item_allergens VALUES(2,3,'S');
INSERT INTO item_allergens VALUES(3,4,'S');
INSERT INTO item_allergens VALUES(4,6,'L');
INSERT INTO item_allergens VALUES(5,8,'G');
INSERT INTO item_allergens VALUES(6,8,'L');
INSERT INTO item_allergens VALUES(7,10,'G');
INSERT INTO item_allergens VALUES(8,10,'L');
INSERT INTO item_allergens VALUES(9,10,'E');
INSERT INTO item_allergens VALUES(10,13,'G');
INSERT INTO item_allergens VALUES(11,13,'L');
INSERT INTO item_allergens VALUES(12,19,'L');
INSERT INTO item_allergens VALUES(13,19,'E');
INSERT INTO item_allergens VALUES(14,14,'G');
CREATE TABLE IF NOT EXISTS "selection_groups" (
    "id" INTEGER NOT NULL CONSTRAINT "PK_selection_groups" PRIMARY KEY AUTOINCREMENT,
    "name" TEXT NOT NULL,
    "type" TEXT NOT NULL,
    "is_required" INTEGER NOT NULL,
    "min_select" INTEGER NOT NULL,
    "max_select" INTEGER NULL,
    "threshold" INTEGER NOT NULL,
    "display_order" INTEGER NOT NULL
);
INSERT INTO selection_groups VALUES(1,'Pizza Size','SINGLE',1,1,1,0,1);
INSERT INTO selection_groups VALUES(2,'Crust Type','SINGLE',1,1,1,0,2);
INSERT INTO selection_groups VALUES(3,'Extra Toppings','MULTIPLE',0,0,5,0,3);
INSERT INTO selection_groups VALUES(4,'Drink Size','SINGLE',1,1,1,0,1);
INSERT INTO selection_groups VALUES(5,'Ice Options','SINGLE',0,0,1,0,2);
INSERT INTO selection_groups VALUES(6,'Drink Options','MULTIPLE',1,1,5,0,4);
INSERT INTO selection_groups VALUES(7,'Side Options','MULTIPLE',0,0,3,0,5);
INSERT INTO selection_groups VALUES(8,'Bowl Size','SINGLE',1,1,1,0,1);
INSERT INTO selection_groups VALUES(9,'Base Choice','SINGLE',1,1,1,0,2);
INSERT INTO selection_groups VALUES(10,'Protein Choice','SINGLE',1,1,1,0,3);
INSERT INTO selection_groups VALUES(11,'Vegetable Toppings','MULTIPLE',0,0,4,0,4);
INSERT INTO selection_groups VALUES(12,'Sauce Choice','SINGLE',1,1,1,0,5);
INSERT INTO selection_groups VALUES(13,'Burger Size','SINGLE',1,1,1,0,1);
INSERT INTO selection_groups VALUES(14,'Burger Toppings','MULTIPLE',0,0,5,0,2);
INSERT INTO selection_groups VALUES(15,'Cooking Preference','SINGLE',1,1,1,0,3);
INSERT INTO selection_groups VALUES(16,'Salad Size','SINGLE',1,1,1,0,1);
INSERT INTO selection_groups VALUES(17,'Salad Add-ons','MULTIPLE',0,0,3,0,2);
INSERT INTO selection_groups VALUES(18,'Dressing Choice','SINGLE',1,1,1,0,3);
INSERT INTO selection_groups VALUES(19,'Breakfast Sides','MULTIPLE',0,0,2,0,1);
INSERT INTO selection_groups VALUES(20,'Drink Size','SINGLE',1,1,1,0,1);
INSERT INTO selection_groups VALUES(21,'Drink Add-ons','MULTIPLE',0,0,2,0,2);
INSERT INTO selection_groups VALUES(22,'Soup Size','SINGLE',1,1,1,0,1);
INSERT INTO selection_groups VALUES(23,'Soup Toppings','MULTIPLE',0,0,2,0,2);
INSERT INTO selection_groups VALUES(24,'Dessert Size','SINGLE',1,1,1,0,1);
INSERT INTO selection_groups VALUES(25,'Dessert Toppings','MULTIPLE',0,0,2,0,2);
INSERT INTO selection_groups VALUES(26,'Vegan Ingredients','MULTIPLE',0,0,10,0,0);
INSERT INTO selection_groups VALUES(27,'Drink Options','MULTIPLE',0,0,999,0,4);
INSERT INTO selection_groups VALUES(28,'Side Options','MULTIPLE',0,0,999,0,5);
INSERT INTO selection_groups VALUES(34,'Exclude Ingredients','EXCLUSIONS',0,0,999,0,2);
INSERT INTO selection_groups VALUES(35,'Sauces','MULTIPLE',0,0,5,2,4);
CREATE TABLE IF NOT EXISTS "category_selection_groups" (
    "id" INTEGER NOT NULL CONSTRAINT "PK_category_selection_groups" PRIMARY KEY AUTOINCREMENT,
    "category_id" INTEGER NOT NULL,
    "selection_group_id" INTEGER NOT NULL,
    "display_order" INTEGER NOT NULL,
    CONSTRAINT "FK_category_selection_groups_categories_category_id" FOREIGN KEY ("category_id") REFERENCES "categories" ("id") ON DELETE CASCADE,
    CONSTRAINT "FK_category_selection_groups_selection_groups_selection_group_id" FOREIGN KEY ("selection_group_id") REFERENCES "selection_groups" ("id") ON DELETE RESTRICT
);
INSERT INTO category_selection_groups VALUES(11,1,1,10);
INSERT INTO category_selection_groups VALUES(12,1,2,20);
INSERT INTO category_selection_groups VALUES(13,1,3,30);
INSERT INTO category_selection_groups VALUES(44,1,35,4);
CREATE TABLE IF NOT EXISTS "item_selection_groups" (
    "id" INTEGER NOT NULL CONSTRAINT "PK_item_selection_groups" PRIMARY KEY AUTOINCREMENT,
    "item_id" INTEGER NOT NULL,
    "selection_group_id" INTEGER NOT NULL,
    "display_order" INTEGER NOT NULL,
    CONSTRAINT "FK_item_selection_groups_items_item_id" FOREIGN KEY ("item_id") REFERENCES "items" ("id") ON DELETE CASCADE,
    CONSTRAINT "FK_item_selection_groups_selection_groups_selection_group_id" FOREIGN KEY ("selection_group_id") REFERENCES "selection_groups" ("id") ON DELETE RESTRICT
);
INSERT INTO item_selection_groups VALUES(1,3,26,100);
INSERT INTO item_selection_groups VALUES(2,3,6,110);
INSERT INTO item_selection_groups VALUES(3,3,7,120);
INSERT INTO item_selection_groups VALUES(10,3,34,2);
CREATE TABLE IF NOT EXISTS "selection_options" (
    "id" INTEGER NOT NULL CONSTRAINT "PK_selection_options" PRIMARY KEY AUTOINCREMENT,
    "selection_group_id" INTEGER NOT NULL,
    "name" TEXT NOT NULL,
    "price" TEXT NOT NULL,
    "display_order" INTEGER NOT NULL,
    CONSTRAINT "FK_selection_options_selection_groups_selection_group_id" FOREIGN KEY ("selection_group_id") REFERENCES "selection_groups" ("id") ON DELETE CASCADE
);
INSERT INTO selection_options VALUES(35,1,'Small','0.0',1);
INSERT INTO selection_options VALUES(36,1,'Medium','2.0',2);
INSERT INTO selection_options VALUES(37,1,'Large','4.0',3);
INSERT INTO selection_options VALUES(38,2,'Regular','0.0',1);
INSERT INTO selection_options VALUES(39,2,'Thin','1.0',2);
INSERT INTO selection_options VALUES(40,2,'Thick','1.0',3);
INSERT INTO selection_options VALUES(41,3,'Extra Cheese','1.5',1);
INSERT INTO selection_options VALUES(42,3,'Pepperoni','2.0',2);
INSERT INTO selection_options VALUES(43,3,'Mushrooms','1.5',3);
INSERT INTO selection_options VALUES(44,3,'Onions','1.0',4);
INSERT INTO selection_options VALUES(45,3,'Bell Peppers','1.5',5);
INSERT INTO selection_options VALUES(46,4,'Small','0.0',1);
INSERT INTO selection_options VALUES(47,4,'Medium','1.0',2);
INSERT INTO selection_options VALUES(48,4,'Large','2.0',3);
INSERT INTO selection_options VALUES(49,5,'No Ice','0.0',1);
INSERT INTO selection_options VALUES(50,5,'Light Ice','0.0',2);
INSERT INTO selection_options VALUES(51,5,'Regular Ice','0.0',3);
INSERT INTO selection_options VALUES(52,6,'Cola','1',1);
INSERT INTO selection_options VALUES(53,6,'Fanta','2',2);
INSERT INTO selection_options VALUES(54,7,'French Fries','2',1);
INSERT INTO selection_options VALUES(55,7,'Onion Rings','3',2);
INSERT INTO selection_options VALUES(56,7,'Salad','4',3);
INSERT INTO selection_options VALUES(57,8,'Small','0.0',1);
INSERT INTO selection_options VALUES(58,8,'Medium','2.0',2);
INSERT INTO selection_options VALUES(59,8,'Large','4.0',3);
INSERT INTO selection_options VALUES(60,9,'Brown Rice','0.0',1);
INSERT INTO selection_options VALUES(61,9,'Quinoa','1.0',2);
INSERT INTO selection_options VALUES(62,9,'Mixed Greens','0.0',3);
INSERT INTO selection_options VALUES(63,10,'Grilled Chicken','2.0',1);
INSERT INTO selection_options VALUES(64,10,'Tofu','1.0',2);
INSERT INTO selection_options VALUES(65,10,'Salmon','3.0',3);
INSERT INTO selection_options VALUES(66,11,'Avocado','1.5',1);
INSERT INTO selection_options VALUES(67,11,'Roasted Vegetables','1.0',2);
INSERT INTO selection_options VALUES(68,11,'Edamame','1.0',3);
INSERT INTO selection_options VALUES(69,11,'Corn','0.5',4);
INSERT INTO selection_options VALUES(70,12,'Teriyaki','0.0',1);
INSERT INTO selection_options VALUES(71,12,'Miso','0.0',2);
INSERT INTO selection_options VALUES(72,12,'Spicy Mayo','0.0',3);
INSERT INTO selection_options VALUES(73,13,'Regular','0.0',1);
INSERT INTO selection_options VALUES(74,13,'Double','4.0',2);
INSERT INTO selection_options VALUES(75,14,'Lettuce','0.0',1);
INSERT INTO selection_options VALUES(76,14,'Tomato','0.0',2);
INSERT INTO selection_options VALUES(77,14,'Onion','0.0',3);
INSERT INTO selection_options VALUES(78,14,'Pickles','0.0',4);
INSERT INTO selection_options VALUES(79,14,'Cheese','1.0',5);
INSERT INTO selection_options VALUES(80,15,'Medium Rare','0.0',1);
INSERT INTO selection_options VALUES(81,15,'Medium','0.0',2);
INSERT INTO selection_options VALUES(82,15,'Well Done','0.0',3);
INSERT INTO selection_options VALUES(83,16,'Small','0.0',1);
INSERT INTO selection_options VALUES(84,16,'Medium','2.0',2);
INSERT INTO selection_options VALUES(85,16,'Large','4.0',3);
INSERT INTO selection_options VALUES(86,17,'Croutons','0.5',1);
INSERT INTO selection_options VALUES(87,17,'Bacon Bits','1.0',2);
INSERT INTO selection_options VALUES(88,17,'Extra Cheese','1.0',3);
INSERT INTO selection_options VALUES(89,18,'Caesar','0.0',1);
INSERT INTO selection_options VALUES(90,18,'Ranch','0.0',2);
INSERT INTO selection_options VALUES(91,18,'Balsamic','0.0',3);
INSERT INTO selection_options VALUES(92,19,'Hash Browns','2.0',1);
INSERT INTO selection_options VALUES(93,19,'Fresh Fruit','2.0',2);
INSERT INTO selection_options VALUES(94,20,'Small','0.0',1);
INSERT INTO selection_options VALUES(95,20,'Medium','1.0',2);
INSERT INTO selection_options VALUES(96,20,'Large','2.0',3);
INSERT INTO selection_options VALUES(97,21,'Extra Ice','0.0',1);
INSERT INTO selection_options VALUES(98,21,'Lemon','0.0',2);
INSERT INTO selection_options VALUES(99,22,'Small','0.0',1);
INSERT INTO selection_options VALUES(100,22,'Medium','2.0',2);
INSERT INTO selection_options VALUES(101,22,'Large','4.0',3);
INSERT INTO selection_options VALUES(102,23,'Croutons','0.5',1);
INSERT INTO selection_options VALUES(103,23,'Cheese','1.0',2);
INSERT INTO selection_options VALUES(104,24,'Regular','0.0',1);
INSERT INTO selection_options VALUES(105,24,'Large','2.0',2);
INSERT INTO selection_options VALUES(106,25,'Whipped Cream','0.5',1);
INSERT INTO selection_options VALUES(107,25,'Chocolate Sauce','0.5',2);
INSERT INTO selection_options VALUES(108,26,'Tofu','2.0',1);
INSERT INTO selection_options VALUES(109,26,'Tempeh','2.5',2);
INSERT INTO selection_options VALUES(110,26,'Seitan','3.0',3);
INSERT INTO selection_options VALUES(111,26,'Vegan Cheese','1.5',4);
INSERT INTO selection_options VALUES(112,26,'Mushrooms','1.0',5);
INSERT INTO selection_options VALUES(113,26,'Bell Peppers','0.5',6);
INSERT INTO selection_options VALUES(114,26,'Onions','0.5',7);
INSERT INTO selection_options VALUES(115,26,'Olives','1.0',8);
INSERT INTO selection_options VALUES(116,26,'Spinach','1.0',9);
INSERT INTO selection_options VALUES(117,26,'Artichoke Hearts','2.0',10);
INSERT INTO selection_options VALUES(143,34,'Mushrooms','0',1);
INSERT INTO selection_options VALUES(144,34,'Peppers','0',2);
INSERT INTO selection_options VALUES(145,34,'Onions','0',3);
INSERT INTO selection_options VALUES(146,35,'BBQ Sauce','0.5',1);
INSERT INTO selection_options VALUES(147,35,'Garlic Sauce','0.5',2);
INSERT INTO selection_options VALUES(148,35,'Hot Sauce','0.5',3);
INSERT INTO selection_options VALUES(149,35,'Sweet Chili Sauce','0.5',4);
INSERT INTO selection_options VALUES(150,35,'Ranch Sauce','0.5',5);
CREATE TABLE postcodes (Id INTEGER PRIMARY KEY AUTOINCREMENT, code TEXT NOT NULL, district TEXT);
INSERT INTO postcodes VALUES(1,'1200','Brigittenau');
INSERT INTO postcodes VALUES(2,'1210','Floridsdorf');
CREATE TABLE delivery_addresses (Id INTEGER PRIMARY KEY AUTOINCREMENT, postcode_id INTEGER NOT NULL, street TEXT NOT NULL, house TEXT, stairs TEXT, stick TEXT, door TEXT, bell TEXT, FOREIGN KEY (postcode_id) REFERENCES postcodes (Id) ON DELETE CASCADE);
INSERT INTO delivery_addresses VALUES(1,1,'Adalbert-Stifter-Straße',NULL,NULL,NULL,NULL,NULL);
INSERT INTO delivery_addresses VALUES(2,1,'Wallensteinstraße',NULL,NULL,NULL,NULL,NULL);
INSERT INTO delivery_addresses VALUES(3,1,'Leipziger Straße',NULL,NULL,NULL,NULL,NULL);
INSERT INTO delivery_addresses VALUES(4,1,'Klosterneuburger Straße',NULL,NULL,NULL,NULL,NULL);
INSERT INTO delivery_addresses VALUES(5,1,'Raffaelgasse',NULL,NULL,NULL,NULL,NULL);
INSERT INTO delivery_addresses VALUES(6,2,'Prager Straße',NULL,NULL,NULL,NULL,NULL);
INSERT INTO delivery_addresses VALUES(7,2,'Brünner Straße',NULL,NULL,NULL,NULL,NULL);
INSERT INTO delivery_addresses VALUES(8,2,'Donaufelder Straße',NULL,NULL,NULL,NULL,NULL);
INSERT INTO delivery_addresses VALUES(9,2,'Angyalföldstraße',NULL,NULL,NULL,NULL,NULL);
INSERT INTO delivery_addresses VALUES(10,2,'Floridusgasse',NULL,NULL,NULL,NULL,NULL);
CREATE TABLE postcode_addresses (Id INTEGER PRIMARY KEY AUTOINCREMENT, postcode_id INTEGER NOT NULL, street TEXT NOT NULL, FOREIGN KEY (postcode_id) REFERENCES postcodes (Id) ON DELETE CASCADE);
INSERT INTO postcode_addresses VALUES(1,1,'Adalbert-Stifter-Straße');
INSERT INTO postcode_addresses VALUES(2,1,'Wallensteinstraße');
INSERT INTO postcode_addresses VALUES(3,1,'Leipziger Straße');
INSERT INTO postcode_addresses VALUES(4,1,'Klosterneuburger Straße');
INSERT INTO postcode_addresses VALUES(5,1,'Raffaelgasse');
INSERT INTO postcode_addresses VALUES(6,2,'Prager Straße');
INSERT INTO postcode_addresses VALUES(7,2,'Brünner Straße');
INSERT INTO postcode_addresses VALUES(8,2,'Donaufelder Straße');
INSERT INTO postcode_addresses VALUES(9,2,'Angyalföldstraße');
INSERT INTO postcode_addresses VALUES(10,2,'Floridusgasse');
CREATE TABLE IF NOT EXISTS "PostcodeMinimumOrders" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_PostcodeMinimumOrders" PRIMARY KEY AUTOINCREMENT,
    "Postcode" TEXT NOT NULL,
    "MinimumOrderValue" TEXT NOT NULL
);
INSERT INTO PostcodeMinimumOrders VALUES(1,'1200','10.0');
INSERT INTO PostcodeMinimumOrders VALUES(2,'1210','20.0');
CREATE TABLE coupons (id INTEGER PRIMARY KEY AUTOINCREMENT, code TEXT NOT NULL UNIQUE, type TEXT NOT NULL, is_periodic INTEGER NOT NULL DEFAULT 0, start_date TEXT, end_date TEXT, email TEXT, is_used INTEGER DEFAULT 0, created_at TEXT DEFAULT CURRENT_TIMESTAMP, discount_ratio REAL DEFAULT 0);
INSERT INTO coupons VALUES(1,'SUMMER2025','PERIODIC',1,'2025-05-01 00:00:00','2025-07-01 23:59:59',NULL,0,'2025-05-01 00:00:00',0.25);
INSERT INTO coupons VALUES(2,'WELCOME10','ONE_TIME',0,NULL,NULL,NULL,0,'2025-05-01 00:00:00',0.5);
CREATE TABLE coupons_history (id INTEGER PRIMARY KEY AUTOINCREMENT, coupon_id INTEGER NOT NULL, email TEXT, used_at TEXT DEFAULT CURRENT_TIMESTAMP, FOREIGN KEY (coupon_id) REFERENCES coupons(id));
CREATE TABLE coupon_schedule (id INTEGER PRIMARY KEY AUTOINCREMENT, coupon_id INTEGER NOT NULL, monday INTEGER NOT NULL DEFAULT 0, tuesday INTEGER NOT NULL DEFAULT 0, wednesday INTEGER NOT NULL DEFAULT 0, thursday INTEGER NOT NULL DEFAULT 0, friday INTEGER NOT NULL DEFAULT 0, saturday INTEGER NOT NULL DEFAULT 0, sunday INTEGER NOT NULL DEFAULT 0, begin_time TEXT NOT NULL, end_time TEXT NOT NULL, created_at TEXT DEFAULT CURRENT_TIMESTAMP, updated_at TEXT DEFAULT CURRENT_TIMESTAMP, FOREIGN KEY (coupon_id) REFERENCES coupons(id), CHECK (begin_time < end_time));
INSERT INTO coupon_schedule VALUES(1,1,1,1,1,1,1,1,0,'11:00:00','23:00:00','2025-05-29 18:31:00','2025-05-29 18:31:00');
INSERT INTO coupon_schedule VALUES(2,2,0,0,0,0,0,1,1,'12:00:00','22:00:00','2025-05-29 18:31:00','2025-05-29 18:31:00');
CREATE TABLE order_details (id INTEGER PRIMARY KEY AUTOINCREMENT, order_id INTEGER NOT NULL, item_details TEXT NOT NULL, FOREIGN KEY (order_id) REFERENCES orders(id) ON DELETE CASCADE);
INSERT INTO order_details VALUES(1,89,'[{"Id":3,"Name":"Vegan Pizza","Quantity":2,"Price":40,"SelectedItems":[{"Id":108,"Name":"Tofu","Quantity":2,"Price":2},{"Id":109,"Name":"Tempeh","Quantity":4,"Price":2.5},{"Id":37,"Name":"Large","Quantity":1,"Price":4},{"Id":143,"Name":"Mushrooms","Quantity":1,"Price":0},{"Id":38,"Name":"Regular","Quantity":1,"Price":0},{"Id":42,"Name":"Pepperoni","Quantity":3,"Price":2},{"Id":52,"Name":"Cola","Quantity":2,"Price":1},{"Id":146,"Name":"BBQ Sauce","Quantity":6,"Price":0.5}]}]');
INSERT INTO order_details VALUES(2,90,'[{"Id":3,"Name":"Vegan Pizza","Quantity":2,"Price":37,"SelectedItems":[{"Id":108,"Name":"Tofu","Quantity":5,"Price":2},{"Id":109,"Name":"Tempeh","Quantity":1,"Price":2.5},{"Id":143,"Name":"Mushrooms","Quantity":1,"Price":0},{"Id":37,"Name":"Large","Quantity":1,"Price":4},{"Id":41,"Name":"Extra Cheese","Quantity":3,"Price":1.5},{"Id":44,"Name":"Onions","Quantity":1,"Price":1},{"Id":39,"Name":"Thin","Quantity":1,"Price":1},{"Id":52,"Name":"Cola","Quantity":1,"Price":1},{"Id":146,"Name":"BBQ Sauce","Quantity":1,"Price":0.5},{"Id":147,"Name":"Garlic Sauce","Quantity":3,"Price":0.5}]}]');
INSERT INTO order_details VALUES(3,91,'[{"Id":3,"Name":"Vegan Pizza","Quantity":2,"Price":37,"SelectedItems":[{"Id":108,"Name":"Tofu","Quantity":5,"Price":2},{"Id":109,"Name":"Tempeh","Quantity":1,"Price":2.5},{"Id":143,"Name":"Mushrooms","Quantity":1,"Price":0},{"Id":37,"Name":"Large","Quantity":1,"Price":4},{"Id":41,"Name":"Extra Cheese","Quantity":3,"Price":1.5},{"Id":44,"Name":"Onions","Quantity":1,"Price":1},{"Id":39,"Name":"Thin","Quantity":1,"Price":1},{"Id":52,"Name":"Cola","Quantity":1,"Price":1},{"Id":146,"Name":"BBQ Sauce","Quantity":1,"Price":0.5},{"Id":147,"Name":"Garlic Sauce","Quantity":3,"Price":0.5}]}]');
CREATE TABLE IF NOT EXISTS "orders" (id INTEGER NOT NULL CONSTRAINT id PRIMARY KEY AUTOINCREMENT, created_at TEXT NOT NULL, customer_info_id INTEGER NULL, order_method TEXT NOT NULL, order_number TEXT NOT NULL, payment_method TEXT NOT NULL, special_notes TEXT NULL, status TEXT NOT NULL, stripe_session_id TEXT NULL, total TEXT NOT NULL, updated_at TEXT NOT NULL, user_id INTEGER NULL, CONSTRAINT FK_orders_customerOrder_info_customer_info_id FOREIGN KEY (customer_info_id) REFERENCES customerOrder_info(Id), CONSTRAINT FK_orders_users_user_id FOREIGN KEY (user_id) REFERENCES users(Id));
INSERT INTO orders VALUES(1,'2025-05-10 11:55:39.246198',1,'','64BDE9B0','card',NULL,'pending','','29','2025-05-10 11:55:39.246293',NULL);
INSERT INTO orders VALUES(2,'2025-05-10 11:57:26.858863',1,'','24BE9D9D','card',NULL,'pending','','63','2025-05-10 11:57:26.858863',NULL);
INSERT INTO orders VALUES(3,'2025-05-10 12:23:00.887096',1,'delivery','23C7AA73','stripe','','processing','','60','2025-05-10 12:23:00.88715',NULL);
INSERT INTO orders VALUES(4,'2025-05-10 12:23:15.486564',2,'delivery','B5A90DCF','stripe','','processing','','60','2025-05-10 12:23:15.486564',NULL);
INSERT INTO orders VALUES(5,'2025-05-10 12:27:09.189923',3,'delivery','2377E4A9','stripe','','processing','','60','2025-05-10 12:27:09.190043',NULL);
INSERT INTO orders VALUES(6,'2025-05-10 12:30:47.094603',4,'delivery','8D3D3EF6','stripe','','processing','','60','2025-05-10 12:30:47.094603',NULL);
INSERT INTO orders VALUES(7,'2025-05-10 12:31:00.673605',5,'delivery','1D5E7986','stripe','','processing','','60','2025-05-10 12:31:00.673605',NULL);
INSERT INTO orders VALUES(8,'2025-05-10 12:31:33.404245',6,'delivery','21612A80','stripe','','processing','','60','2025-05-10 12:31:33.404386',NULL);
INSERT INTO orders VALUES(9,'2025-05-10 12:44:58.839038',1,'delivery','390514D2','stripe',NULL,'pending','cs_test_a1nj0qXzUbhpudK9koiv9VVs6n0jcxqylLuy0vfQnPeIJ3eJsQdwp9LTtE','21','2025-05-10 12:44:58.839093',NULL);
INSERT INTO orders VALUES(10,'2025-05-10 12:48:42.652215',1,'delivery','5EB6C815','stripe',NULL,'completed','cs_test_a1eIVNevx0uxmkPLIQpmLbDwih2nt5QjppKKtakdgBfwYbIm4WpRwdpByl','52','2025-05-10 12:51:40.94172',NULL);
INSERT INTO orders VALUES(11,'2025-05-10 12:52:39.228261',1,'delivery','AF83B6F5','stripe',NULL,'completed','cs_test_a1YS4ToCyDUOuiem9rWDKkYpyqaQinvBiYo7H0TIp4DdncYdb084apgPCD','12','2025-05-10 12:53:05.279045',NULL);
INSERT INTO orders VALUES(12,'2025-05-10 12:53:57.727202',1,'delivery','DCA93C96','cash',NULL,'pending','cs_test_a1voO8IBfoLxraVE3HEdGp8nwWMTOcHd9BGvzG7eCZC3bUGVYb8JNrVf35','60','2025-05-10 12:53:57.727202',NULL);
INSERT INTO orders VALUES(13,'2025-05-10 12:54:10.629937',1,'delivery','FE5DDB27','cash',NULL,'pending','cs_test_a1Bwcexe5SfbS7TnEKUwUB3XMAtBusquTgOFVS0KfUVb3R2fT8ac1vzeZ4','60','2025-05-10 12:54:10.629937',NULL);
INSERT INTO orders VALUES(14,'2025-05-10 12:58:57.241449',1,'delivery','88DE5F60','cash',NULL,'pending','cs_test_a1N99ipYW9ksMWQYP9hGRTCFrJ38a8fICvOgq2WX8KrBUUCafXJvSSe9oc','100','2025-05-10 12:58:57.241559',NULL);
INSERT INTO orders VALUES(15,'2025-05-10 12:59:03.907681',1,'delivery','3AE3EC59','cash',NULL,'pending','cs_test_a1s8p6h9f7pu8eY59o9xw7jcaTTFCqiO57LzgMgBuizhbNdEPklu1kXPWf','100','2025-05-10 12:59:03.907681',NULL);
INSERT INTO orders VALUES(16,'2025-05-10 12:59:20.916821',1,'delivery','BF862657','stripe',NULL,'pending','cs_test_a1MqKFQsxMT3omxt39kcaXBXTU1nHB8BCUEiTJCAwOcPwDDkIjcKjLjdrQ','100','2025-05-10 12:59:20.916821',NULL);
INSERT INTO orders VALUES(17,'2025-05-10 12:59:29.617684',1,'delivery','544C5679','cash',NULL,'pending','cs_test_a1tPYKJfVxNV9gu4ETFUdNWLRWsZgb8NQCNarmwU7gldsDP8AIEXFOlV5m','100','2025-05-10 12:59:29.617684',NULL);
INSERT INTO orders VALUES(18,'2025-05-10 13:00:21.700585',1,'delivery','D558C592','cash',NULL,'pending','cs_test_a1xLOcUKHYq8HDfkcEC1NTWz8RNrLQSsxP7UcpcJjP5GEUfu5LlcvlKIuc','100','2025-05-10 13:00:21.700585',NULL);
INSERT INTO orders VALUES(19,'2025-05-10 13:01:07.534189',1,'delivery','CD9A3F38','cash',NULL,'pending','cs_test_a1p0uW9kzSzD8P3MwMxGGILYKGoDx5IHl0G64Wt4VUrxvF2kLIoplUlfZn','12','2025-05-10 13:01:07.534255',NULL);
INSERT INTO orders VALUES(20,'2025-05-10 13:01:22.956851',1,'delivery','0DD57234','cash',NULL,'pending','cs_test_a1vyJTwiiSCKhkV3Zoxe6cnTbv1Ret8TNVfIJGIrUcq9L8IBo5TNMlQmMO','12','2025-05-10 13:01:22.956851',NULL);
INSERT INTO orders VALUES(21,'2025-05-10 13:02:17.240193',1,'delivery','49518C55','cash',NULL,'pending','cs_test_a18RLLYiSEnfZ7yTe0xgoNGyyTS3QA4zvc80sHhO9vLVLadNpRChOXFfF0','12','2025-05-10 13:02:17.240193',NULL);
INSERT INTO orders VALUES(22,'2025-05-10 13:03:05.997454',1,'delivery','EA0E9140','cash',NULL,'pending','cs_test_a1ZAgQQ0FGeCSU7007YMTzkzZwM7CqLn1zRAf0AfkyVP2JqC2SyuzaPsCA','12','2025-05-10 13:03:05.997454',NULL);
INSERT INTO orders VALUES(23,'2025-05-10 13:04:02.986225',1,'delivery','12FCD63A','cash',NULL,'pending','cs_test_a1jKBjcdwtz07JdjWdaBcdS8tGhHGPDZXLdJhi2WrNbYgtaGfpEvxcqXtf','12','2025-05-10 13:04:02.986226',NULL);
INSERT INTO orders VALUES(24,'2025-05-10 13:05:13.296994',1,'delivery','5803CC7D','cash',NULL,'pending','cs_test_a1ufiBYHXfQx6NFtSiqfq8GFUjFHxd215RCVZoL6ZsckSKCr83CGfM48Lf','12','2025-05-10 13:05:13.296994',NULL);
INSERT INTO orders VALUES(25,'2025-05-10 13:05:22.697064',1,'delivery','5335B2BF','cash',NULL,'pending','cs_test_a1vNBJkcBoiyXa9WwhysZcRcuLhAcM4oYqnfibHwbOwx67UQRk6SUfo2VF','12','2025-05-10 13:05:22.697064',NULL);
INSERT INTO orders VALUES(26,'2025-05-10 13:06:02.249268',1,'delivery','D504F26E','cash',NULL,'pending','cs_test_a1g8zHXzVhucquBIrcPsbAlM7b4WSSa8QM5OUlA9fmPwjDh9udjrTVBcaA','12','2025-05-10 13:06:02.249268',NULL);
INSERT INTO orders VALUES(27,'2025-05-10 13:06:11.914171',1,'delivery','8EBE66DE','cash',NULL,'pending','cs_test_a1PnNqcSNhXmJZHrc4pF3f3kYp4OrsohRizg3wkwA6hlOcVnBUMrpFQdnL','12','2025-05-10 13:06:11.914171',NULL);
INSERT INTO orders VALUES(28,'2025-05-10 13:07:07.781327',1,'delivery','7DBDFAEE','cash',NULL,'pending','cs_test_a1FKD3j3mXXr1NornfzOk3dYCOCan3GS2gJd8pZCZlSHQWTiw4wMqGR1hn','12','2025-05-10 13:07:07.781327',NULL);
INSERT INTO orders VALUES(29,'2025-05-10 13:07:52.089902',1,'delivery','D2C0E9CA','cash',NULL,'pending','cs_test_a1kQmBjhboDg0IsSpZ6yIxjcEmW174T9tGG5iZhtu9BKTabH4Bi0F4cWSq','12','2025-05-10 13:07:52.089902',NULL);
INSERT INTO orders VALUES(30,'2025-05-10 13:08:56.498948',1,'delivery','519DB563','cash',NULL,'pending','cs_test_a1a2gJMOrpHGDqrYA4IDaVjksbUuQ7K11J3uvYSgz9FZbxlEnTwhhxvTpH','12','2025-05-10 13:08:56.498949',NULL);
INSERT INTO orders VALUES(31,'2025-05-10 13:09:18.412612',1,'delivery','B88AB2DB','cash',NULL,'pending','cs_test_a1jKXolx3BiZOQ8xnnCTc7FL52u8jZzf40dCg3GZ5fJB9tRKX6FYDSuBP8','72','2025-05-10 13:09:18.412612',NULL);
INSERT INTO orders VALUES(32,'2025-05-10 13:10:21.487684',1,'delivery','0AC03818','cash',NULL,'pending','cs_test_a1G75ovcfm3Dc3dLEXlaXiVvYNzx3S8KO2PNNcj4RNVknFiEQY9m8Fuz0o','12','2025-05-10 13:10:21.487684',NULL);
INSERT INTO orders VALUES(33,'2025-05-10 13:11:50.925772',1,'delivery','8ED67BF5','cash',NULL,'pending','cs_test_a1wUdAFEOsslqrtnPZcU7uaKbgZnM8yXOJkAnKQjy4MZNtA7sDRQaP4Igl','10','2025-05-10 13:11:50.925772',NULL);
INSERT INTO orders VALUES(34,'2025-05-10 13:12:00.976281',1,'delivery','BF738B05','stripe',NULL,'completed','cs_test_a1fNSTp5AKhO48uk4RIIQFywxP3ZT45ntm3QbVPCA5xFwbe1RCOKT4lKO2','12','2025-05-10 13:14:05.032211',NULL);
INSERT INTO orders VALUES(35,'2025-05-10 13:15:05.187646',1,'delivery','3550174B','stripe',NULL,'completed','cs_test_a1L8yPL1fbMvwRjwdBF5OfB93gvUsCiMsWPCVKV6kaEtWmIRJSVwltb8zW','15','2025-05-10 13:15:39.128631',NULL);
INSERT INTO orders VALUES(36,'2025-05-10 13:15:55.233699',1,'delivery','19A7AFC6','cash',NULL,'pending','cs_test_a11cwEPFgOtvbPwXMUApkcLIWFWUqmUSWtL58sB5Ahqrjz7xwVLRVGg6xP','34','2025-05-10 13:15:55.233699',NULL);
INSERT INTO orders VALUES(37,'2025-05-10 22:25:35.509779',1,'delivery','FE556B5A','cash',NULL,'pending','cs_test_a1ukidOrPTUUTuGhNypPmHgZ6EwlR91JhNaAiN8vqfYGZFTDwC4MCSQmvv','21','2025-05-10 22:25:35.509923',NULL);
INSERT INTO orders VALUES(38,'2025-05-10 22:25:51.356129',1,'delivery','5EA5C888','stripe',NULL,'completed','cs_test_a1TgIi5qbqUEgbUgNPpfWo1DZxm2AZ23roI2PXouiBSxS65yPCiC1YoTnF','49','2025-05-10 22:26:23.327575',NULL);
INSERT INTO orders VALUES(39,'2025-05-10 22:26:38.171749',1,'delivery','264D8804','stripe',NULL,'pending','cs_test_a17UusVlPJ8Ue50yJvBZmcYQS6lVFA1nJnrxW8ZSHFTliIBHT8jtKNQmOb','12','2025-05-10 22:26:38.171749',NULL);
INSERT INTO orders VALUES(40,'2025-05-10 22:26:50.972761',1,'delivery','B235EBCD','stripe',NULL,'pending','cs_test_a10Qel0piJxGtQNQlBH8BCrUgVqEVttwwWzpqtWGNG3pe1gFOaNLJERAR0','12','2025-05-10 22:26:50.972761',NULL);
INSERT INTO orders VALUES(41,'2025-05-11 15:29:15.671271',1,'selfCollection','6FC68676','cash',NULL,'pending','cs_test_a10nEMNnVIi4fnChZZde82Y4Ut2X02ov1pXqUpgXnI5Jq3PPowdBoNgN9o','12','2025-05-11 15:29:15.671326',NULL);
INSERT INTO orders VALUES(42,'2025-05-11 15:29:39.949685',1,'delivery','29F3AB84','stripe',NULL,'pending','cs_test_a1JLXbPRuKnsZdFTFIKIfMDsQOtz9XndUvzwSWmUTUdwCQYOmuQBUVNv2Q','12','2025-05-11 15:29:39.949685',NULL);
INSERT INTO orders VALUES(43,'2025-05-11 15:29:54.89728',1,'selfCollection','47D30332','stripe',NULL,'completed','cs_test_a1DOQpYwUVdli24UV5splhM4dnSQKF9sqvwWrbaem4fegvGemI8gUxntGM','12','2025-05-11 15:30:17.763501',NULL);
INSERT INTO orders VALUES(44,'2025-05-12 08:15:56.448219',1,'selfCollection','FF734010','stripe',NULL,'completed','cs_test_a1x5bw0VutTobxqfLstTidONqWgMRS35B3znvM43YDbd9K42O3dL3hvpiX','46','2025-05-12 08:16:59.09679',NULL);
INSERT INTO orders VALUES(45,'2025-05-12 08:17:56.257434',1,'selfCollection','8B3D7CF0','cash',NULL,'pending','cs_test_a1tNSp6giBM8nP2OcwzDhiwVZAuIkqq8QHWSaqadXtmO2XRVPbYNegxWkj','10','2025-05-12 08:17:56.257434',NULL);
INSERT INTO orders VALUES(46,'2025-05-18 16:34:18.879295',7,'delivery','','cash','Test order','pending','cs_test_a1xBNvuwV7vnlXhVvwMtQWmnah4CbP9jboqjjv0RKsw1C3AremXv4Q8gIO','10.00','2025-05-18 16:34:18.879349',NULL);
INSERT INTO orders VALUES(47,'2025-05-18 16:36:41.210907',8,'delivery','D3FEDF69','cash','Test order','pending',NULL,'10.00','2025-05-18 16:36:41.210989',NULL);
INSERT INTO orders VALUES(48,'2025-05-18 16:44:39.074945',9,'delivery','6FE1A9A2','stripe',NULL,'completed','cs_test_a1PiwznYC9u0rWnQrgiBOTJITO8veY14DNd4jTJxmATWXfPe8dKulhKX7v','14','2025-05-18 16:45:18.677117',NULL);
INSERT INTO orders VALUES(49,'2025-05-18 16:45:37.667039',10,'delivery','27F0F06F','cash','asdasd','pending',NULL,'10','2025-05-18 16:45:37.667039',NULL);
INSERT INTO orders VALUES(50,'2025-05-18 16:46:45.291346',11,'delivery','2A83C7B6','cash','sdasdasdas','pending',NULL,'10','2025-05-18 16:46:45.291346',NULL);
INSERT INTO orders VALUES(51,'2025-05-18 17:07:35.355433',12,'delivery','6BC93141','cash','','pending',NULL,'24','2025-05-18 17:07:35.3555',NULL);
INSERT INTO orders VALUES(52,'2025-05-18 17:16:12.190033',13,'delivery','1446EF7A','stripe',NULL,'processing','cs_test_a1LEdLfZoBnHkJoGdXgBHRErwqJa4Mo7u4tH3RipJd6L0BA22zzwjr7PcW','50','2025-05-18 17:16:12.190033',NULL);
INSERT INTO orders VALUES(53,'2025-05-18 17:18:20.685456',14,'delivery','8CFFCD92','stripe',NULL,'processing','cs_test_a18xxGYLpTEDruo2qdkXrXv7aj2daqUUjhX3s6OPQb2CeNxDfa58CKuBe7','35','2025-05-18 17:18:20.685456',NULL);
INSERT INTO orders VALUES(54,'2025-05-25 21:10:54.870662',15,'delivery','83525D1B','stripe',NULL,'processing','cs_test_a1peYXvFlni0jNAADPjQX6c6O4sFK2KGBe6icuWk8kScJbDTHraSJxalGJ','26','2025-05-25 21:10:54.870713',NULL);
INSERT INTO orders VALUES(55,'2025-05-25 21:12:05.695017',16,'delivery','7A9E9695','stripe',NULL,'processing','cs_test_a1rafDamoHPR1pMAOpT1Koz5ggf9ijhIf0fqig7FIRGGcVElyjPLry6tWm','30','2025-05-25 21:12:05.695017',NULL);
INSERT INTO orders VALUES(56,'2025-05-25 21:13:28.698129',17,'delivery','09327E03','stripe',NULL,'processing','cs_test_a1eIgaz8ma7OQS1Oa4CifedO1UuNsoY78NoSXGtmWNkJB03qUYvHTz6Pqf','30','2025-05-25 21:13:28.698129',NULL);
INSERT INTO orders VALUES(57,'2025-05-25 21:15:34.667775',18,'delivery','68E59A5D','stripe',NULL,'processing','cs_test_a1V4AMnIjUsyqu8zfzyf4SEbU4PLqDzZ4R8w3kYMkyJtAfH8s6CFkVbkUo','42','2025-05-25 21:15:34.667775',NULL);
INSERT INTO orders VALUES(58,'2025-05-25 21:16:45.368413',19,'delivery','E166006B','stripe',NULL,'processing','cs_test_a1fc1CLL3kxVAjUHe9jlzV5Obph6GOVhYU5cJ0jSYPtEjJCIa4AqMu3K1r','40','2025-05-25 21:16:45.368413',NULL);
INSERT INTO orders VALUES(59,'2025-05-26 15:14:33.802889',20,'delivery','970AEA69','cash','','pending',NULL,'32','2025-05-26 15:14:33.802948',NULL);
INSERT INTO orders VALUES(60,'2025-05-26 15:22:09.19988',21,'delivery','28B2141F','stripe',NULL,'processing','cs_test_a1VXZUxTviGLwbDXceDlH9OZNSQLo6SFIQzHT4kyRt2djHguRwdS20ha26','30','2025-05-26 15:22:09.199881',NULL);
INSERT INTO orders VALUES(61,'2025-05-26 15:23:31.831006',22,'delivery','D2965958','stripe',NULL,'processing','cs_test_a1x6Lr973kg5DLJ65i0gUCKitdBVS7aLNU7R0ihpzF1VjOsirbWedkY5hx','30','2025-05-26 15:23:31.831006',NULL);
INSERT INTO orders VALUES(62,'2025-05-26 15:24:43.195028',23,'delivery','1F9C2347','stripe',NULL,'processing','cs_test_a1pRcV7RDrbBZS243WD6zlpM1JR4kR22BqB4KzGolvTCDnEOTXiJdFxhNc','30','2025-05-26 15:24:43.195028',NULL);
INSERT INTO orders VALUES(63,'2025-05-26 15:25:22.065846',24,'delivery','DF087790','stripe',NULL,'processing','cs_test_a1oNVa8LdPrW4sWNk6UuOefyfYc8Ieozlv3oW9fHNTYqDN0ega4gbtGz7h','30','2025-05-26 15:25:22.065897',NULL);
INSERT INTO orders VALUES(64,'2025-05-26 15:26:18.590103',25,'delivery','F27A2A36','stripe',NULL,'completed','cs_test_a1vzQezTaujuVqZ7x4sMyq2IDWSxKPBzGQ2w8yONdUXOBUTs1WfCylkH0T','30','2025-05-26 15:27:36.977766',NULL);
INSERT INTO orders VALUES(65,'2025-05-26 15:27:57.992165',26,'delivery','3499A1BF','stripe',NULL,'processing','cs_test_a1Wr2yRYTiE3Poxnx7FV5PkEnLNqxapGbN2n6CfPlXxxNTY11xMuR4QQNq','30','2025-05-26 15:27:57.992165',NULL);
INSERT INTO orders VALUES(66,'2025-05-26 15:46:33.437712',27,'delivery','FAD190C7','stripe',NULL,'processing','cs_test_a1o7IUeeNIu3PNXRk1U7abvBakHkndMOiM7FkdEKvfwdF32V9HFueW3Ip8','49.5','2025-05-26 15:46:33.437712',NULL);
INSERT INTO orders VALUES(67,'2025-05-26 15:52:00.007382',28,'delivery','B0EF3B6E','cash','','pending',NULL,'41','2025-05-26 15:52:00.007382',NULL);
INSERT INTO orders VALUES(68,'2025-05-28 18:33:58.281616',29,'delivery','D09E87A9','stripe',NULL,'completed','cs_test_a1qQdLedbh094kXojpNLW4s0RKCFKYa6N3GYZcz6o3y2C5PR61oCZpweHA','81.5','2025-05-28 18:34:23.810152',NULL);
INSERT INTO orders VALUES(69,'2025-05-28 20:13:00.705392',30,'delivery','41B67A7B','stripe',NULL,'processing','cs_test_a1sMNJ9D8QxDl1igzAy9QJfKw2KDdrLXWJwnzeGoSHGOzGWIm5pmelXaSJ','20','2025-05-28 20:13:00.705439',NULL);
INSERT INTO orders VALUES(70,'2025-05-29 18:46:28.135696',31,'delivery','FC7BEB6F','stripe',NULL,'processing','cs_test_a1CR5ZhmybkMRsIqBSC8V6QO0FkWdbBSP7bnWp5Ibi4r6IOcBv010oaxCz','80','2025-05-29 18:46:28.13575',NULL);
INSERT INTO orders VALUES(71,'2025-05-29 19:50:01.805',32,'delivery','1F1893EB','cash','','pending',NULL,'60','2025-05-29 19:50:01.805092',NULL);
INSERT INTO orders VALUES(72,'2025-05-31 11:09:21.308694',33,'selfCollection','5A1E0DC7','stripe',NULL,'processing','cs_test_a1ZOTgYFWStAQWLQKtJcPI1JF3hwwHRMfGZqsD29adt71CbORWgvSRCodz','65.5','2025-05-31 11:09:21.310521',NULL);
INSERT INTO orders VALUES(73,'2025-05-31 11:09:42.336821',34,'selfCollection','511516E0','stripe',NULL,'processing','cs_test_a1AQD2VlXC5V3minQg7c8BaoVujdQvo7Sx5UOlOTm5ChKJ6kUo8XVHVw9O','65.5','2025-05-31 11:09:42.336821',NULL);
INSERT INTO orders VALUES(74,'2025-05-31 11:09:42.930236',35,'selfCollection','9BFC87BA','stripe',NULL,'processing','cs_test_a1cLWyBhdPskqFD9YkKF5Wq3GXT33JxsL0hUEcL5P5sE5XwUYwEK5XcTE6','65.5','2025-05-31 11:09:42.930237',NULL);
INSERT INTO orders VALUES(75,'2025-05-31 11:09:58.358774',36,'selfCollection','2BA3CF39','stripe',NULL,'processing','cs_test_a1cqGW6VBGRmpkeqMboMbDAJiIHbEWYT2TPNeYH4st8bgPn4Zk0jrFQ3qd','65.5','2025-05-31 11:09:58.358774',NULL);
INSERT INTO orders VALUES(76,'2025-05-31 11:10:21.363067',37,'selfCollection','EBFEAED0','stripe',NULL,'processing','cs_test_a1lPnRQaRWfSHrBjAHJnD6CPTGUWdHIwazKvKyRyOFYocdPWhZYPsXhWjM','65.5','2025-05-31 11:10:21.363067',NULL);
INSERT INTO orders VALUES(77,'2025-05-31 11:11:40.142338',38,'selfCollection','8CB1F9F9','stripe',NULL,'processing','cs_test_a1ueE4cCx7SDBDUv82szJovn4NtI17fv2evOmzhPsovfqufKs3puwnkjSD','65.5','2025-05-31 11:11:40.142338',NULL);
INSERT INTO orders VALUES(78,'2025-05-31 11:39:57.457979',39,'selfCollection','608444E8','stripe',NULL,'processing','cs_test_a1NgL9Fh1PhA2sPlVmG0Xn56wN0KbaOyVRrZmLHkQPVtICOXHUDlG5CbJb','30','2025-05-31 11:39:57.458035',NULL);
INSERT INTO orders VALUES(79,'2025-05-31 11:52:48.026402',40,'selfCollection','E1058F29','cash','','pending',NULL,'35.25','2025-05-31 11:52:48.026402',NULL);
INSERT INTO orders VALUES(80,'2025-06-01 09:50:47.939035',41,'selfCollection','763E53AC','stripe',NULL,'processing','cs_test_a1O9rtvV56IBdtYJfa7gZXbFcXY8kPVpLbWYR29xSnrjv9dtE1TPYNThOC','40','2025-06-01 09:50:47.939079',NULL);
INSERT INTO orders VALUES(81,'2025-06-01 09:50:48.553121',42,'selfCollection','FF9EF23F','stripe',NULL,'processing','cs_test_a1cSUjQJBP3s0SkuP9UFBEECv7xIzHlVHFIZuaDDo40PRCPRDmAfUQJeW4','40','2025-06-01 09:50:48.553121',NULL);
INSERT INTO orders VALUES(82,'2025-06-01 09:50:59.198728',43,'selfCollection','69CF17B1','stripe',NULL,'processing','cs_test_a1EbMzVNRkefPch9AaO2gL35dtLa5w43UvZJtGNJUOgJOYZpBr9btkN7w0','40','2025-06-01 09:50:59.198728',NULL);
INSERT INTO orders VALUES(83,'2025-06-01 09:57:50.612657',44,'selfCollection','16CCD1A4','cash','','pending',NULL,'20','2025-06-01 09:57:50.612658',NULL);
INSERT INTO orders VALUES(84,'2025-06-01 10:33:25.963123',45,'selfCollection','1C8A4842','cash','','pending',NULL,'80','2025-06-01 10:33:25.963177',NULL);
INSERT INTO orders VALUES(85,'2025-06-01 10:33:52.031425',46,'selfCollection','4607A6AD','cash','','pending',NULL,'80','2025-06-01 10:33:52.031425',NULL);
INSERT INTO orders VALUES(86,'2025-06-01 10:34:15.16765',47,'selfCollection','22898556','cash','','pending',NULL,'80','2025-06-01 10:34:15.16765',NULL);
INSERT INTO orders VALUES(87,'2025-06-01 10:34:19.018735',48,'selfCollection','A44A7D8F','cash','','pending',NULL,'80','2025-06-01 10:34:19.018735',NULL);
INSERT INTO orders VALUES(88,'2025-06-01 10:34:27.694285',49,'selfCollection','D3D4C524','cash','','pending',NULL,'80','2025-06-01 10:34:27.694286',NULL);
INSERT INTO orders VALUES(89,'2025-06-01 11:06:29.644823',55,'selfCollection','4D159F63','cash','','pending',NULL,'80','2025-06-01 11:06:29.644873',NULL);
INSERT INTO orders VALUES(90,'2025-06-01 11:21:26.558211',56,'selfCollection','21A13313','stripe','test test test12','processing',NULL,'100','2025-06-01 11:21:26.558259',NULL);
INSERT INTO orders VALUES(91,'2025-06-01 11:21:38.470284',57,'selfCollection','B11798E2','stripe','test test test12','processing',NULL,'100','2025-06-01 11:21:38.470284',NULL);
DELETE FROM sqlite_sequence;
INSERT INTO sqlite_sequence VALUES('categories',8);
INSERT INTO sqlite_sequence VALUES('items',47);
INSERT INTO sqlite_sequence VALUES('offers',2);
INSERT INTO sqlite_sequence VALUES('item_offers',4);
INSERT INTO sqlite_sequence VALUES('customerOrder_info',57);
INSERT INTO sqlite_sequence VALUES('promotions',2);
INSERT INTO sqlite_sequence VALUES('item_allergens',14);
INSERT INTO sqlite_sequence VALUES('selection_options',150);
INSERT INTO sqlite_sequence VALUES('category_selection_groups',44);
INSERT INTO sqlite_sequence VALUES('selection_groups',35);
INSERT INTO sqlite_sequence VALUES('item_selection_groups',10);
INSERT INTO sqlite_sequence VALUES('postcodes',2);
INSERT INTO sqlite_sequence VALUES('delivery_addresses',10);
INSERT INTO sqlite_sequence VALUES('postcode_addresses',10);
INSERT INTO sqlite_sequence VALUES('PostcodeMinimumOrders',2);
INSERT INTO sqlite_sequence VALUES('coupons',2);
INSERT INTO sqlite_sequence VALUES('coupons_history',13);
INSERT INTO sqlite_sequence VALUES('coupon_schedule',2);
INSERT INTO sqlite_sequence VALUES('orders',91);
INSERT INTO sqlite_sequence VALUES('order_details',3);
CREATE INDEX "IX_item_offers_item_id" ON "item_offers" ("item_id");
CREATE INDEX "IX_item_offers_offer_id" ON "item_offers" ("offer_id");
CREATE INDEX "IX_items_category_id" ON "items" ("category_id");
CREATE INDEX "IX_category_selection_groups_category_id" ON "category_selection_groups" ("category_id");
CREATE INDEX "IX_category_selection_groups_selection_group_id" ON "category_selection_groups" ("selection_group_id");
CREATE INDEX "IX_item_selection_groups_item_id" ON "item_selection_groups" ("item_id");
CREATE INDEX "IX_item_selection_groups_selection_group_id" ON "item_selection_groups" ("selection_group_id");
CREATE INDEX "IX_selection_options_selection_group_id" ON "selection_options" ("selection_group_id");
COMMIT;
