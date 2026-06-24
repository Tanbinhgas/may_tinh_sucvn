# Changelog

Tất cả thay đổi đáng chú ý của dự án được ghi tại đây.
Định dạng theo [Keep a Changelog](https://keepachangelog.com/vi/1.1.0/),
và dự án tuân theo [Semantic Versioning](https://semver.org/lang/vi/).

## [2.0.0] - 2026-06-24

Bổ sung tính năng chatbot AI tư vấn linh kiện máy tính sử dụng Gemini 2.5 Flash,
đồng thời dọn dẹp các trang danh mục tĩnh legacy còn sót lại từ kiến trúc cũ.

### Added
- **AI Chatbot** (`/api/Chatbot`): tích hợp Gemini 2.5 Flash qua REST API cho phép
  khách hàng hỏi tư vấn về cấu hình linh kiện máy tính trực tiếp trên trang web.
  - Endpoint `POST /api/Chatbot` nhận `{ "question": "..." }`, trả về câu trả lời
    từ mô hình Gemini.
  - Validation đầu vào: từ chối câu hỏi rỗng, trả về thông báo lỗi rõ ràng.
  - Timeout 30 giây; xử lý lỗi HTTP và exception đầy đủ.
  - API key đọc từ cấu hình `Gemini:ApiKey` — cần đặt qua user-secrets (không
    commit key thật vào repo).
- `cleanup-release.ps1`: script PowerShell tự động xoá các file legacy trước khi
  phát hành (13 trang category tĩnh, scaffolding MVC thừa, cache `.claude`).

### Changed
- `Program.cs`: đăng ký `AddControllers()`, `AddHttpClient()`, `MapControllers()`
  để hỗ trợ API Controller bên cạnh Razor Pages hiện có.
- `appsettings.json`: thêm placeholder `Gemini:ApiKey` với giá trị rỗng — nhắc
  nhở developer phải cấu hình qua user-secrets trước khi chạy chatbot.

### Removed
- 13 trang danh mục tĩnh legacy (`cpu`, `gpu`, `ram`, `ssd`, `hdd`, `mainboard`,
  `PCcase`, `PowerSupply`, `fan_liquidcooling`, `minitor`, `keyboard`, `mouse`,
  `headset`) — đã được thay thế hoàn toàn bởi `Category.cshtml` động từ v1.0.0,
  nay chính thức xoá khỏi codebase.
- `HomeController.cs` và thư mục `Views/` (scaffolding MVC mặc định không còn
  cần thiết trong ứng dụng Razor Pages thuần).

### Fixed
- Sửa placeholder link `[1.0.0]` trong CHANGELOG từ `example.com` sang đúng URL
  GitHub Release.
- Cập nhật `SETUP.md`: xoá mục "Phần CÒN LẠI (chưa viết)" vì toàn bộ tính năng
  đã được implement từ v1.0.0; thêm hướng dẫn cấu hình Gemini API key.

---

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

---

[2.0.0]: https://github.com/Tanbinhgas/may_tinh_sucvn/releases/tag/v2.0.0
[1.0.0]: https://github.com/Tanbinhgas/may_tinh_sucvn/releases/tag/v1.0.0
