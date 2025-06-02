-- Create tables
CREATE TABLE IF NOT EXISTS categories (
    id INTEGER PRIMARY KEY,
    name TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS items (
    id INTEGER PRIMARY KEY,
    name TEXT NOT NULL,
    description TEXT,
    price INTEGER NOT NULL,
    category_id INTEGER NOT NULL,
    image_url TEXT,
    FOREIGN KEY (category_id) REFERENCES categories(id)
);

CREATE TABLE IF NOT EXISTS selection_groups (
    id INTEGER PRIMARY KEY,
    name TEXT NOT NULL,
    type TEXT NOT NULL,
    is_required INTEGER NOT NULL,
    min_select INTEGER NOT NULL,
    max_select INTEGER,
    threshold INTEGER NOT NULL,
    display_order INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS selection_options (
    id INTEGER PRIMARY KEY,
    selection_group_id INTEGER NOT NULL,
    name TEXT NOT NULL,
    price DECIMAL(10,2),
    display_order INTEGER NOT NULL,
    FOREIGN KEY (selection_group_id) REFERENCES selection_groups(id)
);

CREATE TABLE IF NOT EXISTS item_selection_groups (
    id INTEGER PRIMARY KEY,
    item_id INTEGER NOT NULL,
    selection_group_id INTEGER NOT NULL,
    FOREIGN KEY (item_id) REFERENCES items(id),
    FOREIGN KEY (selection_group_id) REFERENCES selection_groups(id)
);

CREATE TABLE IF NOT EXISTS item_allergens (
    id INTEGER PRIMARY KEY,
    item_id INTEGER NOT NULL,
    allergen_code TEXT NOT NULL,
    FOREIGN KEY (item_id) REFERENCES items(id)
);

CREATE TABLE IF NOT EXISTS customerOrder_info (
    id INTEGER PRIMARY KEY,
    name TEXT NOT NULL,
    email TEXT NOT NULL,
    phone TEXT NOT NULL,
    address TEXT NOT NULL,
    createDate DATETIME NOT NULL
);

CREATE TABLE IF NOT EXISTS orders (
    id INTEGER PRIMARY KEY,
    created_at DATETIME NOT NULL,
    customer_info_id INTEGER,
    order_method TEXT NOT NULL,
    order_number TEXT NOT NULL,
    payment_method TEXT NOT NULL,
    special_notes TEXT,
    status TEXT NOT NULL,
    stripe_session_id TEXT,
    total TEXT NOT NULL,
    updated_at DATETIME NOT NULL,
    user_id INTEGER,
    FOREIGN KEY (customer_info_id) REFERENCES customerOrder_info(id)
);

CREATE TABLE IF NOT EXISTS order_details (
    id INTEGER PRIMARY KEY,
    order_id INTEGER NOT NULL,
    item_details TEXT NOT NULL,
    FOREIGN KEY (order_id) REFERENCES orders(id)
);

CREATE TABLE IF NOT EXISTS category_selection_groups (
    id INTEGER PRIMARY KEY,
    category_id INTEGER NOT NULL,
    selection_group_id INTEGER NOT NULL,
    FOREIGN KEY (category_id) REFERENCES categories(id),
    FOREIGN KEY (selection_group_id) REFERENCES selection_groups(id)
);

CREATE TABLE IF NOT EXISTS selections (
    id INTEGER PRIMARY KEY,
    selection_option_id INTEGER NOT NULL,
    order_item_id INTEGER NOT NULL,
    FOREIGN KEY (selection_option_id) REFERENCES selection_options(id)
);

CREATE TABLE IF NOT EXISTS postcodes (
    id INTEGER PRIMARY KEY,
    code TEXT NOT NULL,
    district TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS delivery_addresses (
    id INTEGER PRIMARY KEY,
    postcode_id INTEGER NOT NULL,
    street TEXT NOT NULL,
    house TEXT NOT NULL,
    stairs TEXT,
    stick TEXT,
    door TEXT,
    bell TEXT,
    FOREIGN KEY (postcode_id) REFERENCES postcodes(id)
);

CREATE TABLE IF NOT EXISTS postcode_minimum_orders (
    id INTEGER PRIMARY KEY,
    postcode_id INTEGER NOT NULL,
    minimum_order_amount INTEGER NOT NULL,
    FOREIGN KEY (postcode_id) REFERENCES postcodes(id)
);

CREATE TABLE IF NOT EXISTS coupons (
    id INTEGER PRIMARY KEY,
    code TEXT NOT NULL UNIQUE,
    discount_percentage INTEGER,
    discount_amount INTEGER,
    minimum_order_amount INTEGER,
    start_date DATETIME NOT NULL,
    end_date DATETIME NOT NULL,
    is_active INTEGER NOT NULL DEFAULT 1
);

CREATE TABLE IF NOT EXISTS coupon_history (
    id INTEGER PRIMARY KEY,
    coupon_id INTEGER NOT NULL,
    order_id INTEGER NOT NULL,
    used_date DATETIME NOT NULL,
    FOREIGN KEY (coupon_id) REFERENCES coupons(id),
    FOREIGN KEY (order_id) REFERENCES orders(id)
);

-- Create indexes
CREATE INDEX IF NOT EXISTS idx_items_category_id ON items(category_id);
CREATE INDEX IF NOT EXISTS idx_item_selection_groups_item_id ON item_selection_groups(item_id);
CREATE INDEX IF NOT EXISTS idx_item_selection_groups_selection_group_id ON item_selection_groups(selection_group_id);
CREATE INDEX IF NOT EXISTS idx_selection_options_selection_group_id ON selection_options(selection_group_id);
CREATE INDEX IF NOT EXISTS idx_item_allergens_item_id ON item_allergens(item_id);
CREATE INDEX IF NOT EXISTS idx_orders_customer_info_id ON orders(customer_info_id);
CREATE INDEX IF NOT EXISTS idx_order_details_order_id ON order_details(order_id);
CREATE INDEX IF NOT EXISTS idx_category_selection_groups_category_id ON category_selection_groups(category_id);
CREATE INDEX IF NOT EXISTS idx_category_selection_groups_selection_group_id ON category_selection_groups(selection_group_id);
CREATE INDEX IF NOT EXISTS idx_selections_selection_option_id ON selections(selection_option_id);
CREATE INDEX IF NOT EXISTS idx_delivery_addresses_postcode_id ON delivery_addresses(postcode_id);
CREATE INDEX IF NOT EXISTS idx_postcode_minimum_orders_postcode_id ON postcode_minimum_orders(postcode_id);
CREATE INDEX IF NOT EXISTS idx_coupon_history_coupon_id ON coupon_history(coupon_id);
CREATE INDEX IF NOT EXISTS idx_coupon_history_order_id ON coupon_history(order_id); 