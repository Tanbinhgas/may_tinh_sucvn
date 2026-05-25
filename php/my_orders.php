<?php
require_once __DIR__ . '/auth.php';
$user = requireLogin();
require_once __DIR__ . '/db.php';

// Lấy tất cả đơn hàng của user
$orders = $pdo->prepare("
    SELECT o.*, 
           COUNT(oi.id) AS item_count
    FROM orders o
    LEFT JOIN order_items oi ON oi.order_id = o.id
    WHERE o.user_id = ?
    GROUP BY o.id
    ORDER BY o.created_at DESC
");
$orders->execute([$user['id']]);
$orders = $orders->fetchAll();

function statusLabel($s) {
    return match($s) {
        'pending'   => ['label' => 'Chờ xác nhận', 'class' => 'status-pending'],
        'confirmed' => ['label' => 'Đã xác nhận',  'class' => 'status-confirmed'],
        'shipping'  => ['label' => 'Đang giao',     'class' => 'status-shipping'],
        'done'      => ['label' => 'Hoàn thành',    'class' => 'status-done'],
        'cancelled' => ['label' => 'Đã huỷ',        'class' => 'status-cancelled'],
        default     => ['label' => $s,              'class' => ''],
    };
}
?>
<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Đơn hàng của tôi - TKL Computer</title>
    <link rel="icon" href="../images/logo-1.png">
    <link href="../css/bootstrap.css" rel="stylesheet">
    <link href="../css/style.css" rel="stylesheet">
    <link href="../css/font-awesome.css" rel="stylesheet">
    <link href="../css/my_orders.css" rel="stylesheet">
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
                <li><a href="#" onclick="doLogout()"><i class="fa fa-sign-out"></i> Đăng xuất</a></li>
            </ul>
        </div>
    </div>
</div>

<div class="orders-wrapper">
    <div class="container">

        <div class="orders-header">
            <h2><i class="fa fa-list-alt"></i> Đơn hàng của tôi</h2>
            <a href="../index.html" class="btn-shop"><i class="fa fa-shopping-cart"></i> Tiếp tục mua sắm</a>
        </div>

        <?php if (empty($orders)): ?>
            <div class="empty-orders">
                <i class="fa fa-inbox"></i>
                <p>Bạn chưa có đơn hàng nào.</p>
                <a href="../index.html" class="btn-primary-cta">Mua sắm ngay</a>
            </div>
        <?php else: ?>

        <div class="orders-list">
            <?php foreach ($orders as $order):
                $status = statusLabel($order['status']);
            ?>
            <div class="order-card">
                <div class="order-card-header">
                    <div class="order-meta">
                        <span class="order-id">#<?= $order['id'] ?></span>
                        <span class="order-date">
                            <i class="fa fa-calendar"></i>
                            <?= date('d/m/Y H:i', strtotime($order['created_at'])) ?>
                        </span>
                        <span class="order-items-count">
                            <i class="fa fa-cubes"></i>
                            <?= $order['item_count'] ?> sản phẩm
                        </span>
                    </div>
                    <span class="order-status <?= $status['class'] ?>"><?= $status['label'] ?></span>
                </div>

                <div class="order-card-body">
                    <!-- Chi tiết sản phẩm (ẩn/hiện) -->
                    <div class="order-items-preview" id="items-<?= $order['id'] ?>" style="display:none">
                        <?php
                        $items = $pdo->prepare("SELECT * FROM order_items WHERE order_id = ?");
                        $items->execute([$order['id']]);
                        foreach ($items->fetchAll() as $item): ?>
                        <div class="order-item-row">
                            <span class="item-name"><?= htmlspecialchars($item['product_name']) ?></span>
                            <span class="item-qty">x<?= $item['quantity'] ?></span>
                            <span class="item-price"><?= number_format($item['price'] * $item['quantity']) ?>₫</span>
                        </div>
                        <?php endforeach; ?>
                    </div>

                    <div class="order-card-footer">
                        <div class="order-info">
                            <div><i class="fa fa-map-marker"></i> <?= htmlspecialchars($order['shipping_address']) ?></div>
                            <div><i class="fa fa-phone"></i> <?= htmlspecialchars($order['phone']) ?></div>
                            <?php if ($order['note']): ?>
                            <div><i class="fa fa-sticky-note"></i> <?= htmlspecialchars($order['note']) ?></div>
                            <?php endif; ?>
                        </div>
                        <div class="order-actions">
                            <div class="order-total">
                                Tổng: <b><?= number_format($order['total_price']) ?>₫</b>
                            </div>
                            <button class="btn-toggle" onclick="toggleItems(<?= $order['id'] ?>)">
                                <i class="fa fa-eye" id="icon-<?= $order['id'] ?>"></i>
                                <span id="label-<?= $order['id'] ?>">Xem sản phẩm</span>
                            </button>
                            <?php if ($order['status'] === 'pending'): ?>
                                <button class="btn-cancel" onclick="cancelOrder(<?= $order['id'] ?>)">
                                    <i class="fa fa-times"></i> Huỷ đơn
                                </button>
                            <?php endif; ?>
                            <?php if ($order['status'] === 'done'): ?>
                                <span class="btn-done"><i class="fa fa-check-circle"></i> Đã nhận hàng</span>
                            <?php endif; ?>
                        </div>
                    </div>
                </div>
            </div>
            <?php endforeach; ?>
        </div>

        <?php endif; ?>
    </div>
</div>

<div class="footer-checkout">
    <p>© 2024 TKL Computer. All rights reserved.</p>
</div>

<script>
function toggleItems(orderId) {
    const el    = document.getElementById('items-' + orderId);
    const icon  = document.getElementById('icon-' + orderId);
    const label = document.getElementById('label-' + orderId);
    const open  = el.style.display === 'none';
    el.style.display   = open ? 'block' : 'none';
    icon.className     = open ? 'fa fa-eye-slash' : 'fa fa-eye';
    label.textContent  = open ? 'Ẩn sản phẩm' : 'Xem sản phẩm';
}

async function cancelOrder(orderId) {
    if (!confirm('Bạn có chắc muốn huỷ đơn hàng #' + orderId + '?')) return;
    const fd = new FormData();
    fd.append('action',   'cancel');
    fd.append('order_id', orderId);
    const res  = await fetch('/may_tinh_sucvn/php/api_orders.php', { method: 'POST', body: fd });
    const data = await res.json();
    if (data.success) {
        location.reload();
    } else {
        alert(data.message);
    }
}

async function doLogout() {
    const fd = new FormData();
    fd.append('action', 'logout');
    await fetch('/may_tinh_sucvn/php/api_auth.php', { method: 'POST', body: fd });
    window.location.href = '/may_tinh_sucvn/php/login.php';
}
</script>
</body>
</html>
