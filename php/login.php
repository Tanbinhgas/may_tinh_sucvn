<?php
require_once __DIR__ . '/auth.php';

// Nếu đã đăng nhập rồi thì redirect về trang chủ
$user = getCurrentUser();
if ($user) {
    header('Location: /may_tinh_sucvn/index.html');
    exit;
}
?>
<!DOCTYPE html>
<html lang="vi">
  <head>
    <meta charset="UTF-8" />
    <link rel="icon" href="/may_tinh_sucvn/images/logo-1.png">
    <title>Đăng Nhập</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="stylesheet" href="/may_tinh_sucvn/css/styleLogReg.css" />
    <link
      href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap"
      rel="stylesheet"
    />
    <style>
      .error-msg   { color: #e74c3c; font-size: 0.9rem; margin-top: -15px; margin-bottom: 10px; }
      .success-msg { color: #27ae60; font-size: 0.9rem; text-align: center; margin-bottom: 10px; }
      .login__signInButton:disabled { background: #aaa; cursor: not-allowed; }
    </style>
  </head>
  <body>
    <!-- from login -->
    <div class="login">
      <div class="login__container">
        <h1>Đăng Nhập</h1>
        <div id="message"></div>
        <form id="loginForm">
          <h5>Email</h5>
          <input type="text" id="username" name="username" class="input-login-username">
          <h5>Password</h5>
          <input type="password" id="password" name="password" class="input-login-password">
          <button type="submit" id="btnLogin" class="login__signInButton">Đăng Nhập</button>
        </form>
        <a href="/may_tinh_sucvn/php/signup.php" class="login__registerButton"
          >Tạo tài khoản mới</a
        >
      </div>
    </div>

    <script>
    document.getElementById('loginForm').addEventListener('submit', async function(e) {
        e.preventDefault();
        const btn = document.getElementById('btnLogin');
        const msg = document.getElementById('message');
        const username = document.getElementById('username').value.trim();
        const password = document.getElementById('password').value;

        if (!username || !password) {
            msg.innerHTML = '<p class="error-msg">Vui lòng điền đầy đủ thông tin.</p>';
            return;
        }

        btn.disabled = true;
        btn.textContent = 'Đang đăng nhập...';

        const formData = new FormData();
        formData.append('action', 'login');
        formData.append('username', username);
        formData.append('password', password);

        try {
            const res = await fetch('/may_tinh_sucvn/php/api_auth.php', { method: 'POST', body: formData });
            const data = await res.json();

            if (data.success) {
                msg.innerHTML = '<p class="success-msg">Đăng nhập thành công! Đang chuyển trang...</p>';
                setTimeout(() => {
                    window.location.href = data.role === 'admin' ? '/may_tinh_sucvn/admin/index.php' : '/may_tinh_sucvn/index.html';
                }, 800);
            } else {
                msg.innerHTML = `<p class="error-msg">${data.message}</p>`;
                btn.disabled = false;
                btn.textContent = 'Đăng Nhập';
            }
        } catch (err) {
            msg.innerHTML = '<p class="error-msg">Lỗi kết nối, vui lòng thử lại.</p>';
            btn.disabled = false;
            btn.textContent = 'Đăng Nhập';
        }
    });
    </script>
  </body>
</html>
