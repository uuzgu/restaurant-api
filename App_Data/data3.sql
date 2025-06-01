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