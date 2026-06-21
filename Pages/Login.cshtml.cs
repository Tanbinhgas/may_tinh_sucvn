using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using may_tinh_sucvn.Models;
using may_tinh_sucvn.Services;

namespace may_tinh_sucvn.Pages;

public class LoginModel : PageModel
{
    private readonly IAuthService _auth;
    public LoginModel(IAuthService auth) => _auth = auth;

    [BindProperty] public string UsernameOrEmail { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
            return Redirect(User.IsInRole(nameof(UserRole.Admin)) ? "/Admin" : "/");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(UsernameOrEmail) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Vui lòng điền đầy đủ thông tin.";
            return Page();
        }

        var result = await _auth.LoginAsync(UsernameOrEmail, Password);
        if (!result.Success || result.User is null)
        {
            ErrorMessage = result.Error;
            return Page();
        }

        await SignInAsync(result.User);
        return Redirect(result.User.Role == UserRole.Admin ? "/Admin" : "/");
    }

    private async Task SignInAsync(User user)
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
