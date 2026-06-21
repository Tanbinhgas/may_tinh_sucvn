using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using may_tinh_sucvn.Models;
using may_tinh_sucvn.Services;

namespace may_tinh_sucvn.Pages;

public class ProfileModel : PageModel
{
    private readonly IAuthService _auth;
    public ProfileModel(IAuthService auth) => _auth = auth;

    /// <summary>Tài khoản dùng để hiển thị các trường chỉ đọc (username/email/vai trò).</summary>
    public User? Account { get; set; }

    [BindProperty] public string? FullName { get; set; }
    [BindProperty] public string? Phone { get; set; }
    [BindProperty] public string? Address { get; set; }

    [BindProperty] public string CurrentPassword { get; set; } = string.Empty;
    [BindProperty] public string NewPassword { get; set; } = string.Empty;
    [BindProperty] public string ConfirmPassword { get; set; } = string.Empty;

    public string? InfoError { get; set; }
    public string? InfoOk { get; set; }
    public string? PwError { get; set; }
    public string? PwOk { get; set; }

    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task<IActionResult> OnGetAsync()
    {
        Account = await _auth.GetByIdAsync(UserId);
        if (Account is null) return RedirectToPage("/Login");

        FullName = Account.FullName;
        Phone = Account.Phone;
        Address = Account.Address;
        return Page();
    }

    public async Task<IActionResult> OnPostInfoAsync()
    {
        var r = await _auth.UpdateProfileAsync(UserId, FullName, Phone, Address);
        Account = await _auth.GetByIdAsync(UserId);
        if (Account is null) return RedirectToPage("/Login");

        if (!r.Success) { InfoError = r.Error; return Page(); }

        // Làm mới cookie để tên hiển thị (claim "FullName") cập nhật ở các lần điều hướng sau.
        await RefreshSignInAsync(r.User!);
        InfoOk = "Đã cập nhật thông tin tài khoản.";
        return Page();
    }

    public async Task<IActionResult> OnPostPasswordAsync()
    {
        Account = await _auth.GetByIdAsync(UserId);
        if (Account is null) return RedirectToPage("/Login");

        if (NewPassword != ConfirmPassword)
        {
            PwError = "Xác nhận mật khẩu mới không khớp.";
            return Page();
        }

        var r = await _auth.ChangePasswordAsync(UserId, CurrentPassword, NewPassword);
        if (!r.Success) { PwError = r.Error; return Page(); }

        PwOk = "Đổi mật khẩu thành công.";
        return Page();
    }

    private async Task RefreshSignInAsync(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role.ToString()),
            new("FullName", user.FullName ?? user.Username),
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties { IsPersistent = true });
    }
}
