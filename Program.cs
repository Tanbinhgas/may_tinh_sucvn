using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using may_tinh_sucvn.Data;
using may_tinh_sucvn.Models;
using may_tinh_sucvn.Services;

var builder = WebApplication.CreateBuilder(args);

// ── EF Core / SQL Server ───────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// ── Xác thực bằng cookie + phân quyền theo Role ────────────
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Login";
        opt.AccessDeniedPath = "/Login";
        opt.ExpireTimeSpan = TimeSpan.FromDays(7);
        opt.SlidingExpiration = true;
        opt.Cookie.Name = "tkl.auth";
        opt.Cookie.HttpOnly = true;
        opt.Cookie.SameSite = SameSiteMode.Lax;
        // opt.Cookie.SecurePolicy = CookieSecurePolicy.Always; // bật khi chạy HTTPS thật
    });

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("AdminOnly", p => p.RequireRole(nameof(UserRole.Admin)));
});

// ── Razor Pages + bảo vệ theo thư mục/trang ────────────────
builder.Services.AddRazorPages(opt =>
{
    opt.Conventions.AuthorizeFolder("/Admin", "AdminOnly");
    opt.Conventions.AuthorizePage("/Cart");
    opt.Conventions.AuthorizePage("/Checkout");
    opt.Conventions.AuthorizePage("/Profile");
    opt.Conventions.AuthorizePage("/MyOrders");
    opt.Conventions.AuthorizePage("/OrderSuccess");
});

// ── Controllers (cho API chatbot) ──────────────────────────
builder.Services.AddControllers();  // ← THÊM DÒNG NÀY

// ── Dependency Injection: business services ────────────────
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddHttpContextAccessor();

// ── HttpClient cho Chatbot ──────────────────────────────────
builder.Services.AddHttpClient();   // ← THÊM DÒNG NÀY

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();
app.MapControllers();  // ← THÊM DÒNG NÀY (để API hoạt động)

// (Tuỳ chọn) tạo admin đầu tiên từ cấu hình "Seed:Admin" nếu DB chưa có admin nào.
// Không có credential mặc định nào bị hard-code — phải tự đặt trong user-secrets/appsettings.
await SeedData.EnsureAdminAsync(app);

// Nạp danh mục sản phẩm ban đầu (chỉ chạy khi bảng Products còn rỗng).
await SeedData.EnsureProductsAsync(app);

app.Run();