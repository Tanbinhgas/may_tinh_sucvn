<?php
// ============================================================
//  db.php - Kết nối MySQL bằng PDO
// ============================================================

define('DB_HOST', 'localhost');
define('DB_NAME', 'tkl_computer');
define('DB_USER', 'root');
define('DB_PASS', '');
define('DB_CHARSET', 'utf8mb4');

try {
    $pdo = new PDO(
        "mysql:host=" . DB_HOST . ";dbname=" . DB_NAME . ";charset=" . DB_CHARSET,
        DB_USER,
        DB_PASS,
        [
            PDO::ATTR_ERRMODE            => PDO::ERRMODE_EXCEPTION,
            PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
            PDO::ATTR_EMULATE_PREPARES   => false,
        ]
    );
} catch (PDOException $e) {
    http_response_code(500);
    // Không lộ chi tiết lỗi ra ngoài
    error_log('DB Error: ' . $e->getMessage());
    die(json_encode(['error' => 'Không thể kết nối database. Vui lòng thử lại sau.']));
}
