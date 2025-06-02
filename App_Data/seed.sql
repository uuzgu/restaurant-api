-- Seed categories
INSERT INTO categories (id, name) VALUES
(0, 'Promotions'),
(1, 'Pizza'),
(2, 'Bowls'),
(3, 'Cheeseburger'),
(4, 'Salad'),
(5, 'Breakfast'),
(6, 'Drinks'),
(7, 'Soup'),
(8, 'Dessert');

-- Seed selection groups
INSERT INTO selection_groups (id, name, type, is_required, min_select, max_select, threshold, display_order) VALUES
(1, 'Size', 'single', 1, 1, 1, 0, 1),
(2, 'Crust', 'single', 1, 1, 1, 0, 2),
(3, 'Toppings', 'multiple', 0, 0, 5, 0, 3),
(4, 'Extras', 'multiple', 0, 0, 3, 0, 4);

-- Seed selection options
INSERT INTO selection_options (id, selection_group_id, name, price, display_order) VALUES
(1, 1, 'Small', 0, 1),
(2, 1, 'Medium', 200, 2),
(3, 1, 'Large', 400, 3),
(4, 2, 'Regular', 0, 1),
(5, 2, 'Thin', 0, 2),
(6, 2, 'Thick', 100, 3),
(7, 3, 'Extra Cheese', 50, 1),
(8, 3, 'Mushrooms', 30, 2),
(9, 3, 'Peppers', 30, 3),
(10, 4, 'Garlic Sauce', 20, 1),
(11, 4, 'BBQ Sauce', 20, 2),
(12, 4, 'Hot Sauce', 20, 3);

-- Seed items
INSERT INTO items (id, name, description, price, category_id, image_url) VALUES
(1, 'Margherita Pizza', 'Classic tomato sauce and mozzarella', 800, 1, '/images/margherita.jpg'),
(2, 'Caesar Salad', 'Fresh romaine lettuce with Caesar dressing', 600, 4, '/images/caesar.jpg'),
(3, 'Chicken Bowl', 'Grilled chicken with rice and vegetables', 900, 2, '/images/chicken-bowl.jpg'),
(4, 'Classic Burger', 'Beef patty with lettuce and tomato', 700, 3, '/images/burger.jpg');

-- Link items with selection groups
INSERT INTO item_selection_groups (item_id, selection_group_id) VALUES
(1, 1), -- Margherita Pizza - Size
(1, 2), -- Margherita Pizza - Crust
(1, 3), -- Margherita Pizza - Toppings
(1, 4), -- Margherita Pizza - Extras
(4, 3), -- Classic Burger - Toppings
(4, 4); -- Classic Burger - Extras

-- Add some allergens
INSERT INTO item_allergens (item_id, allergen_code) VALUES
(1, 'G'), -- Margherita Pizza - Gluten
(1, 'L'), -- Margherita Pizza - Lactose
(2, 'E'), -- Caesar Salad - Egg
(3, 'G'), -- Chicken Bowl - Gluten
(4, 'G'), -- Classic Burger - Gluten
(4, 'L'); -- Classic Burger - Lactose 