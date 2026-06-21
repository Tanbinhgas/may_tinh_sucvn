using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using may_tinh_sucvn.Data;
using may_tinh_sucvn.Models;

namespace may_tinh_sucvn.Pages;

public class CategoryModel : PageModel
{
    private readonly AppDbContext _db;
    public CategoryModel(AppDbContext db) => _db = db;

    public Category? Category { get; set; }
    public List<Product> Products { get; set; } = new();
    public List<Category> AllCategories { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(string slug)
    {
        AllCategories = await _db.Categories.OrderBy(c => c.Id).ToListAsync();

        Category = await _db.Categories.FirstOrDefaultAsync(c => c.Slug == slug);
        if (Category is null) return NotFound();

        Products = await _db.Products
            .Where(p => p.CategoryId == Category.Id && p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();

        return Page();
    }
}
