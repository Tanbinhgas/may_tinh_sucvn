namespace may_tinh_sucvn.Models;

/// <summary>
/// Dòng giỏ hàng. CHỈ lưu ProductId + Quantity của người dùng đã đăng nhập.
/// Giá và tên LUÔN lấy từ bảng Product lúc đọc / checkout — đây là bản vá lỗ hổng
/// price-tampering của backend cũ (giỏ cũ lưu giá do client gửi lên).
/// </summary>
public class CartItem
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 1;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public Product? Product { get; set; }
}
