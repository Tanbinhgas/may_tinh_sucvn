namespace may_tinh_sucvn.Services;

using may_tinh_sucvn.Models;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(string username, string email, string password, string fullName, string? phone);
    Task<AuthResult> LoginAsync(string usernameOrEmail, string password);
    Task<User?> GetByIdAsync(int id);
    Task<AuthResult> UpdateProfileAsync(int userId, string? fullName, string? phone, string? address);
    Task<AuthResult> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
}
