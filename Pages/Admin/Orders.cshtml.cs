using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using may_tinh_sucvn.Data;
using may_tinh_sucvn.Models;

namespace may_tinh_sucvn.Pages.Admin;

public class OrdersModel : PageModel
{
    private readonly AppDbContext _db;
    public OrdersModel(AppDbContext db) => _db = db;

    public List<Order> Orders { get; set; } = new();

    [BindProperty(SupportsGet = true)] public string? Status { get; set; }
    public OrderStatus? Active { get; set; }

    public async Task OnGetAsync()
    {
        var q = _db.Orders.Include(o => o.User).Include(o => o.Items).AsQueryable();

        if (Enum.TryParse<OrderStatus>(Status, ignoreCase: true, out var st))
        {
            Active = st;
            q = q.Where(o => o.Status == st);
        }

        Orders = await q.OrderByDescending(o => o.CreatedAt).ToListAsync();
    }
}
