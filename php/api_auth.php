<?php
// ============================================================
//  api_auth.php - API xử lý đăng nhập / đăng ký / đăng xuất
// ============================================================

header('Content-Type: application/json; charset=utf-8');
require_once __DIR__ . '/auth.php';

$action = $_POST['action'] ?? '';

// ── ĐĂNG NHẬP ──────────────────────────────────────────────
if ($action === 'login') {
    $username = trim($_POST['username'] ?? '');
    $password = $_POST['password'] ?? '';

    if (!$username || !$password) {
        echo json_encode(['success' => false, 'message' => 'Vui lòng điền đầy đủ thông tin.']);
        exit;
    }

    echo json_encode(login($username, $password));
    exit;
}

// ── ĐĂNG KÝ ────────────────────────────────────────────────
if ($action === 'register') {
    $username  = trim($_POST['username']  ?? '');
    $email     = trim($_POST['email']     ?? '');
    $password  = $_POST['password']       ?? '';
    $full_name = trim($_POST['full_name'] ?? '');
    $phone     = trim($_POST['phone']     ?? '');

    if (!$username || !$email || !$password || !$full_name) {
        echo json_encode(['success' => false, 'message' => 'Vui lòng điền đầy đủ thông tin bắt buộc.']);
        exit;
    }
    if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
        echo json_encode(['success' => false, 'message' => 'Email không hợp lệ.']);
        exit;
    }
    if (strlen($password) < 6) {
        echo json_encode(['success' => false, 'message' => 'Mật khẩu tối thiểu 6 ký tự.']);
        exit;
    }

    echo json_encode(register($username, $email, $password, $full_name, $phone ?: null));
    exit;
}

// ── ĐĂNG XUẤT ──────────────────────────────────────────────
if ($action === 'logout') {
    logout(); // tự redirect
}

echo json_encode(['success' => false, 'message' => 'Hành động không hợp lệ.']);
