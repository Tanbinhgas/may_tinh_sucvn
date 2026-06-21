using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using may_tinh_sucvn.Data;

namespace may_tinh_sucvn.Tests;

/// <summary>
/// Cấp một <see cref="AppDbContext"/> chạy trên SQLite in-memory — provider quan hệ thật,
/// hỗ trợ transaction / ExecuteDelete như SQL Server (khác với provider InMemory).
/// Connection được giữ mở suốt vòng đời để dữ liệu không bị xoá giữa các thao tác.
/// </summary>
public sealed class SqliteTestDb : IDisposable
{
    private readonly SqliteConnection _connection;
    public AppDbContext Db { get; }

    public SqliteTestDb()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        Db = new AppDbContext(options);
        Db.Database.EnsureCreated(); // tạo schema + seed 13 danh mục (HasData)
    }

    public void Dispose()
    {
        Db.Dispose();
        _connection.Dispose();
    }
}
