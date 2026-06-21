using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using may_tinh_sucvn.Services;

namespace may_tinh_sucvn.Pages;

public class CheckoutModel : PageModel
{
    private readonly ICartService _cart;
    private readonly IAuthService _auth;
    public CheckoutModel(ICartService cart, IAuthService auth) { _cart = cart; _auth = auth; }

    public CartView Cart { get; set; } = new(Array.Empty<CartLine>(), 0m, 0);

    [BindProperty] public string Phone { get; set; } = string.Empty;
    [BindProperty] public string Address { get; set; } = string.Empty;
    [BindProperty] public string? Note { get; set; }

    public string? Error { get; set; }
    public List<string> StockErrors { get; set; } = new();

    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task<IActionResult> OnGetAsync()
    {
        Cart = await _cart.GetCartAsync(UserId);
        if (Cart.Count == 0) return RedirectToPage("/Cart");

        var u = await _auth.GetByIdAsync(UserId);
        Phone = u?.Phone ?? string.Empty;
        Address = u?.Address ?? string.Empty;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Cart = await _cart.GetCartAsync(UserId);
        if (Cart.Count == 0) return RedirectToPage("/Cart");

        var result = await _cart.CheckoutAsync(UserId, Phone ?? string.Empty, Address ?? string.Empty, Note);
        if (result.Success)
            return RedirectToPage("/OrderSuccess", new { id = result.OrderId });

        if (result.StockErrors is { Count: > 0 })
            StockErrors = result.StockErrors.ToList();
        Error = result.Error;
        return Page();
    }
}
