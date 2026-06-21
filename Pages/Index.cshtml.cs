using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using may_tinh_sucvn.Data;
using may_tinh_sucvn.Models;

namespace may_tinh_sucvn.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    public IndexModel(AppDbContext db) => _db = db;

    public IReadOnlyList<Product> Products { get; private set; } = new List<Product>();
    public IReadOnlyList<Category> Categories { get; private set; } = new List<Category>();

    public async Task OnGetAsync()
    {
        Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
        Products = await _db.Products
            .Where(p => p.IsActive)
            .Include(p => p.Category)
            .OrderByDescending(p => p.CreatedAt)
            .Take(24)
            .ToListAsync();
    }
}
