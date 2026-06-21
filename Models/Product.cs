using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace may_tinh_sucvn.Models;

public class Product
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Slug { get; set; } = string.Empty;

    public string? Description { get; set; }

    /// <summary>Giá VNĐ — NGUỒN SỰ THẬT về giá. Client không bao giờ gửi giá lên.</summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public int Stock { get; set; }

    [MaxLength(255)]
    public string? ImageUrl { get; set; }

    [MaxLength(100)]
    public string? Brand { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Category? Category { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
