using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using may_tinh_sucvn.Data;
using may_tinh_sucvn.Models;

namespace may_tinh_sucvn.Pages.Admin;

public class ProductEditModel : PageModel
{
    private readonly AppDbContext _db;
    public ProductEditModel(AppDbContext db) => _db = db;

    [BindProperty] public InputModel Input { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public bool IsEdit => Input.Id != 0;
    public string? Error { get; set; }

    public class InputModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Brand { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        Categories = await _db.Categories.OrderBy(c => c.Id).ToListAsync();

        if (id is int pid)
        {
            var p = await _db.Products.FindAsync(pid);
            if (p is null) return NotFound();
            Input = new InputModel
            {
                Id = p.Id, Name = p.Name, Slug = p.Slug, CategoryId = p.CategoryId,
                Price = p.Price, Stock = p.Stock, Brand = p.Brand, ImageUrl = p.ImageUrl,
                Description = p.Description, IsActive = p.IsActive
            };
        }
        else if (Categories.Count > 0)
        {
            Input.CategoryId = Categories[0].Id;
            Input.IsActive = true;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Categories = await _db.Categories.OrderBy(c => c.Id).ToListAsync();

        if (string.IsNullOrWhiteSpace(Input.Name)) { Error = "Tên sản phẩm không được để trống."; return Page(); }
        if (Input.Price < 0 || Input.Stock < 0) { Error = "Giá và tồn kho phải ≥ 0."; return Page(); }
        if (!await _db.Categories.AnyAsync(c => c.Id == Input.CategoryId)) { Error = "Danh mục không hợp lệ."; return Page(); }

        var slug = Slugify(string.IsNullOrWhiteSpace(Input.Slug) ? Input.Name : Input.Slug!);
        if (await _db.Products.AnyAsync(p => p.Slug == slug && p.Id != Input.Id))
        {
            Error = $"Slug \"{slug}\" đã tồn tại. Hãy đổi tên hoặc nhập slug khác.";
            return Page();
        }

        Product p;
        if (Input.Id != 0)
        {
            p = await _db.Products.FindAsync(Input.Id) ?? throw new InvalidOperationException("Không tìm thấy sản phẩm.");
            p.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            p = new Product { CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            _db.Products.Add(p);
        }

        p.Name = Input.Name.Trim();
        p.Slug = slug;
        p.CategoryId = Input.CategoryId;
        p.Price = Input.Price;
        p.Stock = Input.Stock;
        p.Brand = string.IsNullOrWhiteSpace(Input.Brand) ? null : Input.Brand!.Trim();
        p.ImageUrl = string.IsNullOrWhiteSpace(Input.ImageUrl) ? null : Input.ImageUrl!.Trim();
        p.Description = string.IsNullOrWhiteSpace(Input.Description) ? null : Input.Description!.Trim();
        p.IsActive = Input.IsActive;

        await _db.SaveChangesAsync();
        return RedirectToPage("/Admin/Products");
    }

    /// <summary>Tạo slug không dấu từ chuỗi tiếng Việt.</summary>
    private static string Slugify(string input)
    {
        var s = input.Trim().ToLowerInvariant().Replace('đ', 'd');
        var decomposed = s.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var ch in decomposed)
            if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                sb.Append(ch);
        s = sb.ToString().Normalize(NormalizationForm.FormC);
        s = Regex.Replace(s, "[^a-z0-9]+", "-").Trim('-');
        return string.IsNullOrEmpty(s) ? Guid.NewGuid().ToString("n")[..8] : s;
    }
}
