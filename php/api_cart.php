<?php
// ============================================================
//  api_cart.php - API giỏ hàng
//  Thay thế localStorage bằng MySQL
// ============================================================

header('Content-Type: application/json; charset=utf-8');
require_once __DIR__ . '/auth.php';

$user = getCurrentUser();
$action = $_POST['action'] ?? $_GET['action'] ?? '';

// ── CHƯA ĐĂNG NHẬP ─────────────────────────────────────────
if (!$user) {
    echo json_encode(['success' => false, 'message' => 'Vui lòng đăng nhập để dùng giỏ hàng.', 'require_login' => true]);
    exit;
}

$uid = $user['id'];

// ── LẤY GIỎ HÀNG ───────────────────────────────────────────
if ($action === 'get') {
    $items = $pdo->prepare("SELECT * FROM cart WHERE user_id = ? ORDER BY updated_at DESC");
    $items->execute([$uid]);
    echo json_encode(['success' => true, 'items' => $items->fetchAll()]);
    exit;
}

// ── THÊM / CẬP NHẬT SẢN PHẨM ──────────────────────────────
if ($action === 'add') {
    $product_id = trim($_POST['product_id'] ?? '');
    $name       = trim($_POST['name']       ?? '');
    $price      = (int)($_POST['price']     ?? 0);
    $quantity   = max(1, (int)($_POST['quantity'] ?? 1));
    $image      = trim($_POST['image']      ?? '');
    $summary    = trim($_POST['summary']    ?? '');

    if (!$product_id || !$name || !$price) {
        echo json_encode(['success' => false, 'message' => 'Thiếu thông tin sản phẩm.']);
        exit;
    }

    // Nếu đã có thì tăng số lượng, chưa có thì thêm mới
    $pdo->prepare("
        INSERT INTO cart (user_id, product_id, name, price, quantity, image, summary)
        VALUES (?, ?, ?, ?, ?, ?, ?)
        ON DUPLICATE KEY UPDATE quantity = quantity + VALUES(quantity)
    ")->execute([$uid, $product_id, $name, $price, $quantity, $image, $summary]);

    // Đếm số loại sản phẩm trong giỏ
    $count = $pdo->prepare("SELECT COUNT(*) FROM cart WHERE user_id = ?");
    $count->execute([$uid]);

    echo json_encode(['success' => true, 'message' => 'Đã thêm vào giỏ hàng!', 'cart_count' => $count->fetchColumn()]);
    exit;
}

// ── CẬP NHẬT SỐ LƯỢNG ──────────────────────────────────────
if ($action === 'update') {
    $product_id = trim($_POST['product_id'] ?? '');
    $quantity   = (int)($_POST['quantity']  ?? 0);

    if ($quantity <= 0) {
        // Số lượng = 0 thì xoá
        $pdo->prepare("DELETE FROM cart WHERE user_id = ? AND product_id = ?")->execute([$uid, $product_id]);
    } else {
        $pdo->prepare("UPDATE cart SET quantity = ? WHERE user_id = ? AND product_id = ?")->execute([$quantity, $uid, $product_id]);
    }

    // Tính lại tổng
    $total = $pdo->prepare("SELECT COALESCE(SUM(price * quantity), 0) FROM cart WHERE user_id = ?");   	<!-- Tính tổng tiền đơn hàng -->
    $total->execute([$uid]);

    echo json_encode(['success' => true, 'total' => $total->fetchColumn()]);
    exit;
}

// ── XOÁ 1 SẢN PHẨM ─────────────────────────────────────────
if ($action === 'remove') {
    $product_id = trim($_POST['product_id'] ?? '');
    $pdo->prepare("DELETE FROM cart WHERE user_id = ? AND product_id = ?")->execute([$uid, $product_id]);

    $total = $pdo->prepare("SELECT COALESCE(SUM(price * quantity), 0) FROM cart WHERE user_id = ?");
    $total->execute([$uid]);

    echo json_encode(['success' => true, 'total' => $total->fetchColumn()]);
    exit;
}

// ── XOÁ TOÀN BỘ GIỎ HÀNG ──────────────────────────────────
if ($action === 'clear') {
    $pdo->prepare("DELETE FROM cart WHERE user_id = ?")->execute([$uid]);
    echo json_encode(['success' => true]);
    exit;
}

echo json_encode(['success' => false, 'message' => 'Hành động không hợp lệ.']);
