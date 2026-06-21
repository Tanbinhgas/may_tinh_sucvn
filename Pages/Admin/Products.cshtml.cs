using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using may_tinh_sucvn.Data;
using may_tinh_sucvn.Models;

namespace may_tinh_sucvn.Pages.Admin;

public class ProductsModel : PageModel
{
    private readonly AppDbContext _db;
    public ProductsModel(AppDbContext db) => _db = db;

    public List<Product> Products { get; set; } = new();
    [TempData] public string? Message { get; set; }

    public async Task OnGetAsync()
    {
        Products = await _db.Products
            .Include(p => p.Category)
            .OrderBy(p => p.CategoryId).ThenBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostToggleAsync(int id)
    {
        var p = await _db.Products.FindAsync(id);
        if (p is not null)
        {
            p.IsActive = !p.IsActive;
            p.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            Message = p.IsActive ? "Đã hiển thị sản phẩm." : "Đã ẩn sản phẩm.";
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var p = await _db.Products.FindAsync(id);
        if (p is null) return RedirectToPage();

        // Đã nằm trong đơn hàng → không xoá cứng để giữ lịch sử, chỉ ẩn.
        bool inOrders = await _db.OrderItems.AnyAsync(oi => oi.ProductId == id);
        if (inOrders)
        {
            p.IsActive = false;
            p.UpdatedAt = DateTime.UtcNow;
            Message = "Sản phẩm đã thuộc đơn hàng nên được ẩn thay vì xoá (giữ lịch sử đơn).";
        }
        else
        {
            // Gỡ khỏi giỏ hàng (nếu có) rồi xoá hẳn.
            _db.CartItems.RemoveRange(_db.CartItems.Where(c => c.ProductId == id));
            _db.Products.Remove(p);
            Message = "Đã xoá sản phẩm.";
        }
        await _db.SaveChangesAsync();
        return RedirectToPage();
    }
}
