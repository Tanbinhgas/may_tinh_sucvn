<?php
require_once __DIR__ . '/auth.php';
$user = requireLogin();
require_once __DIR__ . '/db.php';

$orderId = (int)($_GET['id'] ?? 0);

// Lấy thông tin đơn hàng (chỉ xem đơn của mình)
$stmt = $pdo->prepare("
    SELECT o.*, u.full_name FROM orders o
    JOIN users u ON u.id = o.user_id
    WHERE o.id = ? AND o.user_id = ?
");
$stmt->execute([$orderId, $user['id']]);
$order = $stmt->fetch();

if (!$order) {
    header('Location: /may_tinh_sucvn/index.html');
    exit;
}

// Lấy chi tiết sản phẩm trong đơn
$items = $pdo->prepare("SELECT * FROM order_items WHERE order_id = ?");
$items->execute([$orderId]);
$items = $items->fetchAll();
?>
<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Đặt hàng thành công - TKL Computer</title>
    <link rel="icon" href="../images/logo-1.png">
    <link href="../css/bootstrap.css" rel="stylesheet">
    <link href="../css/style.css" rel="stylesheet">
    <link href="../css/font-awesome.css" rel="stylesheet">
    <link href="../css/checkout.css" rel="stylesheet">
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet">
</head>
<body>

<div class="header">
    <div class="container">
        <div class="logo">
            <h1><a href="../index.html"><b>P<br>C</b>TKL Computer<span>Prestige is more precious than gold</span></a></h1>
        </div>
    </div>
</div>

<div class="checkout-wrapper">
    <div class="container">
        <div class="success-box">
            <div class="success-icon">✅</div>
            <h2>Đặt hàng thành công!</h2>
            <p>Cảm ơn bạn đã mua hàng tại <b>TKL Computer</b>.</p>
            <p>Chúng tôi sẽ liên hệ xác nhận đơn hàng sớm nhất.</p>

            <div class="order-info-box">
                <div class="order-info-row">
                    <span>Mã đơn hàng</span>
                    <b>#<?= $order['id'] ?></b>
                </div>
                <div class="order-info-row">
                    <span>Khách hàng</span>
                    <b><?= htmlspecialchars($order['full_name']) ?></b>
                </div>
                <div class="order-info-row">
                    <span>Số điện thoại</span>
                    <b><?= htmlspecialchars($order['phone']) ?></b>
                </div>
                <div class="order-info-row">
                    <span>Địa chỉ giao hàng</span>
                    <b><?= htmlspecialchars($order['shipping_address']) ?></b>
                </div>
                <div class="order-info-row">
                    <span>Trạng thái</span>
                    <span class="badge-pending">Chờ xác nhận</span>
                </div>
            </div>

            <!-- Chi tiết sản phẩm -->
            <div class="order-detail-table">
                <h4>Chi tiết đơn hàng</h4>
                <table>
                    <thead>
                        <tr><th>Sản phẩm</th><th>SL</th><th>Đơn giá</th><th>Thành tiền</th></tr>
                    </thead>
                    <tbody>
                        <?php foreach ($items as $item): ?>
                        <tr>
                            <td><?= htmlspecialchars($item['product_name']) ?></td>
                            <td><?= $item['quantity'] ?></td>
                            <td><?= number_format($item['price']) ?>₫</td>
                            <td><?= number_format($item['price'] * $item['quantity']) ?>₫</td>
                        </tr>
                        <?php endforeach; ?>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="3"><b>Tổng cộng</b></td>
                            <td><b><?= number_format($order['total_price']) ?>₫</b></td>
                        </tr>
                    </tfoot>
                </table>
            </div>

            <div class="success-actions">
                <a href="/may_tinh_sucvn/php/my_orders.php" class="btn-order" style="background:#039445;margin-bottom:10px">
                    📋 Xem lịch sử đơn hàng
                </a>
                <a href="../index.html" class="btn-order">← Tiếp tục mua sắm</a>
            </div>
        </div>
    </div>
</div>

<div class="footer-checkout">
    <p>© 2024 TKL Computer. All rights reserved.</p>
</div>

</body>
</html>
