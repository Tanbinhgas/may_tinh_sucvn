using Microsoft.EntityFrameworkCore;
using may_tinh_sucvn.Data;
using may_tinh_sucvn.Models;

namespace may_tinh_sucvn.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _db;
    public OrderService(AppDbContext db) => _db = db;

    public async Task<(bool Ok, string Message)> CancelOrderAsync(int orderId, int? customerUserId = null)
    {
        var order = await _db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order is null) return (false, "Không tìm thấy đơn hàng.");

        if (customerUserId is int uid)
        {
            // Khách: phải là đơn của chính mình và đang chờ xác nhận.
            if (order.UserId != uid) return (false, "Không tìm thấy đơn hàng.");
            if (order.Status != OrderStatus.Pending)
                return (false, "Chỉ có thể huỷ đơn đang chờ xác nhận.");
        }
        else
        {
            // Admin: không huỷ đơn đã hoàn thành hoặc đã huỷ.
            if (order.Status is OrderStatus.Done or OrderStatus.Cancelled)
                return (false, "Không thể huỷ đơn đã hoàn thành hoặc đã huỷ.");
        }

        // Chống hoàn kho hai lần.
        if (order.Status == OrderStatus.Cancelled)
            return (false, "Đơn hàng đã được huỷ trước đó.");

        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            foreach (var it in order.Items)
            {
                var p = await _db.Products.FindAsync(it.ProductId);
                if (p is not null) p.Stock += it.Quantity; // hoàn lại tồn kho
            }
            order.Status = OrderStatus.Cancelled;
            order.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            return (true, "Đã huỷ đơn hàng và hoàn lại tồn kho.");
        }
        catch
        {
            await tx.RollbackAsync();
            return (false, "Có lỗi xảy ra khi huỷ đơn. Vui lòng thử lại.");
        }
    }
}
