# TKL Computer

Website thương mại điện tử bán linh kiện máy tính, xây dựng trên **ASP.NET Core
Razor Pages + Entity Framework Core (SQL Server)**, chạy trên **.NET 10**.

> Dự án đã được tái cấu trúc từ kiến trúc lai cũ (frontend ASP.NET + backend
> Node.js/Express port từ PHP) sang **một ứng dụng ASP.NET Core duy nhất** với
> EF Core code-first. Backend Node, admin PHP và các schema SQL viết tay đã được
> loại bỏ; lược đồ cơ sở dữ liệu nay do **EF Migrations** quản lý.

## Kiến trúc

Phân lớp rõ ràng:

```
Models/      # Thực thể EF (User, Category, Product, Order, OrderItem, CartItem, ChatRequest) + Enums
Data/        # AppDbContext, SeedData (admin + sản phẩm), Migrations
Services/    # Nghiệp vụ qua interface + DI:
             #   IAuthService   - đăng ký / đăng nhập / đổi mật khẩu / hồ sơ
             #   ICartService   - giỏ hàng + đặt hàng (CheckoutAsync)
             #   IOrderService  - huỷ đơn + hoàn kho (dùng chung khách & admin)
Controllers/ # API controllers:
             #   ChatbotController - POST /api/Chatbot (Gemini 2.5 Flash, IHttpClientFactory)
Pages/       # Razor Pages — storefront
  Admin/     #   Khu quản trị (khoá theo policy "AdminOnly")
wwwroot/     # CSS / JS / ảnh / fonts
```

## Tính năng

**Khách hàng**
- Trang chủ + danh mục động `/category/{slug}` đọc sản phẩm từ DB.
- Chi tiết sản phẩm `/product/{slug}` kèm sản phẩm liên quan.
- Giỏ hàng, thanh toán, "Đơn hàng của tôi" (huỷ đơn khi đang chờ xác nhận).
- Trang hồ sơ: sửa thông tin cá nhân, đổi mật khẩu.
- **Chatbot AI** tư vấn linh kiện tích hợp Gemini 2.5 Flash (`POST /api/Chatbot`).

**Quản trị** (`/Admin`)
- Bảng điều khiển, quản lý sản phẩm (CRUD), đơn hàng (đổi trạng thái), doanh thu,
  người dùng (khoá/mở, cấp/hạ quyền).

## Bảo mật

- **Chống sửa giá:** `CartItem` chỉ lưu `ProductId` + số lượng; tổng tiền và giá
  từng món luôn tính từ `Product` phía server khi đặt hàng (`CheckoutAsync`).
- Mật khẩu băm bằng **BCrypt** (work factor 12); đăng nhập fail-closed.
- **Antiforgery** tự động trên mọi form POST của Razor Pages.
- **Chống IDOR:** trang xem đơn lọc theo người dùng đăng nhập.
- Đặt hàng và huỷ đơn (hoàn kho) chạy trong **transaction**.
- Không hard-code tài khoản admin hay Gemini API key — seed từ user-secrets/cấu hình.
- `ChatbotController` dùng **`IHttpClientFactory`** (tránh socket exhaustion).

## Công nghệ

- .NET 10 · ASP.NET Core Razor Pages
- Entity Framework Core 10 (SQL Server)
- Xác thực Cookie + phân quyền theo Role
- BCrypt.Net-Next
- Gemini 2.5 Flash API (chatbot)

## Bắt đầu nhanh

Xem chi tiết trong [`SETUP.md`](SETUP.md). Tóm tắt:

```bash
# 1. Cấu hình chuỗi kết nối trong appsettings.json (ConnectionStrings:Default)
# 2. Đặt tài khoản admin và Gemini API key qua user-secrets
dotnet user-secrets set "Seed:Admin:Email"    "admin@tklcomputer.vn"
dotnet user-secrets set "Seed:Admin:Password" "<mật-khẩu-mạnh>"
dotnet user-secrets set "Gemini:ApiKey"       "<api-key-từ-google-ai-studio>"
# 3. Tạo CSDL từ migration
dotnet ef database update
# 4. Chạy
dotnet run
```

## Kiểm thử

```bash
dotnet test tests/may_tinh_sucvn.Tests.csproj
```

Test dùng **SQLite in-memory** (provider quan hệ, hỗ trợ transaction) và **Moq**
phủ các luồng lõi: xác thực, tính giá khi đặt hàng (chống sửa giá), trừ/hoàn kho,
huỷ đơn, và ChatbotController (mock Gemini API).

## Giấy phép

[MIT](LICENSE) © 2026 TKL Computer
