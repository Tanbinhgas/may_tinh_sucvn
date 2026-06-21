using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using may_tinh_sucvn.Data;
using may_tinh_sucvn.Models;

namespace may_tinh_sucvn.Pages;

public class ProductModel : PageModel
{
    private readonly AppDbContext _db;
    public ProductModel(AppDbContext db) => _db = db;

    public Product? Product { get; set; }
    public List<Product> Related { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(string? slug)
    {
        // Truy cập trống /product (link footer cũ) -> về trang chủ thay vì lỗi.
        if (string.IsNullOrWhiteSpace(slug)) return RedirectToPage("/Index");

        Product = await _db.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Slug == slug && p.IsActive);

        if (Product is null) return NotFound();

        var catId = Product.CategoryId;
        var selfId = Product.Id;
        Related = await _db.Products
            .Where(p => p.CategoryId == catId && p.Id != selfId && p.IsActive)
            .OrderBy(p => p.Name)
            .Take(4)
            .ToListAsync();

        return Page();
    }
}
