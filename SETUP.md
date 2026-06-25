# TKL Computer — Hướng dẫn cài đặt (v2.0.0)

Website thương mại điện tử linh kiện máy tính, xây dựng trên **ASP.NET Core Razor Pages + EF Core (SQL Server)**. Phiên bản v2.0.0 bổ sung tính năng **AI Chatbot** tư vấn linh kiện tích hợp Gemini 2.5 Flash.

---

## Yêu cầu

| Thành phần | Phiên bản tối thiểu |
|---|---|
| .NET SDK | 9.0 trở lên |
| SQL Server | 2019 trở lên (hoặc LocalDB) |
| `dotnet-ef` tool | 9.0 trở lên |
| Gemini API key | Bắt buộc để chatbot hoạt động |

Cài `dotnet-ef` nếu chưa có:
```bash
dotnet tool install --global dotnet-ef
```

---

## 1. Clone repository

```bash
git clone https://github.com/Tanbinhgas/may_tinh_sucvn.git
cd may_tinh_sucvn
git checkout v2.0.0
```

---

## 2. Cấu hình kết nối database

Mở `appsettings.json` và sửa chuỗi kết nối:

```json
"ConnectionStrings": {
  "Default": "Server=localhost;Database=tkl_computer;Trusted_Connection=True;TrustServerCertificate=True"
}
```

Thay `Server=localhost` bằng địa chỉ SQL Server của bạn nếu cần.

---

## 3. Cấu hình tài khoản admin (user-secrets)

Tài khoản admin **không được hard-code** — phải đặt qua user-secrets:

```bash
dotnet user-secrets set "Seed:Admin:Email"    "admin@tklcomputer.vn"
dotnet user-secrets set "Seed:Admin:Password" "<mật-khẩu-mạnh>"
```

Admin sẽ được tạo tự động khi chạy lần đầu nếu database chưa có tài khoản admin.

---

## 4. Cấu hình Gemini API key (cho chatbot)

Tính năng chatbot cần Gemini API key. Key **không được commit** vào repo — đặt qua user-secrets:

```bash
dotnet user-secrets set "Gemini:ApiKey" "<your-gemini-api-key>"
```

> Lấy API key miễn phí tại [Google AI Studio](https://aistudio.google.com/app/apikey).  
> Nếu không cấu hình key, ứng dụng vẫn chạy bình thường — chỉ endpoint chatbot trả lỗi 500.

---

## 5. Tạo / cập nhật database

Migration đã có sẵn trong repo — chỉ cần chạy:

```bash
dotnet ef database update
```

Lệnh này sẽ tạo đầy đủ các bảng:
`users`, `categories` (seed 13 danh mục), `products` (seed ~60 sản phẩm),
`orders`, `order_items`, `cart_items`.

---

## 6. Chạy ứng dụng

```bash
dotnet run
```

Ứng dụng mặc định chạy tại `https://localhost:5001` (hoặc port được gán tự động).

---

## 7. Kiểm thử

```bash
dotnet test tests/may_tinh_sucvn.Tests.csproj
```

15 test case (xUnit + SQLite in-memory) phủ các luồng lõi: xác thực, giỏ hàng
(tính giá từ server, chống sửa giá), đặt hàng (trừ kho), huỷ đơn (hoàn kho, chống IDOR).

---

## Tính năng đã triển khai

### Khách hàng
- Trang chủ, danh mục động `/category/{slug}`, chi tiết sản phẩm `/product/{slug}`.
- Giỏ hàng → Thanh toán → Xác nhận đơn hàng.
- Trang "Đơn hàng của tôi": xem lịch sử, huỷ đơn đang chờ xác nhận.
- Trang hồ sơ: sửa thông tin cá nhân, đổi mật khẩu.
- **AI Chatbot** (mới trong v2.0.0): hỏi tư vấn linh kiện qua Gemini 2.5 Flash.

### Quản trị (`/Admin`)
- Bảng điều khiển tổng quan.
- Quản lý sản phẩm: thêm, sửa, xoá, upload ảnh.
- Quản lý đơn hàng: xem danh sách, đổi trạng thái, huỷ đơn kèm hoàn kho.
- Báo cáo doanh thu.
- Quản lý người dùng: khoá/mở tài khoản, cấp/hạ quyền Admin.

---

## Bảo mật đã tích hợp

- **Chống sửa giá:** `CartItem` chỉ lưu `ProductId` + số lượng; tổng tiền tính từ `Product` phía server trong transaction khi đặt hàng.
- **BCrypt work factor 12:** mật khẩu không thể khôi phục ngược.
- **Antiforgery token** tự động trên toàn bộ form POST (Razor Pages).
- **Chống IDOR:** mọi truy vấn đơn hàng đều lọc theo `UserId` đang đăng nhập.
- **Transaction-safe:** đặt hàng, huỷ đơn và hoàn kho chạy trong một transaction duy nhất.
- **Không hard-code credential:** admin seed từ user-secrets, Gemini key từ user-secrets.

---

## Kiến trúc thư mục

```
may_tinh_sucvn/
├── Controllers/          # API Controllers (ChatbotController)
├── Data/                 # AppDbContext, SeedData, Migrations/
├── Models/               # Thực thể EF: User, Category, Product, Order, OrderItem, CartItem
├── Pages/                # Razor Pages — storefront + Admin/
│   └── Admin/            # Khu quản trị (khoá bằng policy "AdminOnly")
├── Services/             # IAuthService, ICartService, IOrderService + implementations
├── wwwroot/              # Static files (CSS, JS, ảnh)
├── tests/                # xUnit test project (SQLite in-memory)
├── database/             # SQL scripts tham khảo
├── .github/workflows/    # CI/CD (build + test tự động)
├── appsettings.json
├── CHANGELOG.md
└── README.md
```
