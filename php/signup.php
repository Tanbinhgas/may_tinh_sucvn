<?php
require_once __DIR__ . '/auth.php';

$user = getCurrentUser();
if ($user) {
    header('Location: /may_tinh_sucvn/index.html');
    exit;
}
?>
<!DOCTYPE html>
<html lang="vi">
  <head>
    <meta charset="UTF-8">
    <link rel="icon" href="/may_tinh_sucvn/images/logo-1.png">
    <title>Đăng Ký</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="/may_tinh_sucvn/css/styleLogReg.css">
    <link
      href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap"
      rel="stylesheet"
    />
    <style>
      .error-msg   { color: #e74c3c; font-size: 0.9rem; margin-top: -15px; margin-bottom: 10px; }
      .success-msg { color: #27ae60; font-size: 0.9rem; text-align: center; margin-bottom: 10px; }
      .signup__signInButton:disabled { background: #aaa; cursor: not-allowed; }
    </style>
  </head>
  <body>
    <!-- form signup -->
    <div class="signup">
      <div class="signup__container">
        <h1>Đăng Ký</h1>
        <div id="message"></div>
        <form id="signupForm">
          <h5>Email</h5>
          <input type="email" id="email" name="email" class="input-signup-username" />
          <h5>Password</h5>
          <input type="password" id="password" name="password" class="input-signup-password" />
          <button type="submit" id="btnSignup" class="signup__signInButton">Đăng Ký</button>
        </form>
        <a href="/may_tinh_sucvn/php/login.php" class="signup__registerButton">Đăng Nhập</a>
      </div>
    </div>

    <script>
    document.getElementById('signupForm').addEventListener('submit', async function(e) {
        e.preventDefault();
        const btn = document.getElementById('btnSignup');
        const msg = document.getElementById('message');

        const email    = document.getElementById('email').value.trim();
        const password = document.getElementById('password').value;

        if (!email || !password) {
            msg.innerHTML = '<p class="error-msg">Vui lòng điền đầy đủ thông tin.</p>';
            return;
        }
        // Email cơ bản
        if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
            msg.innerHTML = '<p class="error-msg">Email không hợp lệ.</p>';
            return;
        }
        if (password.length < 6) {
            msg.innerHTML = '<p class="error-msg">Mật khẩu tối thiểu 6 ký tự.</p>';
            return;
        }

        // Tự suy ra username + full_name từ email
        // VD: nguyenvana@gmail.com → username = "nguyenvana", full_name = "nguyenvana"
        // User có thể vào trang profile sửa lại sau.
        const prefix    = email.split('@')[0];
        const username  = prefix;
        const full_name = prefix;

        btn.disabled = true;
        btn.textContent = 'Đang xử lý...';

        const formData = new FormData();
        formData.append('action',    'register');
        formData.append('username',  username);
        formData.append('email',     email);
        formData.append('password',  password);
        formData.append('full_name', full_name);
        formData.append('phone',     '');

        try {
            const res = await fetch('/may_tinh_sucvn/php/api_auth.php', { method: 'POST', body: formData });
            const data = await res.json();

            if (data.success) {
                msg.innerHTML = '<p class="success-msg">Đăng ký thành công! Đang chuyển sang đăng nhập...</p>';
                setTimeout(() => window.location.href = '/may_tinh_sucvn/php/login.php', 1200);
            } else {
                msg.innerHTML = `<p class="error-msg">${data.message}</p>`;
                btn.disabled = false;
                btn.textContent = 'Đăng Ký';
            }
        } catch (err) {
            msg.innerHTML = '<p class="error-msg">Lỗi kết nối, vui lòng thử lại.</p>';
            btn.disabled = false;
            btn.textContent = 'Đăng Ký';
        }
    });
    </script>
  </body>
</html>
