using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using may_tinh_sucvn.Data;
using may_tinh_sucvn.Models;

namespace may_tinh_sucvn.Pages.Admin;

public class UsersModel : PageModel
{
    private readonly AppDbContext _db;
    public UsersModel(AppDbContext db) => _db = db;

    public List<Row> Users { get; set; } = new();
    public int CurrentUserId { get; set; }
    [TempData] public string? Message { get; set; }

    public record Row(User User, int OrderCount);

    private int Me => int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var v) ? v : 0;

    public async Task OnGetAsync()
    {
        CurrentUserId = Me;
        var rows = await _db.Users
            .Select(u => new Row(u, u.Orders.Count))
            .ToListAsync();
        Users = rows
            .OrderByDescending(r => r.User.Role == UserRole.Admin)
            .ThenBy(r => r.User.Id)
            .ToList();
    }

    public async Task<IActionResult> OnPostToggleActiveAsync(int id)
    {
        if (id == Me) { Message = "Không thể tự khoá tài khoản của chính mình."; return RedirectToPage(); }
        var u = await _db.Users.FindAsync(id);
        if (u is not null)
        {
            u.IsActive = !u.IsActive;
            u.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            Message = u.IsActive ? "Đã mở khoá tài khoản." : "Đã khoá tài khoản.";
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostToggleRoleAsync(int id)
    {
        if (id == Me) { Message = "Không thể đổi vai trò của chính mình."; return RedirectToPage(); }
        var u = await _db.Users.FindAsync(id);
        if (u is not null)
        {
            u.Role = u.Role == UserRole.Admin ? UserRole.Customer : UserRole.Admin;
            u.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            Message = u.Role == UserRole.Admin ? "Đã cấp quyền quản trị." : "Đã chuyển về khách hàng.";
        }
        return RedirectToPage();
    }
}
