using Microsoft.EntityFrameworkCore;
using may_tinh_sucvn.Models;

namespace may_tinh_sucvn.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    // Mốc thời gian cố định cho seed (HasData yêu cầu giá trị tĩnh, không dùng DateTime.UtcNow).
    private static readonly DateTime SeedDate = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    protected override void OnModelCreating(ModelBuilder b)
    {
        // ── USER ──────────────────────────────────────────────
        b.Entity<User>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
            e.HasIndex(u => u.Username).IsUnique();
            e.Property(u => u.Role).HasConversion<string>().HasMaxLength(20);
        });

        // ── CATEGORY ──────────────────────────────────────────
        b.Entity<Category>(e =>
        {
            e.HasIndex(c => c.Slug).IsUnique();
        });

        // ── PRODUCT ───────────────────────────────────────────
        b.Entity<Product>(e =>
        {
            e.HasIndex(p => p.Slug).IsUnique();
            e.HasIndex(p => p.CategoryId);
            e.HasIndex(p => p.IsActive);
            e.HasOne(p => p.Category)
             .WithMany(c => c.Products)
             .HasForeignKey(p => p.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── ORDER ─────────────────────────────────────────────
        b.Entity<Order>(e =>
        {
            e.Property(o => o.Status).HasConversion<string>().HasMaxLength(20);
            e.HasIndex(o => o.UserId);
            e.HasIndex(o => o.Status);
            e.HasOne(o => o.User)
             .WithMany(u => u.Orders)
             .HasForeignKey(o => o.UserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── ORDER ITEM ────────────────────────────────────────
        b.Entity<OrderItem>(e =>
        {
            e.HasOne(i => i.Order)
             .WithMany(o => o.Items)
             .HasForeignKey(i => i.OrderId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(i => i.Product)
             .WithMany(p => p.OrderItems)
             .HasForeignKey(i => i.ProductId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── CART ITEM ─────────────────────────────────────────
        b.Entity<CartItem>(e =>
        {
            // Mỗi (user, product) chỉ có đúng 1 dòng giỏ.
            e.HasIndex(c => new { c.UserId, c.ProductId }).IsUnique();
            e.HasOne(c => c.User)
             .WithMany(u => u.CartItems)
             .HasForeignKey(c => c.UserId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(c => c.Product)
             .WithMany()
             .HasForeignKey(c => c.ProductId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── SEED DANH MỤC ─────────────────────────────────────
        b.Entity<Category>().HasData(
            new Category { Id = 1,  Name = "CPU - Bộ vi xử lý",       Slug = "cpu",       CreatedAt = SeedDate },
            new Category { Id = 2,  Name = "GPU - Card đồ họa",       Slug = "gpu",       CreatedAt = SeedDate },
            new Category { Id = 3,  Name = "RAM - Bộ nhớ",            Slug = "ram",       CreatedAt = SeedDate },
            new Category { Id = 4,  Name = "SSD - Ổ cứng thể rắn",    Slug = "ssd",       CreatedAt = SeedDate },
            new Category { Id = 5,  Name = "HDD - Ổ cứng cơ",         Slug = "hdd",       CreatedAt = SeedDate },
            new Category { Id = 6,  Name = "Mainboard - Bo mạch chủ", Slug = "mainboard", CreatedAt = SeedDate },
            new Category { Id = 7,  Name = "PC Case - Vỏ máy tính",   Slug = "pccase",    CreatedAt = SeedDate },
            new Category { Id = 8,  Name = "PSU - Nguồn máy tính",    Slug = "psu",       CreatedAt = SeedDate },
            new Category { Id = 9,  Name = "Tản nhiệt",               Slug = "cooling",   CreatedAt = SeedDate },
            new Category { Id = 10, Name = "Monitor - Màn hình",      Slug = "monitor",   CreatedAt = SeedDate },
            new Category { Id = 11, Name = "Keyboard - Bàn phím",     Slug = "keyboard",  CreatedAt = SeedDate },
            new Category { Id = 12, Name = "Mouse - Chuột",           Slug = "mouse",     CreatedAt = SeedDate },
            new Category { Id = 13, Name = "Headset - Tai nghe",      Slug = "headset",   CreatedAt = SeedDate }
        );
    }
}
