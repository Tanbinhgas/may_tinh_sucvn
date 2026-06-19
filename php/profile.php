<?php
require_once __DIR__ . '/auth.php';

$user = requireLogin();

function e(?string $value): string {
    return htmlspecialchars($value ?? '', ENT_QUOTES, 'UTF-8');
}

function formatDate(?string $date): string {
    if (!$date) return 'Chưa cập nhật';
    $timestamp = strtotime($date);
    return $timestamp ? date('d/m/Y', $timestamp) : e($date);
}

$roleLabel = ($user['role'] ?? '') === 'admin' ? 'Quản trị viên' : 'Khách hàng';
?>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <link rel="icon" href="../images/logo-1.png">
    <title>Hồ sơ cá nhân</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href="../css/bootstrap.css" rel="stylesheet" type="text/css">
    <link href="../css/style.css" rel="stylesheet" type="text/css">
    <link href="../css/profile.css" rel="stylesheet" type="text/css">
    <link href="../css/font-awesome.css" rel="stylesheet">
    <script src="../js/jquery-1.11.1.min.js"></script>
    <link href="//fonts.googleapis.com/css?family=Montserrat:400,700" rel="stylesheet" type="text/css">
    <link href="//fonts.googleapis.com/css?family=Noto+Sans:400,700" rel="stylesheet" type="text/css">
</head>
<body>
    <div class="header">
        <div class="container">
            <div class="logo">
                <h1><a href="../index.html"><b>P<br>C</b>TKL Computer<span>Prestige is more precious than gold</span></a></h1>
            </div>
            <div class="head-t">
                <ul class="card">
                    <li id="loginLi"><a href="profile.php"><i class="fa fa-user" aria-hidden="true"></i><?php echo e($user['full_name']); ?></a></li>
                    <li id="signLi" style="display:none;"><a href="signup.php"><i class="fa fa-arrow-right" aria-hidden="true"></i>Đăng Ký</a></li>
                    <li id="logoutLi"><a href="#" id="btnProfileLogout"><i class="fa fa-sign-out" aria-hidden="true"></i>Đăng Xuất</a></li>
                </ul>
            </div>
            <div class="header-ri">
                <ul class="social-top">
                    <li><a href="#" class="icon facebook"><i class="fa fa-facebook" aria-hidden="true"></i><span></span></a></li>
                </ul>
            </div>
            <div class="nav-top">
                <nav class="navbar navbar-default">
                    <div class="navbar-header nav_2">
                        <button type="button" class="navbar-toggle collapsed navbar-toggle1" data-toggle="collapse" data-target="#bs-megadropdown-tabs">
                            <span class="sr-only">Toggle navigation</span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                        </button>
                    </div>
                    <div class="collapse navbar-collapse" id="bs-megadropdown-tabs">
                        <ul class="nav navbar-nav">
                            <li><a href="../index.html" class="hyper"><span>Trang Chủ</span></a></li>
                            <li class="dropdown">
                                <a href="#" class="dropdown-toggle hyper" data-toggle="dropdown"><span>Linh Kiện PC<b class="caret"></b></span></a>
                                <ul class="dropdown-menu multi">
                                    <div class="row">
                                        <div class="col-sm-3">
                                            <ul class="multi-column-dropdown">
                                                <li><a href="../pages/mainboard.html">Mainboard</a></li>
                                                <li><a href="../pages/cpu.html">CPU</a></li>
                                                <li><a href="../pages/ram.html">RAM</a></li>
                                                <li><a href="../pages/gpu.html">GPU</a></li>
                                            </ul>
                                        </div>
                                        <div class="col-sm-3">
                                            <ul class="multi-column-dropdown">
                                                <li><a href="../pages/ssd.html">SSD</a></li>
                                                <li><a href="../pages/hdd.html">HDD</a></li>
                                                <li><a href="../pages/PowerSupply.html">Power Supply</a></li>
                                                <li><a href="../pages/PCcase.html">PC Case</a></li>
                                                <li><a href="../pages/fan_liquidcooling.html">Fan/Liquid Cooling</a></li>
                                            </ul>
                                        </div>
                                        <div class="clearfix"></div>
                                    </div>
                                </ul>
                            </li>
                            <li class="dropdown">
                                <a href="#" class="dropdown-toggle hyper" data-toggle="dropdown"><span>Gaming Gear<b class="caret"></b></span></a>
                                <ul class="dropdown-menu multi multi1">
                                    <div class="row">
                                        <div class="col-sm-3">
                                            <ul class="multi-column-dropdown">
                                                <li><a href="../pages/minitor.html">Màn Hình</a></li>
                                                <li><a href="../pages/keyboard.html">Bàn Phím</a></li>
                                            </ul>
                                        </div>
                                        <div class="col-sm-3">
                                            <ul class="multi-column-dropdown">
                                                <li><a href="../pages/mouse.html">Chuột</a></li>
                                                <li><a href="../pages/headset.html">Tai Nghe</a></li>
                                            </ul>
                                        </div>
                                        <div class="clearfix"></div>
                                    </div>
                                </ul>
                            </li>
                            <li><a href="../pages/contact.html" class="hyper"><span>Liên Hệ</span></a></li>
                        </ul>
                    </div>
                </nav>
                <div class="cart">
                    <span class="fa fa-shopping-cart my-cart-icon"><span class="badge badge-notify my-cart-badge"></span></span>
                </div>
                <div class="clearfix"></div>
            </div>
        </div>
    </div>

    <main class="profile-page">
        <div class="container">
            <section class="profile-panel">
                <div class="profile-summary">
                    <div class="profile-avatar" aria-hidden="true">
                        <?php echo strtoupper(substr(e($user['full_name'] ?: $user['username']), 0, 1)); ?>
                    </div>
                    <div>
                        <p class="profile-kicker">Hồ sơ cá nhân</p>
                        <h2><?php echo e($user['full_name']); ?></h2>
                        <span class="role-badge <?php echo ($user['role'] ?? '') === 'admin' ? 'is-admin' : ''; ?>">
                            <?php echo e($roleLabel); ?>
                        </span>
                    </div>
                </div>

                <div class="profile-info-grid">
                    <div class="profile-info-item">
                        <span>Họ và tên</span>
                        <strong><?php echo e($user['full_name']); ?></strong>
                    </div>
                    <div class="profile-info-item">
                        <span>Email</span>
                        <strong><?php echo e($user['email'] ?? 'Chưa cập nhật'); ?></strong>
                    </div>
                    <div class="profile-info-item">
                        <span>Vai trò</span>
                        <strong><?php echo e($roleLabel); ?></strong>
                    </div>
                    <div class="profile-info-item">
                        <span>Ngày tạo</span>
                        <strong><?php echo formatDate($user['created_at'] ?? null); ?></strong>
                    </div>
                </div>
            </section>

            <section class="profile-tools" aria-label="Bộ công cụ quản trị tài khoản">
                <a class="profile-tool" href="#">
                    <i class="fa fa-pencil" aria-hidden="true"></i>
                    <span><strong>Chỉnh sửa</strong><small>Cập nhật thông tin cá nhân</small></span>
                </a>
                <a class="profile-tool" href="#">
                    <i class="fa fa-lock" aria-hidden="true"></i>
                    <span><strong>Bảo mật</strong><small>Đổi mật khẩu và cấu hình 2FA</small></span>
                </a>
                <a class="profile-tool" href="#">
                    <i class="fa fa-bell" aria-hidden="true"></i>
                    <span><strong>Thông báo</strong><small>Cấu hình email hệ thống</small></span>
                </a>
                <a class="profile-tool" href="#">
                    <i class="fa fa-clock-o" aria-hidden="true"></i>
                    <span><strong>Nhật ký</strong><small>Xem lịch sử đăng nhập và hoạt động</small></span>
                </a>
            </section>

            <section class="profile-actions">
                <button type="button" class="btn-logout-profile" id="btnLogoutMain">
                    <i class="fa fa-sign-out" aria-hidden="true"></i>
                    Đăng xuất
                </button>
            </section>
        </div>
    </main>

    <script>
        async function logoutFromProfile(event) {
            event.preventDefault();
            const fd = new FormData();
            fd.append('action', 'logout');
            await fetch('/may_tinh_sucvn/php/api_auth.php', { method: 'POST', body: fd });
            window.location.href = '/may_tinh_sucvn/index.html';
        }

        document.getElementById('btnProfileLogout').addEventListener('click', logoutFromProfile);
        document.getElementById('btnLogoutMain').addEventListener('click', logoutFromProfile);
    </script>
    <script src="../js/bootstrap.js"></script>
</body>
</html>
