USE tkl_computer;

CREATE TABLE IF NOT EXISTS cart (
    id          INT AUTO_INCREMENT PRIMARY KEY,
    user_id     INT NOT NULL,
    product_id  VARCHAR(50) NOT NULL,
    name        VARCHAR(200) NOT NULL,
    price       DECIMAL(15,0) NOT NULL,
    quantity    INT NOT NULL DEFAULT 1,
    image       VARCHAR(255),
    summary     TEXT,
    updated_at  DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    UNIQUE KEY unique_cart_item (user_id, product_id),
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);
