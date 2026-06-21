namespace may_tinh_sucvn.Services;

using may_tinh_sucvn.Models;

/// <summary>Kết quả đăng nhập / đăng ký.</summary>
public record AuthResult(bool Success, string? Error = null, User? User = null);

/// <summary>Một dòng giỏ hàng đã "làm giàu" với dữ liệu sản phẩm thật.</summary>
public record CartLine(int ProductId, string Name, decimal Price, int Quantity, int Stock, string? ImageUrl)
{
    public decimal LineTotal => Price * Quantity;
}

/// <summary>Toàn bộ giỏ hàng để hiển thị.</summary>
public record CartView(IReadOnlyList<CartLine> Lines, decimal Total, int Count);

/// <summary>Kết quả thêm vào giỏ.</summary>
public record AddToCartResult(bool Success, string? Error = null, int Stock = 0, int CartCount = 0);

/// <summary>Kết quả đặt hàng.</summary>
public record CheckoutResult(bool Success, int? OrderId = null, IReadOnlyList<string>? StockErrors = null, string? Error = null);
