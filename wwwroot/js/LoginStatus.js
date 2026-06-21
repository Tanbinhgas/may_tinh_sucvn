// ============================================================
//  LoginStatus.js - Hiển thị trạng thái đăng nhập trên navbar
// ============================================================

async function checkLoginStatus() {
    try {
        const res  = await fetch('/api/user?action=status', { credentials: 'include' });
        const data = await res.json();

        const loginLi  = document.getElementById('loginLi');
        const logoutLi = document.getElementById('logoutLi');
        const signLi   = document.getElementById('signLi');

        if (!loginLi || !logoutLi) return;

        if (data.logged_in) {
            // Đã đăng nhập — hiện tên + link đơn hàng
            loginLi.innerHTML = `
                <a href="/profile">
                    <i class="fa fa-user" aria-hidden="true"></i> ${data.full_name}
                </a>`;
            if (signLi) signLi.style.display = 'none';

            logoutLi.style.display = 'inline-block';
            logoutLi.innerHTML = `
                <a href="#" id="btnLogout">
                    <i class="fa fa-sign-out" aria-hidden="true"></i> Đăng Xuất
                </a>`;

            document.getElementById('btnLogout').addEventListener('click', async (e) => {
                e.preventDefault();
                const fd = new FormData();
                fd.append('action', 'logout');
                await fetch('/api/auth', { method: 'POST', body: fd, credentials: 'include' });
                window.location.href = '/';
            });

            // Nếu là admin → thêm link Admin Panel
            if (data.role === 'admin') {
                const adminLi = document.createElement('li');
                adminLi.innerHTML = `
                    <a href="/admin" style="color:#FAB005">
                        <i class="fa fa-cog" aria-hidden="true"></i> Admin
                    </a>`;
                logoutLi.parentNode.insertBefore(adminLi, logoutLi);
            }

        } else {
            // Chưa đăng nhập
            loginLi.innerHTML = `
                <a href="/login">
                    <i class="fa fa-user" aria-hidden="true"></i> Đăng Nhập
                </a>`;
            logoutLi.style.display = 'none';
        }
    } catch (err) {
        console.error('Lỗi kiểm tra đăng nhập:', err);
    }
}

checkLoginStatus();
