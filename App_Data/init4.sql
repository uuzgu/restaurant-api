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