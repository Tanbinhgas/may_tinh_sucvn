using Microsoft.EntityFrameworkCore;
using may_tinh_sucvn.Models;
using BCryptNet = BCrypt.Net.BCrypt;

namespace may_tinh_sucvn.Data;

public static class SeedData
{
    /// <summary>
    /// Tạo tài khoản admin đầu tiên từ cấu hình "Seed:Admin" nếu DB đã sẵn sàng
    /// và chưa có admin nào. Nếu không cấu hình Email/Password thì bỏ qua.
    /// </summary>
    public static async Task EnsureAdminAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Chỉ chạy khi DB đã được migrate (tránh crash ở lần chạy đầu trước khi update DB).
        if (!await db.Database.CanConnectAsync()) return;
        if (!(await db.Database.GetAppliedMigrationsAsync()).Any()) return;

        var cfg = app.Configuration.GetSection("Seed:Admin");
        var email = cfg["Email"];
        var password = cfg["Password"];
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) return;

        if (await db.Users.AnyAsync(u => u.Role == UserRole.Admin)) return;

        db.Users.Add(new User
        {
            Username = cfg["Username"] ?? "admin",
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = BCryptNet.HashPassword(password, 12),
            FullName = cfg["FullName"] ?? "Quản Trị Viên",
            Role = UserRole.Admin,
            IsActive = true,
        });
        await db.SaveChangesAsync();
    }

    /// <summary>
    /// Nạp danh sách sản phẩm ban đầu nếu DB đã migrate và bảng Products còn rỗng.
    /// Idempotent: chạy lại nhiều lần cũng không tạo trùng (chỉ seed khi chưa có sản phẩm nào).
    /// </summary>
    public static async Task EnsureProductsAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (!await db.Database.CanConnectAsync()) return;
        if (!(await db.Database.GetAppliedMigrationsAsync()).Any()) return;

        // Đã có sản phẩm thì bỏ qua (không ghi đè dữ liệu admin đã chỉnh sau này).
        if (await db.Products.AnyAsync()) return;

        db.Products.AddRange(SeedProducts.All());
        await db.SaveChangesAsync();
    }
}
