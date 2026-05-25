<?php
// ============================================================
//  auth.php - Hệ thống xác thực dùng chung
// ============================================================

require_once __DIR__ . '/db.php';

// Khởi động session PHP (bảo mật hơn cookie thuần)
if (session_status() === PHP_SESSION_NONE) {
    session_set_cookie_params([
        'lifetime' => 0,
        'path'     => '/',
        'httponly' => true,
        'samesite' => 'Strict',
    ]);
    session_start();
}

// ── ĐĂNG KÝ ────────────────────────────────────────────────
function register(string $username, string $email, string $password, string $full_name, ?string $phone = null): array {
    global $pdo;

    $stmt = $pdo->prepare("SELECT id FROM users WHERE email = ? OR username = ? LIMIT 1");
    $stmt->execute([$email, $username]);
    if ($stmt->fetch()) {
        return ['success' => false, 'message' => 'Email hoặc tên đăng nhập đã tồn tại.'];
    }

    $hashed = password_hash($password, PASSWORD_BCRYPT, ['cost' => 12]);
    $stmt = $pdo->prepare("
        INSERT INTO users (username, email, password, full_name, phone, role)
        VALUES (?, ?, ?, ?, ?, 'customer')
    ");
    $stmt->execute([$username, $email, $hashed, $full_name, $phone ?: null]);

    return ['success' => true, 'message' => 'Đăng ký thành công!'];
}

// ── ĐĂNG NHẬP ──────────────────────────────────────────────
function login(string $username_or_email, string $password): array {
    global $pdo;

    $stmt = $pdo->prepare("
        SELECT * FROM users
        WHERE (username = ? OR email = ?) AND is_active = 1
        LIMIT 1
    ");
    $stmt->execute([$username_or_email, $username_or_email]);
    $user = $stmt->fetch();

    if (!$user || !password_verify($password, $user['password'])) {
        return ['success' => false, 'message' => 'Sai tên đăng nhập hoặc mật khẩu.'];
    }

    // Lưu thông tin vào session PHP
    $_SESSION['user_id']   = $user['id'];
    $_SESSION['username']  = $user['username'];
    $_SESSION['full_name'] = $user['full_name'];
    $_SESSION['role']      = $user['role'];

    // Đồng thời lưu token vào DB để có thể revoke từ xa
    $token      = bin2hex(random_bytes(64));
    $expires_at = date('Y-m-d H:i:s', strtotime('+7 days'));
    $pdo->prepare("
        INSERT INTO sessions (id, user_id, ip_address, user_agent, expires_at)
        VALUES (?, ?, ?, ?, ?)
    ")->execute([
        $token,
        $user['id'],
        $_SERVER['REMOTE_ADDR']     ?? null,
        $_SERVER['HTTP_USER_AGENT'] ?? null,
        $expires_at,
    ]);

    return [
        'success'   => true,
        'role'      => $user['role'],
        'full_name' => $user['full_name'],
    ];
}

// ── LẤY USER HIỆN TẠI ──────────────────────────────────────
function getCurrentUser(): ?array {
    if (!empty($_SESSION['user_id'])) {
        return [
            'id'        => $_SESSION['user_id'],
            'username'  => $_SESSION['username'],
            'full_name' => $_SESSION['full_name'],
            'role'      => $_SESSION['role'],
        ];
    }
    return null;
}

// ── YÊU CẦU ĐĂNG NHẬP ──────────────────────────────────────
function requireLogin(): array {
    $user = getCurrentUser();
    if (!$user) {
        header('Location: /may_tinh_sucvn/php/login.php');
        exit;
    }
    return $user;
}

// ── YÊU CẦU QUYỀN ADMIN ────────────────────────────────────
function requireAdmin(): array {
    $user = getCurrentUser();
    if (!$user) {
        header('Location: /may_tinh_sucvn/php/login.php');
        exit;
    }
    if ($user['role'] !== 'admin') {
        http_response_code(403);
        die('<h2>403 - Bạn không có quyền truy cập trang này.</h2>');
    }
    return $user;
}

// ── ĐĂNG XUẤT ──────────────────────────────────────────────
function logout(): void {
    global $pdo;

    // Xoá session DB nếu có
    if (!empty($_SESSION['user_id'])) {
        $pdo->prepare("DELETE FROM sessions WHERE user_id = ?")->execute([$_SESSION['user_id']]);
    }

    // Huỷ session PHP
    $_SESSION = [];
    session_destroy();

    header('Location: /may_tinh_sucvn/php/login.php');
    exit;
}
