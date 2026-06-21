using may_tinh_sucvn.Services;
using Xunit;

namespace may_tinh_sucvn.Tests;

public class AuthServiceTests
{
    [Fact]
    public async Task Register_NewUser_HashesPasswordAndPersists()
    {
        using var t = new SqliteTestDb();
        var auth = new AuthService(t.Db);

        var r = await auth.RegisterAsync("nam", "nam@example.com", "secret123", "Nam Nguyen", "0900000000");

        Assert.True(r.Success);
        Assert.NotNull(r.User);
        Assert.NotEqual("secret123", r.User!.PasswordHash); // mật khẩu đã được hash
    }

    [Fact]
    public async Task Register_DuplicateEmail_Fails()
    {
        using var t = new SqliteTestDb();
        var auth = new AuthService(t.Db);
        await auth.RegisterAsync("a", "dup@example.com", "secret123", "A", null);

        var r = await auth.RegisterAsync("b", "dup@example.com", "secret123", "B", null);

        Assert.False(r.Success);
    }

    [Fact]
    public async Task Login_WrongPassword_FailsClosed()
    {
        using var t = new SqliteTestDb();
        var auth = new AuthService(t.Db);
        await auth.RegisterAsync("nam", "nam@example.com", "secret123", "Nam", null);

        var r = await auth.LoginAsync("nam", "wrong-password");

        Assert.False(r.Success);
    }

    [Fact]
    public async Task Login_CorrectPassword_Succeeds()
    {
        using var t = new SqliteTestDb();
        var auth = new AuthService(t.Db);
        await auth.RegisterAsync("nam", "nam@example.com", "secret123", "Nam", null);

        var r = await auth.LoginAsync("nam@example.com", "secret123");

        Assert.True(r.Success);
        Assert.NotNull(r.User);
    }

    [Fact]
    public async Task ChangePassword_WrongCurrent_Fails()
    {
        using var t = new SqliteTestDb();
        var auth = new AuthService(t.Db);
        var reg = await auth.RegisterAsync("nam", "nam@example.com", "secret123", "Nam", null);

        var r = await auth.ChangePasswordAsync(reg.User!.Id, "not-the-current", "newsecret123");

        Assert.False(r.Success);
    }

    [Fact]
    public async Task ChangePassword_Valid_AllowsLoginWithNewPasswordOnly()
    {
        using var t = new SqliteTestDb();
        var auth = new AuthService(t.Db);
        var reg = await auth.RegisterAsync("nam", "nam@example.com", "secret123", "Nam", null);

        var change = await auth.ChangePasswordAsync(reg.User!.Id, "secret123", "newsecret123");
        Assert.True(change.Success);

        Assert.False((await auth.LoginAsync("nam", "secret123")).Success);
        Assert.True((await auth.LoginAsync("nam", "newsecret123")).Success);
    }
}
