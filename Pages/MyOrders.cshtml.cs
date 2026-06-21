using may_tinh_sucvn.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using may_tinh_sucvn.Data;
using may_tinh_sucvn.Models;

namespace may_tinh_sucvn.Pages;

public class MyOrdersModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly IOrderService _orders;
    public MyOrdersModel(AppDbContext db, IOrderService orders) { _db = db; _orders = orders; }

    public List<Order> Orders { get; set; } = new();
    [TempData] public string? Message { get; set; }

    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task OnGetAsync()
    {
        Orders = await _db.Orders
            .Include(o => o.Items)
            .Where(o => o.UserId == UserId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostCancelAsync(int id)
    {
        var (_, message) = await _orders.CancelOrderAsync(id, UserId);
        Message = message;
        return RedirectToPage();
    }
}
