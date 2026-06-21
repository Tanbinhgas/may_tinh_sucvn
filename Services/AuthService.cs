using Microsoft.EntityFrameworkCore;
using may_tinh_sucvn.Data;
using may_tinh_sucvn.Models;
using BCryptNet = BCrypt.Net.BCrypt;

namespace may_tinh_sucvn.Services;

public class AuthService : IAuthService
{
    private const int BcryptWorkFactor = 12;
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db) => _db = db;

    public async Task<AuthResult> RegisterAsync(string username, string email, string password, string fullName, string? phone)
    {
        username = username.Trim();
        email = email.Trim().ToLowerInvariant();

        var exists = await _db.Users.AnyAsync(u => u.Email == email || u.Username == username);
        if (exists)
            return new AuthResult(false, "Email hoặc tên đăng nhập đã tồn tại.");

        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = BCryptNet.HashPassword(password, BcryptWorkFactor),
            FullName = string.IsNullOrWhiteSpace(fullName) ? username : fullName.Trim(),
            Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim(),
            Role = UserRole.Customer,
            IsActive = true,
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return new AuthResult(true, User: user);
    }

    public async Task<AuthResult> LoginAsync(string usernameOrEmail, string password)
    {
        var id = usernameOrEmail.Trim();
        var user = await _db.Users
            .FirstOrDefaultAsync(u => (u.Username == id || u.Email == id) && u.IsActive);

        // Fail-closed: nếu không có user / sai mật khẩu / lỗi verify -> đều coi là thất bại.
        if (user is null || !BCryptNet.Verify(password, user.PasswordHash))
            return new AuthResult(false, "Sai tên đăng nhập hoặc mật khẩu.");

        return new AuthResult(true, User: user);
    }

    public Task<User?> GetByIdAsync(int id) =>
        _db.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsActive);

    public async Task<AuthResult> UpdateProfileAsync(int userId, string? fullName, string? phone, string? address)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);
        if (user is null) return new AuthResult(false, "Không tìm thấy tài khoản.");

        user.FullName = string.IsNullOrWhiteSpace(fullName) ? null : fullName.Trim();
        user.Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();
        user.Address = string.IsNullOrWhiteSpace(address) ? null : address.Trim();
        user.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return new AuthResult(true, User: user);
    }

    public async Task<AuthResult> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);
        if (user is null) return new AuthResult(false, "Không tìm thấy tài khoản.");

        if (!BCryptNet.Verify(currentPassword, user.PasswordHash))
            return new AuthResult(false, "Mật khẩu hiện tại không đúng.");
        if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
            return new AuthResult(false, "Mật khẩu mới phải từ 6 ký tự trở lên.");

        user.PasswordHash = BCryptNet.HashPassword(newPassword, BcryptWorkFactor);
        user.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return new AuthResult(true, User: user);
    }
}
