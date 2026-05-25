USE tkl_computer;

INSERT INTO users (username, email, password, full_name, role, is_active)
VALUES (
    'admin',
    'admin@tklcomputer.vn',
    '$2y$12$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2uheWG/igi.',
    'Quản Trị Viên',
    'admin',
    1
);

-- Kiểm tra đã tạo chưa
SELECT id, username, email, role, created_at FROM users;
