CREATE DATABASE IF NOT EXISTS tkl_computer
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE tkl_computer;

-- ============================================================
-- 1. BẢNG USERS - Tài khoản người dùng
-- ============================================================
CREATE TABLE users (
    id          INT AUTO_INCREMENT PRIMARY KEY,
    username    VARCHAR(50)  NOT NULL UNIQUE,
    email       VARCHAR(100) NOT NULL UNIQUE,
    password    VARCHAR(255) NOT NULL,          -- bcrypt hash
    full_name   VARCHAR(100),
    phone       VARCHAR(15),
    address     TEXT,
    role        ENUM('admin', 'customer') NOT NULL DEFAULT 'customer',
    is_active   TINYINT(1) NOT NULL DEFAULT 1,  -- 0 = bị khoá
    created_at  DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at  DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Tài khoản admin mặc định (password: Admin@123)
INSERT INTO users (username, email, password, full_name, role)
VALUES (
    'admin',
    'admin@tklcomputer.vn',
    '$2y$12$exampleHashedPasswordHere',  -- thay bằng hash thật khi deploy
    'Quản Trị Viên',
    'admin'
);

-- ============================================================
-- 2. BẢNG CATEGORIES - Danh mục sản phẩm
-- ============================================================
CREATE TABLE categories (
    id          INT AUTO_INCREMENT PRIMARY KEY,
    name        VARCHAR(100) NOT NULL,           -- VD: CPU, GPU, RAM
    slug        VARCHAR(100) NOT NULL UNIQUE,    -- VD: cpu, gpu, ram
    description TEXT,
    created_at  DATETIME DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO categories (name, slug) VALUES
    ('CPU - Bộ vi xử lý',       'cpu'),
    ('GPU - Card đồ họa',       'gpu'),
    ('RAM - Bộ nhớ',            'ram'),
    ('SSD - Ổ cứng thể rắn',   'ssd'),
    ('HDD - Ổ cứng cơ',        'hdd'),
    ('Mainboard - Bo mạch chủ', 'mainboard'),
    ('PC Case - Vỏ máy tính',   'pccase'),
    ('PSU - Nguồn máy tính',    'psu'),
    ('Tản nhiệt',               'cooling'),
    ('Monitor - Màn hình',      'monitor'),
    ('Keyboard - Bàn phím',     'keyboard'),
    ('Mouse - Chuột',           'mouse'),
    ('Headset - Tai nghe',      'headset');

-- ============================================================
-- 3. BẢNG PRODUCTS - Sản phẩm
-- ============================================================
CREATE TABLE products (
    id            INT AUTO_INCREMENT PRIMARY KEY,
    category_id   INT NOT NULL,
    name          VARCHAR(200) NOT NULL,
    slug          VARCHAR(200) NOT NULL UNIQUE,
    description   TEXT,
    price         DECIMAL(15, 0) NOT NULL,       -- VNĐ
    stock         INT NOT NULL DEFAULT 0,
    image_url     VARCHAR(255),
    brand         VARCHAR(100),
    is_active     TINYINT(1) NOT NULL DEFAULT 1, -- 0 = ẩn khỏi cửa hàng
    created_at    DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at    DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    FOREIGN KEY (category_id) REFERENCES categories(id) ON DELETE RESTRICT
);

CREATE INDEX idx_products_category ON products(category_id);
CREATE INDEX idx_products_active    ON products(is_active);

-- ============================================================
-- 4. BẢNG ORDERS - Đơn hàng
-- ============================================================
CREATE TABLE orders (
    id               INT AUTO_INCREMENT PRIMARY KEY,
    user_id          INT NOT NULL,
    status           ENUM(
                         'pending',    -- chờ xác nhận
                         'confirmed',  -- đã xác nhận
                         'shipping',   -- đang giao
                         'done',       -- hoàn thành
                         'cancelled'   -- đã huỷ
                     ) NOT NULL DEFAULT 'pending',
    total_price      DECIMAL(15, 0) NOT NULL,
    shipping_address TEXT NOT NULL,
    phone            VARCHAR(15) NOT NULL,
    note             TEXT,
    created_at       DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at       DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE RESTRICT
);

CREATE INDEX idx_orders_user   ON orders(user_id);
CREATE INDEX idx_orders_status ON orders(status);

-- ============================================================
-- 5. BẢNG ORDER_ITEMS - Chi tiết đơn hàng
-- ============================================================
CREATE TABLE order_items (
    id          INT AUTO_INCREMENT PRIMARY KEY,
    order_id    INT NOT NULL,
    product_id  INT NOT NULL,
    product_name VARCHAR(200) NOT NULL,  -- lưu tên tại thời điểm mua
    price       DECIMAL(15, 0) NOT NULL, -- lưu giá tại thời điểm mua
    quantity    INT NOT NULL DEFAULT 1,

    FOREIGN KEY (order_id)   REFERENCES orders(id)   ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE RESTRICT
);

-- ============================================================
-- 6. BẢNG SESSIONS - Phiên đăng nhập (thay thế JWT đơn giản)
-- ============================================================
CREATE TABLE sessions (
    id          VARCHAR(128) PRIMARY KEY,        -- session token
    user_id     INT NOT NULL,
    ip_address  VARCHAR(45),
    user_agent  VARCHAR(255),
    expires_at  DATETIME NOT NULL,
    created_at  DATETIME DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);

-- ============================================================
-- 7. VIEW: Thống kê doanh thu theo tháng (cho admin)
-- ============================================================
CREATE VIEW revenue_by_month AS
SELECT
    DATE_FORMAT(created_at, '%Y-%m')  AS month,
    COUNT(*)                           AS total_orders,
    SUM(total_price)                   AS total_revenue
FROM orders
WHERE status = 'done'
GROUP BY month
ORDER BY month DESC;

-- ============================================================
-- 8. VIEW: Thống kê sản phẩm bán chạy (cho admin)
-- ============================================================
CREATE VIEW top_selling_products AS
SELECT
    p.id,
    p.name,
    c.name          AS category,
    SUM(oi.quantity) AS total_sold,
    SUM(oi.quantity * oi.price) AS total_revenue
FROM order_items oi
JOIN products p ON p.id = oi.product_id
JOIN categories c ON c.id = p.category_id
JOIN orders o ON o.id = oi.order_id
WHERE o.status = 'done'
GROUP BY p.id, p.name, c.name
ORDER BY total_sold DESC;
