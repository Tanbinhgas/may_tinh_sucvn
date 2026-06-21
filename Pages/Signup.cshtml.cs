using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using may_tinh_sucvn.Models;
using may_tinh_sucvn.Services;

namespace may_tinh_sucvn.Pages;

public partial class SignupModel : PageModel
{
    private readonly IAuthService _auth;
    public SignupModel(IAuthService auth) => _auth = auth;

    [BindProperty] public string Email { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;
    [BindProperty] public string? FullName { get; set; }
    [BindProperty] public string? Phone { get; set; }
    public string? ErrorMessage { get; set; }

    [GeneratedRegex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$")]
    private static partial Regex EmailRegex();

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true) return Redirect("/");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Email = (Email ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Vui lòng điền đầy đủ thông tin.";
            return Page();
        }
        if (!EmailRegex().IsMatch(Email))
        {
            ErrorMessage = "Email không hợp lệ.";
            return Page();
        }
        if (Password.Length < 6)
        {
            ErrorMessage = "Mật khẩu tối thiểu 6 ký tự.";
            return Page();
        }

        var username = Email.Split('@')[0];
        var fullName = string.IsNullOrWhiteSpace(FullName) ? username : FullName!.Trim();

        var result = await _auth.RegisterAsync(username, Email, Password, fullName, Phone);
        if (!result.Success)
        {
            ErrorMessage = result.Error;
            return Page();
        }

        return RedirectToPage("/Login");
    }
}
