# Changelog

Tất cả thay đổi đáng chú ý của dự án được ghi tại đây.
Định dạng theo [Keep a Changelog](https://keepachangelog.com/vi/1.1.0/),
và dự án tuân theo [Semantic Versioning](https://semver.org/lang/vi/).

## [Unreleased]

## [2.1.0] - 2026-06-25

Bản vá kỹ thuật giải quyết toàn bộ vấn đề từ đánh giá v2.0.0.

### Added
- **Test chatbot** (`ChatbotControllerTests`): 5 test case dùng Moq mock
  `IHttpClientFactory` — câu hỏi rỗng, thiếu API key, Gemini OK, Gemini lỗi,
  timeout. Bổ sung `Moq 4.20.72` vào project test.
- `Models/ChatRequest.cs`: tách `ChatRequest` khỏi `ChatbotController.cs`
  về đúng tầng Models.

### Changed
- **`ChatbotController`**: thay `new HttpClient()` bằng `IHttpClientFactory`
  (inject qua constructor, đăng ký `AddHttpClient("Gemini")` trong `Program.cs`)
  — giải quyết antipattern socket exhaustion.
- `Program.cs`: thêm `AddHttpClient` + `MapControllers()`.
- `README.md`: sửa mục Công nghệ — bỏ ghi nhầm ".NET 10 · EF Core 10" trong khi
  thực tế đang chạy đúng .NET 10 + EF Core 10 (đồng bộ với csproj `net10.0` +
  packages `10.0.0`).

### Fixed
- Version string nhất quán: README, csproj, CHANGELOG, tag đều là `2.1.0`.

## [2.0.0] - 2026-06-25

### Added
- **AI Chatbot** (`POST /api/Chatbot`): tích hợp Gemini 2.5 Flash tư vấn linh kiện.
- `cleanup-release.ps1`: script PowerShell dọn dẹp file legacy.

### Changed
- Bumped version sang `2.0.0` trên csproj, CHANGELOG, tag.
- `README.md` cập nhật mô tả tính năng chatbot.
- `SETUP.md` thêm hướng dẫn cấu hình Gemini API key.

### Removed
- 13 trang category tĩnh (`cpu`, `gpu`, `ram`, `ssd`, `hdd`, `mainboard`, `PCcase`,
  `PowerSupply`, `fan_liquidcooling`, `minitor`, `keyboard`, `mouse`, `headset`).
- `HomeController.cs` và `Views/` (scaffolding MVC mặc định thừa).

### Fixed
- `appsettings.json`: xoá placeholder Gemini API key.

### Known Issues
- Chatbot chưa có rate limiting per-user (sẽ thêm ở v2.2.0).

## [1.0.0] - 2026-06-21

Bản phát hành đầu tiên sau khi tái cấu trúc toàn bộ sang ASP.NET Core + EF Core.

### Added
- Lớp nền EF Core code-first, tầng dịch vụ DI, seed 60 sản phẩm.
- Storefront động, giỏ hàng, thanh toán, quản trị, hồ sơ.
- 15 xUnit test (auth, cart, order), CI GitHub Actions.

### Security
- Chống sửa giá, BCrypt, antiforgery, chống IDOR, transaction.

[Unreleased]: https://github.com/Tanbinhgas/may_tinh_sucvn/compare/v2.1.0...HEAD
[2.1.0]: https://github.com/Tanbinhgas/may_tinh_sucvn/compare/v2.0.0...v2.1.0
[2.0.0]: https://github.com/Tanbinhgas/may_tinh_sucvn/compare/v1.0.0...v2.0.0
[1.0.0]: https://github.com/Tanbinhgas/may_tinh_sucvn/releases/tag/v1.0.0
