using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using may_tinh_sucvn.Data;
using may_tinh_sucvn.Models;

namespace may_tinh_sucvn.Pages.Admin;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    public IndexModel(AppDbContext db) => _db = db;

    public int ProductCount { get; set; }
    public int OrderCount { get; set; }
    public int PendingCount { get; set; }
    public int UserCount { get; set; }
    public decimal Revenue { get; set; }
    public List<Order> RecentOrders { get; set; } = new();

    public async Task OnGetAsync()
    {
        ProductCount = await _db.Products.CountAsync(p => p.IsActive);
        OrderCount   = await _db.Orders.CountAsync();
        PendingCount = await _db.Orders.CountAsync(o => o.Status == OrderStatus.Pending);
        UserCount    = await _db.Users.CountAsync(u => u.Role == UserRole.Customer);
        Revenue      = await _db.Orders
            .Where(o => o.Status == OrderStatus.Done)
            .SumAsync(o => (decimal?)o.TotalPrice) ?? 0m;

        RecentOrders = await _db.Orders
            .Include(o => o.User)
            .OrderByDescending(o => o.CreatedAt)
            .Take(8)
            .ToListAsync();
    }
}
