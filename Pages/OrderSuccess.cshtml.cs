using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using may_tinh_sucvn.Data;
using may_tinh_sucvn.Models;

namespace may_tinh_sucvn.Pages;

public class OrderSuccessModel : PageModel
{
    private readonly AppDbContext _db;
    public OrderSuccessModel(AppDbContext db) => _db = db;

    public Order? Order { get; set; }

    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task<IActionResult> OnGetAsync(int id)
    {
        // Chỉ cho xem đơn của CHÍNH mình (chống IDOR).
        Order = await _db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == UserId);

        if (Order is null) return RedirectToPage("/MyOrders");
        return Page();
    }
}
