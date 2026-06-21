# Changelog

Tất cả thay đổi đáng chú ý của dự án được ghi tại đây.
Định dạng theo [Keep a Changelog](https://keepachangelog.com/vi/1.1.0/),
và dự án tuân theo [Semantic Versioning](https://semver.org/lang/vi/).

## [1.0.0] - 2026-06-21

Bản phát hành đầu tiên sau khi tái cấu trúc toàn bộ sang một ứng dụng
ASP.NET Core + EF Core duy nhất.

### Added
- Lớp nền EF Core code-first: các thực thể `User`, `Category`, `Product`,
  `Order`, `OrderItem`, `CartItem`; `AppDbContext` với index duy nhất, ràng buộc
  khoá ngoại và seed 13 danh mục; migration `InitialCreate`.
- Tầng dịch vụ qua DI: `IAuthService`, `ICartService`, `IOrderService`.
- Seed 60 sản phẩm từ dữ liệu cũ, ánh xạ sang schema EF.
- Storefront động: danh mục `/category/{slug}`, chi tiết `/product/{slug}` kèm
  sản phẩm liên quan.
- Giỏ hàng, luồng thanh toán, trang "Đơn hàng của tôi" (huỷ đơn đang chờ),
  trang hồ sơ (sửa thông tin, đổi mật khẩu).
- Khu quản trị `/Admin`: bảng điều khiển, sản phẩm (CRUD), đơn hàng (đổi trạng
  thái + huỷ kèm hoàn kho), doanh thu, người dùng.
- Xác thực Cookie + phân quyền theo Role; khu `/Admin` khoá bằng policy
  `AdminOnly`.
- Giao diện đăng nhập / đăng ký dùng lại style gốc của site.

### Changed
- Chuyển từ kiến trúc lai (ASP.NET tĩnh + backend Node.js/Express) sang một ứng
  dụng ASP.NET Core Razor Pages duy nhất.
- 13 trang danh mục tĩnh được thay bằng một trang động chung; menu trỏ tới
  `/category/{slug}`.
- Lược đồ CSDL nay do EF Migrations quản lý (nguồn duy nhất).

### Security
- Chống sửa giá: `CartItem` không lưu giá; tổng tiền tính từ `Product` phía
  server trong transaction khi đặt hàng.
- Băm mật khẩu bằng BCrypt (work factor 12); đăng nhập fail-closed.
- Antiforgery token tự động trên các form POST.
- Kiểm tra quyền sở hữu đơn hàng (chống IDOR).
- Huỷ đơn + hoàn kho gom vào `OrderService`, chạy trong transaction, chống hoàn
  kho hai lần.
- Bỏ tài khoản admin mặc định hard-code; seed từ user-secrets/cấu hình.

### Removed
- Backend Node.js/Express (`js-controller/`).
- Admin PHP cũ và các schema SQL viết tay trùng lặp.
- Thành phần React lạc (`src/Login.jsx`).

[1.0.0]: https://example.com/your-repo/releases/tag/v1.0.0
