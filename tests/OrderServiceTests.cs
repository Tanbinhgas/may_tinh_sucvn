using may_tinh_sucvn.Data;
using may_tinh_sucvn.Models;
using may_tinh_sucvn.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace may_tinh_sucvn.Tests;

public class OrderServiceTests
{
    private static async Task<(int userId, int productId, int orderId)> SeedOrder(
        AppDbContext db, OrderStatus status, int currentStock, int qty)
    {
        var user = new User { Username = "u", Email = "u@example.com", PasswordHash = "x" };
        db.Users.Add(user);
        var product = new Product { CategoryId = 1, Name = "P", Slug = "p", Price = 100m, Stock = currentStock, IsActive = true };
        db.Products.Add(product);
        await db.SaveChangesAsync();

        var order = new Order
        {
            UserId = user.Id,
            Status = status,
            TotalPrice = 100m * qty,
            ShippingAddress = "Addr",
            Phone = "0900000000",
            Items = new List<OrderItem>
            {
                new() { ProductId = product.Id, ProductName = "P", Price = 100m, Quantity = qty }
            }
        };
        db.Orders.Add(order);
        await db.SaveChangesAsync();
        return (user.Id, product.Id, order.Id);
    }

    [Fact]
    public async Task Customer_CancelOwnPending_RestocksAndCancels()
    {
        using var t = new SqliteTestDb();
        var (userId, productId, orderId) = await SeedOrder(t.Db, OrderStatus.Pending, currentStock: 6, qty: 4);

        var (ok, _) = await new OrderService(t.Db).CancelOrderAsync(orderId, userId);

        Assert.True(ok);
        Assert.Equal(OrderStatus.Cancelled, (await t.Db.Orders.FindAsync(orderId))!.Status);
        Assert.Equal(10, (await t.Db.Products.FindAsync(productId))!.Stock); // 6 + 4 hoàn lại
    }

    [Fact]
    public async Task Customer_CannotCancelOthersOrder()
    {
        using var t = new SqliteTestDb();
        var (_, productId, orderId) = await SeedOrder(t.Db, OrderStatus.Pending, 6, 4);

        var (ok, _) = await new OrderService(t.Db).CancelOrderAsync(orderId, customerUserId: 99999);

        Assert.False(ok);
        Assert.Equal(OrderStatus.Pending, (await t.Db.Orders.FindAsync(orderId))!.Status);
        Assert.Equal(6, (await t.Db.Products.FindAsync(productId))!.Stock);
    }

    [Fact]
    public async Task Customer_CannotCancelNonPending()
    {
        using var t = new SqliteTestDb();
        var (userId, _, orderId) = await SeedOrder(t.Db, OrderStatus.Shipping, 6, 4);

        var (ok, _) = await new OrderService(t.Db).CancelOrderAsync(orderId, userId);

        Assert.False(ok);
    }

    [Fact]
    public async Task Admin_CanCancelConfirmed_Restocks()
    {
        using var t = new SqliteTestDb();
        var (_, productId, orderId) = await SeedOrder(t.Db, OrderStatus.Confirmed, 6, 4);

        var (ok, _) = await new OrderService(t.Db).CancelOrderAsync(orderId); // lối admin

        Assert.True(ok);
        Assert.Equal(10, (await t.Db.Products.FindAsync(productId))!.Stock);
    }

    [Fact]
    public async Task Cancel_AlreadyCancelled_NoDoubleRestock()
    {
        using var t = new SqliteTestDb();
        var (_, productId, orderId) = await SeedOrder(t.Db, OrderStatus.Cancelled, 10, 4);

        var (ok, _) = await new OrderService(t.Db).CancelOrderAsync(orderId); // admin

        Assert.False(ok); // đã huỷ trước đó
        Assert.Equal(10, (await t.Db.Products.FindAsync(productId))!.Stock); // không cộng kho lần hai
    }
}
