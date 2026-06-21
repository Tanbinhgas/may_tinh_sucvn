using may_tinh_sucvn.Data;
using may_tinh_sucvn.Models;
using may_tinh_sucvn.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace may_tinh_sucvn.Tests;

public class CartServiceTests
{
    private static async Task<(int userId, int productId)> SeedUserAndProduct(AppDbContext db, decimal price, int stock)
    {
        var user = new User { Username = "buyer", Email = "buyer@example.com", PasswordHash = "x", FullName = "Buyer" };
        db.Users.Add(user);
        var product = new Product { CategoryId = 1, Name = "Test CPU", Slug = "test-cpu", Price = price, Stock = stock, IsActive = true };
        db.Products.Add(product);
        await db.SaveChangesAsync();
        return (user.Id, product.Id);
    }

    [Fact]
    public async Task Checkout_ComputesTotalFromServerPrice()
    {
        using var t = new SqliteTestDb();
        var (userId, productId) = await SeedUserAndProduct(t.Db, price: 1000m, stock: 10);

        // CartItem chỉ có ProductId + Quantity (không có trường giá) — giá luôn lấy từ Product.
        t.Db.CartItems.Add(new CartItem { UserId = userId, ProductId = productId, Quantity = 3 });
        await t.Db.SaveChangesAsync();

        var result = await new CartService(t.Db).CheckoutAsync(userId, "0900000000", "123 Duong ABC", null);

        Assert.True(result.Success);
        var order = await t.Db.Orders.Include(o => o.Items).FirstAsync(o => o.Id == result.OrderId);
        Assert.Equal(3000m, order.TotalPrice);            // 1000 * 3, tính từ server
        Assert.Equal(1000m, order.Items.Single().Price);  // snapshot giá server
        Assert.Equal(3, order.Items.Single().Quantity);
    }

    [Fact]
    public async Task Checkout_DecrementsStock_AndClearsCart()
    {
        using var t = new SqliteTestDb();
        var (userId, productId) = await SeedUserAndProduct(t.Db, 500m, 10);
        t.Db.CartItems.Add(new CartItem { UserId = userId, ProductId = productId, Quantity = 4 });
        await t.Db.SaveChangesAsync();

        var result = await new CartService(t.Db).CheckoutAsync(userId, "0900000000", "Addr", null);

        Assert.True(result.Success);
        Assert.Equal(6, (await t.Db.Products.FindAsync(productId))!.Stock); // 10 - 4
        Assert.False(await t.Db.CartItems.AnyAsync(c => c.UserId == userId)); // giỏ đã được dọn
    }

    [Fact]
    public async Task Checkout_InsufficientStock_FailsWithoutCreatingOrder()
    {
        using var t = new SqliteTestDb();
        var (userId, productId) = await SeedUserAndProduct(t.Db, 500m, 2);
        t.Db.CartItems.Add(new CartItem { UserId = userId, ProductId = productId, Quantity = 5 });
        await t.Db.SaveChangesAsync();

        var result = await new CartService(t.Db).CheckoutAsync(userId, "0900000000", "Addr", null);

        Assert.False(result.Success);
        Assert.NotNull(result.StockErrors);
        Assert.NotEmpty(result.StockErrors!);
        Assert.Equal(0, await t.Db.Orders.CountAsync());
        Assert.Equal(2, (await t.Db.Products.FindAsync(productId))!.Stock); // tồn kho không đổi
    }

    [Fact]
    public async Task Checkout_EmptyCart_Fails()
    {
        using var t = new SqliteTestDb();
        var (userId, _) = await SeedUserAndProduct(t.Db, 500m, 10);

        var result = await new CartService(t.Db).CheckoutAsync(userId, "0900000000", "Addr", null);

        Assert.False(result.Success);
        Assert.Equal(0, await t.Db.Orders.CountAsync());
    }
}
