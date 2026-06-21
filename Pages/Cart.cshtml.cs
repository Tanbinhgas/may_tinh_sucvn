using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using may_tinh_sucvn.Services;

namespace may_tinh_sucvn.Pages;

public class CartModel : PageModel
{
    private readonly ICartService _cart;
    public CartModel(ICartService cart) => _cart = cart;

    public CartView Cart { get; private set; } = new(Array.Empty<CartLine>(), 0, 0);
    public string? Message { get; set; }

    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task OnGetAsync() => Cart = await _cart.GetCartAsync(UserId);

    public async Task<IActionResult> OnPostAddAsync(int productId, int quantity = 1)
    {
        var r = await _cart.AddAsync(UserId, productId, quantity);
        if (!r.Success) TempData["CartMessage"] = r.Error;
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateAsync(int productId, int quantity)
    {
        await _cart.UpdateQuantityAsync(UserId, productId, quantity);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveAsync(int productId)
    {
        await _cart.RemoveAsync(UserId, productId);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostClearAsync()
    {
        await _cart.ClearAsync(UserId);
        return RedirectToPage();
    }
}
