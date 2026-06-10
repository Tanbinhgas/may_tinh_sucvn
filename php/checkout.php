<?php
require_once __DIR__ . '/auth.php';
$user = requireLogin(); // chưa đăng nhập → redirect login
require_once __DIR__ . '/db.php';

$msg = '';

// Lấy giỏ hàng
$stmt = $pdo->prepare("SELECT * FROM cart WHERE user_id = ? ORDER BY updated_at DESC");
$stmt->execute([$user['id']]);
$cartItems = $stmt->fetchAll();

// Tính tổng
$total = array_reduce($cartItems, fn($sum, $i) => $sum + $i['price'] * $i['quantity'], 0);

// ── XỬ LÝ ĐẶT HÀNG ─────────────────────────────────────────
// Bùi Minh Tú xử lý đặt hàng (week3) 3.2
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $phone   = trim($_POST['phone']   ?? '');
    $address = trim($_POST['address'] ?? '');
    $note    = trim($_POST['note']    ?? '');

    if (!$phone || !$address) {
        $msg = ['type' => 'error', 'text' => 'Vui lòng điền đầy đủ số điện thoại và địa chỉ.'];
    } elseif (empty($cartItems)) {
        $msg = ['type' => 'error', 'text' => 'Giỏ hàng trống, không thể đặt hàng.'];
    } else {
        // Tạo đơn hàng
        $pdo->prepare("
            INSERT INTO orders (user_id, total_price, shipping_address, phone, note, status)
            VALUES (?, ?, ?, ?, ?, 'pending')
        ")->execute([$user['id'], $total, $address, $phone, $note]);

        $orderId = $pdo->lastInsertId();

        // Lưu chi tiết đơn hàng
        $itemStmt = $pdo->prepare("
            INSERT INTO order_items (order_id, product_id, product_name, price, quantity)
            VALUES (?, ?, ?, ?, ?)
        ");
        foreach ($cartItems as $item) {
            $itemStmt->execute([$orderId, $item['product_id'], $item['name'], $item['price'], $item['quantity']]);
        }

        // Xoá giỏ hàng
        $pdo->prepare("DELETE FROM cart WHERE user_id = ?")->execute([$user['id']]);

        // Redirect trang thành công
        header("Location: /may_tinh_sucvn/php/order_success.php?id=$orderId");
        exit;
    }
}

// Lấy thông tin user để điền sẵn
$userInfo = $pdo->prepare("SELECT * FROM users WHERE id = ?");
$userInfo->execute([$user['id']]);
$userInfo = $userInfo->fetch();
?>
<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Đặt hàng - TKL Computer</title>
    <link rel="icon" href="../images/logo-1.png">
    <link href="../css/bootstrap.css" rel="stylesheet">
    <link href="../css/style.css" rel="stylesheet">
    <link href="../css/font-awesome.css" rel="stylesheet">
    <link href="../css/checkout.css" rel="stylesheet">
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet">
</head>
<body>

<!-- HEADER -->
<div class="header">
    <div class="container">
        <div class="logo">
            <h1><a href="../index.html"><b>P<br>C</b>TKL Computer<span>Prestige is more precious than gold</span></a></h1>
        </div>
        <div class="head-t">
            <ul class="card">
                <li><a href="../index.html"><i class="fa fa-home"></i> Trang chủ</a></li>
                <li><a href="#"><i class="fa fa-user"></i> <?= htmlspecialchars($user['full_name']) ?></a></li>
            </ul>
        </div>
    </div>
</div>

<!-- NỘI DUNG -->
<div class="checkout-wrapper">
    <div class="container">
        <h2 class="checkout-title"><i class="fa fa-shopping-cart"></i> Xác nhận đơn hàng</h2>

        <?php if ($msg): ?>
            <div class="alert alert-<?= $msg['type'] ?>"><?= $msg['text'] ?></div>
        <?php endif; ?>

        <?php if (empty($cartItems)): ?>
            <div class="empty-cart">
                <i class="fa fa-shopping-cart"></i>
                <p>Giỏ hàng của bạn đang trống.</p>
                <a href="../index.html" class="btn-back">← Tiếp tục mua sắm</a>
            </div>
        <?php else: ?>

        <div class="checkout-grid">

            <!-- CỘT TRÁI: Form thông tin -->
            <div class="checkout-form-col">
                <div class="checkout-card">
                    <h3><i class="fa fa-map-marker"></i> Thông tin giao hàng</h3>
                    <form method="POST" id="checkoutForm">
                        <div class="form-group">
                            <label>Họ và tên</label>
                            <input type="text" value="<?= htmlspecialchars($userInfo['full_name']) ?>" disabled class="form-control">
                        </div>
                        <div class="form-group">
                            <label>Số điện thoại <span class="required">*</span></label>
                            <input type="text" name="phone" class="form-control"
                                value="<?= htmlspecialchars($userInfo['phone'] ?? '') ?>"
                                placeholder="0xxxxxxxxx" required>
                        </div>
                        <div class="form-group">
                            <label>Địa chỉ giao hàng <span class="required">*</span></label>
                            <textarea name="address" class="form-control" rows="3"
                                placeholder="Số nhà, đường, phường/xã, quận/huyện, tỉnh/thành phố..."
                                required><?= htmlspecialchars($userInfo['address'] ?? '') ?></textarea>
                        </div>
                        <div class="form-group">
                            <label>Ghi chú cho đơn hàng</label>
                            <textarea name="note" class="form-control" rows="2"
                                placeholder="VD: Gọi trước khi giao, giao buổi chiều..."></textarea>
                        </div>

                        <div class="payment-method">
                            <h4><i class="fa fa-money"></i> Phương thức thanh toán</h4>
                            <label class="payment-option selected">
                                <input type="radio" name="payment" value="cod" checked>
                                <i class="fa fa-money"></i> Thanh toán khi nhận hàng (COD)
                            </label>
                        </div>

                        <button type="submit" class="btn-order">
                            <i class="fa fa-check-circle"></i> Đặt hàng ngay
                        </button>
                    </form>
                </div>
            </div>

            <!-- CỘT PHẢI: Tóm tắt đơn hàng -->
            <div class="checkout-summary-col">
                <div class="checkout-card">
                    <h3><i class="fa fa-list"></i> Đơn hàng của bạn (<?= count($cartItems) ?> sản phẩm)</h3>
                    <ul class="order-items">
                        <?php foreach ($cartItems as $item): ?>
                        <li class="order-item">
                            <img src="<?= htmlspecialchars($item['image'] ?? '') ?>"
                                 alt="<?= htmlspecialchars($item['name']) ?>"
                                 onerror="this.style.display='none'">
                            <div class="order-item-info">
                                <p class="order-item-name"><?= htmlspecialchars($item['name']) ?></p>
                                <p class="order-item-qty">x<?= $item['quantity'] ?></p>
                            </div>
                            <p class="order-item-price">
                                <?= number_format($item['price'] * $item['quantity']) ?>₫
                            </p>
                        </li>
                        <?php endforeach; ?>
                    </ul>

                    <div class="order-totals">
                        <div class="total-row">
                            <span>Tạm tính</span>
                            <span><?= number_format($total) ?>₫</span>
                        </div>
                        <div class="total-row">
                            <span>Phí vận chuyển</span>
                            <span class="free">Miễn phí</span>
                        </div>
                        <div class="total-row grand-total">
                            <span>Tổng cộng</span>
                            <span><?= number_format($total) ?>₫</span>
                        </div>
                    </div>
                </div>

                <a href="../index.html" class="btn-back">← Tiếp tục mua sắm</a>
            </div>
        </div>

        <?php endif; ?>
    </div>
</div>

<!-- FOOTER đơn giản -->
<div class="footer-checkout">
    <p>© 2024 TKL Computer. All rights reserved.</p>
</div>

</body>
</html>
