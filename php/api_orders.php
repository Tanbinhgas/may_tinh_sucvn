<?php
// ============================================================
//  api_orders.php - API đơn hàng cho khách
// ============================================================

header('Content-Type: application/json; charset=utf-8');
require_once __DIR__ . '/auth.php';

$user = getCurrentUser();
if (!$user) {
    echo json_encode(['success' => false, 'message' => 'Vui lòng đăng nhập.']);
    exit;
}

$action = $_POST['action'] ?? '';

// ── HUỶ ĐƠN HÀNG (chỉ khi đang pending) ───────────────────
if ($action === 'cancel') {
    $order_id = (int)($_POST['order_id'] ?? 0);

    // Kiểm tra đơn thuộc về user và đang pending
    $stmt = $pdo->prepare("
        SELECT id FROM orders 
        WHERE id = ? AND user_id = ? AND status = 'pending'
    ");
    $stmt->execute([$order_id, $user['id']]);

    if (!$stmt->fetch()) {
        echo json_encode(['success' => false, 'message' => 'Không thể huỷ đơn hàng này.']);
        exit;
    }

    $pdo->prepare("UPDATE orders SET status = 'cancelled' WHERE id = ?")
        ->execute([$order_id]);

    echo json_encode(['success' => true, 'message' => 'Đã huỷ đơn hàng thành công.']);
    exit;
}

echo json_encode(['success' => false, 'message' => 'Hành động không hợp lệ.']);
