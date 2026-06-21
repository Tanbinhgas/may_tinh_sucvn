using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using may_tinh_sucvn.Data;
using may_tinh_sucvn.Models;
using may_tinh_sucvn.Services;

namespace may_tinh_sucvn.Pages.Admin;

public class OrderDetailModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly IOrderService _orders;
    public OrderDetailModel(AppDbContext db, IOrderService orders) { _db = db; _orders = orders; }

    public Order? Order { get; set; }
    [TempData] public string? Message { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Order = await _db.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (Order is null) return NotFound();
        return Page();
    }

    public async Task<IActionResult> OnPostStatusAsync(int id, OrderStatus status)
    {
        // Huỷ đơn phải đi qua nút "Huỷ đơn" (để hoàn kho), không đặt trực tiếp ở đây.
        if (status == OrderStatus.Cancelled)
        {
            Message = "Để huỷ đơn, hãy dùng nút \"Huỷ đơn (hoàn kho)\".";
            return RedirectToPage(new { id });
        }

        var o = await _db.Orders.FindAsync(id);
        if (o is not null)
        {
            o.Status = status;
            o.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            Message = "Đã cập nhật trạng thái đơn hàng.";
        }
        return RedirectToPage(new { id });
    }

    public async Task<IActionResult> OnPostCancelAsync(int id)
    {
        var (_, message) = await _orders.CancelOrderAsync(id); // lối admin
        Message = message;
        return RedirectToPage(new { id });
    }
}
