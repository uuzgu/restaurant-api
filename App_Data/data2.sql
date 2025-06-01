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