<?php
// ============================================================
//  api_user.php - Trả về thông tin user hiện tại (cho JS)
// ============================================================

header('Content-Type: application/json; charset=utf-8');
require_once __DIR__ . '/auth.php';

$action = $_GET['action'] ?? '';

// Trạng thái đăng nhập (dùng cho navbar)
if ($action === 'status') {
    $user = getCurrentUser();
    if ($user) {
        echo json_encode([
            'logged_in' => true,
            'username'  => $user['username'],
            'full_name' => $user['full_name'],
            'role'      => $user['role'],
        ]);
    } else {
        echo json_encode(['logged_in' => false]);
    }
    exit;
}

echo json_encode(['error' => 'Hành động không hợp lệ.']);
