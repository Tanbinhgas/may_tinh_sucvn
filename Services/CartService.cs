using Microsoft.EntityFrameworkCore;
using may_tinh_sucvn.Data;
using may_tinh_sucvn.Models;

namespace may_tinh_sucvn.Services;

public class CartService : ICartService
{
    private readonly AppDbContext _db;

    public CartService(AppDbContext db) => _db = db;

    /// <summary>Đọc giỏ — giá/tên LẤY TỪ Product (không tin dữ liệu client từng gửi).</summary>
    public async Task<CartView> GetCartAsync(int userId)
    {
        var lines = await _db.CartItems
            .Where(c => c.UserId == userId && c.Product != null && c.Product.IsActive)
            .OrderByDescending(c => c.UpdatedAt)
            .Select(c => new CartLine(
                c.ProductId,
                c.Product!.Name,
                c.Product.Price,
                c.Quantity,
                c.Product.Stock,
                c.Product.ImageUrl))
            .ToListAsync();

        var total = lines.Sum(l => l.LineTotal);
        return new CartView(lines, total, lines.Count);
    }

    public async Task<AddToCartResult> AddAsync(int userId, int productId, int quantity)
    {
        quantity = Math.Max(1, quantity);

        // Xác minh sản phẩm tồn tại & đang bán.
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId && p.IsActive);
        if (product is null)
            return new AddToCartResult(false, "Sản phẩm không tồn tại.");

        var item = await _db.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
        var newQty = (item?.Quantity ?? 0) + quantity;

        if (newQty > product.Stock)
            return new AddToCartResult(false, $"Sản phẩm chỉ còn {product.Stock} trong kho.", product.Stock);

        if (item is null)
            _db.CartItems.Add(new CartItem { UserId = userId, ProductId = productId, Quantity = quantity });
        else
        {
            item.Quantity = newQty;
            item.UpdatedAt = DateTime.UtcNow;
        }
        await _db.SaveChangesAsync();

        var count = await _db.CartItems.CountAsync(c => c.UserId == userId);
        return new AddToCartResult(true, Stock: product.Stock, CartCount: count);
    }

    public async Task UpdateQuantityAsync(int userId, int productId, int quantity)
    {
        var item = await _db.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
        if (item is null) return;

        if (quantity <= 0)
        {
            _db.CartItems.Remove(item);
        }
        else
        {
            var stock = await _db.Products.Where(p => p.Id == productId).Select(p => p.Stock).FirstOrDefaultAsync();
            item.Quantity = Math.Min(quantity, stock);
            item.UpdatedAt = DateTime.UtcNow;
        }
        await _db.SaveChangesAsync();
    }

    public async Task RemoveAsync(int userId, int productId)
    {
        await _db.CartItems
            .Where(c => c.UserId == userId && c.ProductId == productId)
            .ExecuteDeleteAsync();
    }

    public async Task ClearAsync(int userId)
    {
        await _db.CartItems.Where(c => c.UserId == userId).ExecuteDeleteAsync();
    }

    /// <summary>
    /// Đặt hàng trong 1 transaction: kiểm tra tồn kho + TÍNH GIÁ TỪ SERVER,
    /// tạo Order/OrderItem (snapshot giá), trừ kho, xoá giỏ. Chống price-tampering.
    /// </summary>
    public async Task<CheckoutResult> CheckoutAsync(int userId, string phone, string address, string? note)
    {
        phone = phone.Trim();
        address = address.Trim();
        if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address))
            return new CheckoutResult(false, Error: "Vui lòng điền đầy đủ số điện thoại và địa chỉ.");

        // Nạp giỏ kèm sản phẩm thật.
        var items = await _db.CartItems
            .Where(c => c.UserId == userId)
            .Include(c => c.Product)
            .ToListAsync();

        if (items.Count == 0)
            return new CheckoutResult(false, Error: "Giỏ hàng trống, không thể đặt hàng.");

        // Kiểm tra tồn kho dựa trên dữ liệu sản phẩm hiện tại.
        var stockErrors = new List<string>();
        foreach (var it in items)
        {
            if (it.Product is null || !it.Product.IsActive)
                stockErrors.Add($"Sản phẩm (ID {it.ProductId}) không còn tồn tại.");
            else if (it.Quantity > it.Product.Stock)
                stockErrors.Add($"'{it.Product.Name}' chỉ còn {it.Product.Stock} trong kho (bạn đặt {it.Quantity}).");
        }
        if (stockErrors.Count > 0)
            return new CheckoutResult(false, StockErrors: stockErrors);

        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            // Tổng tiền tính HOÀN TOÀN từ giá server.
            var total = items.Sum(it => it.Product!.Price * it.Quantity);

            var order = new Order
            {
                UserId = userId,
                Status = OrderStatus.Pending,
                TotalPrice = total,
                ShippingAddress = address,
                Phone = phone,
                Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim(),
            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync(); // lấy order.Id

            foreach (var it in items)
            {
                var p = it.Product!;
                _db.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = p.Id,
                    ProductName = p.Name,   // snapshot
                    Price = p.Price,        // snapshot giá server
                    Quantity = it.Quantity,
                });
                p.Stock -= it.Quantity;     // trừ kho
            }

            _db.CartItems.RemoveRange(items);
            await _db.SaveChangesAsync();
            await tx.CommitAsync();

            return new CheckoutResult(true, OrderId: order.Id);
        }
        catch
        {
            await tx.RollbackAsync();
            return new CheckoutResult(false, Error: "Có lỗi xảy ra khi đặt hàng. Vui lòng thử lại.");
        }
    }
}
