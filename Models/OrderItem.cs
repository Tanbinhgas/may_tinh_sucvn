using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace may_tinh_sucvn.Models;

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    /// <summary>Snapshot tên + giá lúc mua, để lịch sử đơn không đổi khi sản phẩm đổi giá.</summary>
    [Required, MaxLength(200)]
    public string ProductName { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public int Quantity { get; set; } = 1;

    public Order? Order { get; set; }
    public Product? Product { get; set; }
}
