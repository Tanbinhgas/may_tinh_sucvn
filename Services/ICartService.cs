namespace may_tinh_sucvn.Services;

public interface ICartService
{
    Task<CartView> GetCartAsync(int userId);
    Task<AddToCartResult> AddAsync(int userId, int productId, int quantity);
    Task UpdateQuantityAsync(int userId, int productId, int quantity);
    Task RemoveAsync(int userId, int productId);
    Task ClearAsync(int userId);
    Task<CheckoutResult> CheckoutAsync(int userId, string phone, string address, string? note);
}
