using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using may_tinh_sucvn.Data;
using may_tinh_sucvn.Models;

namespace may_tinh_sucvn.Pages.Admin;

public class RevenueModel : PageModel
{
    private readonly AppDbContext _db;
    public RevenueModel(AppDbContext db) => _db = db;

    public decimal TotalRevenue { get; set; }
    public int DoneOrders { get; set; }
    public decimal OpenValue { get; set; }   // giá trị đơn đang xử lý (chưa hoàn thành, chưa huỷ)
    public List<StatusStat> ByStatus { get; set; } = new();
    public List<MonthStat> ByMonth { get; set; } = new();
    public List<TopProduct> Top { get; set; } = new();

    public record StatusStat(OrderStatus Status, int Count, decimal Sum);
    public record MonthStat(int Year, int Month, decimal Sum, int Count);
    public record TopProduct(string Name, int Quantity, decimal Revenue);

    public async Task OnGetAsync()
    {
        TotalRevenue = await _db.Orders.Where(o => o.Status == OrderStatus.Done)
            .SumAsync(o => (decimal?)o.TotalPrice) ?? 0m;

        DoneOrders = await _db.Orders.CountAsync(o => o.Status == OrderStatus.Done);

        OpenValue = await _db.Orders
            .Where(o => o.Status == OrderStatus.Pending
                     || o.Status == OrderStatus.Confirmed
                     || o.Status == OrderStatus.Shipping)
            .SumAsync(o => (decimal?)o.TotalPrice) ?? 0m;

        ByStatus = await _db.Orders
            .GroupBy(o => o.Status)
            .Select(g => new StatusStat(g.Key, g.Count(), g.Sum(x => x.TotalPrice)))
            .ToListAsync();

        var since = DateTime.UtcNow.AddMonths(-5);
        since = new DateTime(since.Year, since.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var months = await _db.Orders
            .Where(o => o.Status == OrderStatus.Done && o.CreatedAt >= since)
            .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
            .Select(g => new MonthStat(g.Key.Year, g.Key.Month, g.Sum(x => x.TotalPrice), g.Count()))
            .ToListAsync();
        ByMonth = months.OrderBy(m => m.Year).ThenBy(m => m.Month).ToList();

        var itemsForTop = await _db.OrderItems
            .Where(oi => oi.Order!.Status != OrderStatus.Cancelled)
            .Select(oi => new { oi.ProductName, oi.Quantity, oi.Price })
            .ToListAsync();

        Top = itemsForTop
            .GroupBy(oi => oi.ProductName)
            .Select(g => new TopProduct(g.Key, g.Sum(x => x.Quantity), g.Sum(x => x.Price * x.Quantity)))
            .OrderByDescending(t => t.Quantity)
            .Take(5)
            .ToList();
    }
}
