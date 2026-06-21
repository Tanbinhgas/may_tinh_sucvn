using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace may_tinh_sucvn.Models;

public class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    /// <summary>Tổng tiền — luôn tính lại ở server lúc đặt hàng, không nhận từ client.</summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }

    [Required]
    public string ShippingAddress { get; set; } = string.Empty;

    [Required, MaxLength(15)]
    public string Phone { get; set; } = string.Empty;

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
