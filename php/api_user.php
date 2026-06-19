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
            'email'     => $user['email'] ?? '',
            'role'      => $user['role'],
            'created_at'=> $user['created_at'] ?? null,
        ]);
    } else {
        echo json_encode(['logged_in' => false]);
    }
    exit;
}

if ($action === 'profile') {
    $user = getCurrentUser();
    if (!$user) {
        http_response_code(401);
        echo json_encode(['logged_in' => false, 'message' => 'Vui lòng đăng nhập.']);
        exit;
    }

    echo json_encode([
        'logged_in'  => true,
        'username'   => $user['username'],
        'full_name'  => $user['full_name'],
        'email'      => $user['email'] ?? '',
        'phone'      => $user['phone'] ?? '',
        'address'    => $user['address'] ?? '',
        'role'       => $user['role'],
        'created_at' => $user['created_at'] ?? null,
    ]);
    exit;
}

echo json_encode(['error' => 'Hành động không hợp lệ.']);
