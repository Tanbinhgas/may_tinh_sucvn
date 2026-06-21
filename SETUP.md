# TKL Computer — Kiến trúc ASP.NET Core + EF Core (foundation)

Lớp nền chuẩn DI/EF Core. **Sandbox không có .NET SDK nên chưa tạo migration thật** —
các lệnh dưới đây bạn chạy trên máy có `dotnet`.

## 1. Cài package
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package BCrypt.Net-Next
dotnet tool install --global dotnet-ef   # nếu chưa có
```
(`may_tinh_sucvn.csproj` đã liệt kê sẵn — điều chỉnh số version cho khớp .NET 10 đã cài.)

## 2. Cấu hình kết nối + admin
- Sửa `ConnectionStrings:Default` trong `appsettings.json`.
- Đặt `Seed:Admin:Email` + `Seed:Admin:Password` **trong user-secrets** (không commit):
```bash
dotnet user-secrets init
dotnet user-secrets set "Seed:Admin:Email" "admin@tklcomputer.vn"
dotnet user-secrets set "Seed:Admin:Password" "<mat-khau-manh>"
```
Admin chỉ được tạo nếu có cấu hình này và DB chưa có admin — không còn credential mặc định hard-code.

## 3. Migration (EF làm nguồn sự thật duy nhất)
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
Migration sinh ra bảng users / categories (seed 13 danh mục) / products / orders / order_items / cart_items
với unique index (email, username, slug, (user_id, product_id)) đúng như cấu hình trong AppDbContext.

## 4. Chạy
```bash
dotnet run
```

## 5. Dọn dẹp (xoá khỏi repo)
- `js-controller/` (toàn bộ backend Node + node_modules)
- `src/Login.jsx`
- `bin/`, `obj/`  → thêm vào `.gitignore`
- `database/database_schema.sql` (MySQL) và `database/sqlserver_schema.sql` (viết tay)
  — EF Migrations thay thế hoàn toàn.

## Những bản vá bảo mật đã cài sẵn vào thiết kế
- **Chống price-tampering:** `CartItem` chỉ lưu `ProductId` + `Quantity`; giá/tên luôn lấy từ
  `Product`; `CheckoutAsync` tính tổng từ giá server trong 1 transaction.
- **Auth fail-closed:** `LoginAsync` chỉ chấp nhận user `IsActive` và verify bcrypt thành công.
- **CSRF:** Razor Pages tự động chèn + kiểm tra antiforgery token trên mọi form POST.
- **Phân quyền:** Cookie auth + policy `AdminOnly`; cả thư mục `/Admin` được bảo vệ theo Role.
- **SQL injection:** EF Core tham số hoá toàn bộ truy vấn.

## Phần CÒN LẠI (chưa viết — phase tiếp theo)
- `Pages/Admin/`: Index (dashboard), Products, Orders, Revenue, Users.
- `Pages/Checkout`, `Pages/MyOrders`, `Pages/OrderSuccess`, `Pages/Profile`.
- `Pages/Category/[slug].cshtml` hợp nhất → sau đó xoá 13 file cũ (cpu/gpu/ram/...).
- (Nên có) `Pages/Shared/_Layout.cshtml` chung; hiện mỗi trang đang `Layout = null` để chạy độc lập.
