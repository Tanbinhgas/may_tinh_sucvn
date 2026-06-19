<?php
// ============================================================
//  api_cart.php - API giỏ hàng, kiểm tra tồn kho
// ============================================================

header('Content-Type: application/json; charset=utf-8');
require_once __DIR__ . '/auth.php';
require_once __DIR__ . '/db.php';

$user = getCurrentUser();
$action = $_POST['action'] ?? $_GET['action'] ?? '';

if (!$user) {
    echo json_encode(['success' => false, 'message' => 'Vui lòng đăng nhập để dùng giỏ hàng.', 'require_login' => true]);
    exit;
}

$uid = $user['id'];

// ── LẤY GIỎ HÀNG (kèm stock) ──────────────────────────────
if ($action === 'get') {
    $items = $pdo->prepare("
        SELECT c.*, p.stock 
        FROM cart c 
        LEFT JOIN products p ON c.product_id = p.id 
        WHERE c.user_id = ? 
        ORDER BY c.updated_at DESC
    ");
    $items->execute([$uid]);
    $cartItems = $items->fetchAll();

    $total = 0;
    foreach ($cartItems as $item) {
        $total += $item['price'] * $item['quantity'];
    }

    echo json_encode([
        'success' => true,
        'items' => $cartItems,
        'total' => $total
    ]);
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

    $checkStock = $pdo->prepare("SELECT stock FROM products WHERE id = ?");
    $checkStock->execute([$product_id]);
    $stock = $checkStock->fetchColumn();

    $currentQty = $pdo->prepare("SELECT quantity FROM cart WHERE user_id = ? AND product_id = ?");
    $currentQty->execute([$uid, $product_id]);
    $existingQty = $currentQty->fetchColumn() ?: 0;

    $newTotalQty = $existingQty + $quantity;

    if ($stock !== false && $newTotalQty > $stock) {
        echo json_encode([
            'success' => false,
            'message' => "Sản phẩm chỉ còn $stock sản phẩm trong kho.",
            'stock' => $stock
        ]);
        exit;
    }

    $pdo->prepare("
        INSERT INTO cart (user_id, product_id, name, price, quantity, image, summary)
        VALUES (?, ?, ?, ?, ?, ?, ?)
        ON DUPLICATE KEY UPDATE quantity = quantity + VALUES(quantity)
    ")->execute([$uid, $product_id, $name, $price, $quantity, $image, $summary]);

    $count = $pdo->prepare("SELECT COUNT(*) FROM cart WHERE user_id = ?");
    $count->execute([$uid]);

    echo json_encode(['success' => true, 'message' => 'Đã thêm vào giỏ hàng!', 'cart_count' => $count->fetchColumn()]);
    exit;
}

// ── CẬP NHẬT SỐ LƯỢNG ──────────────────────────────────────
if ($action === 'update') {
    $product_id = trim($_POST['product_id'] ?? '');
    $quantity   = (int)($_POST['quantity']  ?? 0);

    $checkStock = $pdo->prepare("SELECT stock FROM products WHERE id = ?");
    $checkStock->execute([$product_id]);
    $stock = $checkStock->fetchColumn();

    if ($stock !== false && $quantity > $stock) {
        echo json_encode([
            'success' => false,
            'message' => "Kho chỉ còn $stock sản phẩm.",
            'stock' => $stock
        ]);
        exit;
    }

    if ($quantity <= 0) {
        $pdo->prepare("DELETE FROM cart WHERE user_id = ? AND product_id = ?")->execute([$uid, $product_id]);
    } else {
        $pdo->prepare("UPDATE cart SET quantity = ? WHERE user_id = ? AND product_id = ?")->execute([$quantity, $uid, $product_id]);
    }

    $total = $pdo->prepare("SELECT COALESCE(SUM(price * quantity), 0) FROM cart WHERE user_id = ?");
    $total->execute([$uid]);

    $count = $pdo->prepare("SELECT COUNT(*) FROM cart WHERE user_id = ?");
    $count->execute([$uid]);

    echo json_encode([
        'success' => true,
        'total' => $total->fetchColumn(),
        'cart_count' => $count->fetchColumn(),
        'stock' => $stock
    ]);
    exit;
}

// ── XOÁ 1 SẢN PHẨM ─────────────────────────────────────────
if ($action === 'remove') {
    $product_id = trim($_POST['product_id'] ?? '');
    $pdo->prepare("DELETE FROM cart WHERE user_id = ? AND product_id = ?")->execute([$uid, $product_id]);

    $total = $pdo->prepare("SELECT COALESCE(SUM(price * quantity), 0) FROM cart WHERE user_id = ?");
    $total->execute([$uid]);

    $count = $pdo->prepare("SELECT COUNT(*) FROM cart WHERE user_id = ?");
    $count->execute([$uid]);

    echo json_encode([
        'success' => true,
        'total' => $total->fetchColumn(),
        'cart_count' => $count->fetchColumn()
    ]);
    exit;
}

// ── XOÁ TOÀN BỘ GIỎ HÀNG ──────────────────────────────────
if ($action === 'clear') {
    $pdo->prepare("DELETE FROM cart WHERE user_id = ?")->execute([$uid]);
    echo json_encode(['success' => true]);
    exit;
}

echo json_encode(['success' => false, 'message' => 'Hành động không hợp lệ.']);
