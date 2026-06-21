namespace may_tinh_sucvn.Models;

/// <summary>Vai trò tài khoản. Lưu xuống DB dưới dạng chuỗi "Customer"/"Admin".</summary>
public enum UserRole
{
    Customer,
    Admin
}

/// <summary>Trạng thái đơn hàng. Lưu xuống DB dưới dạng chuỗi.</summary>
public enum OrderStatus
{
    Pending,    // Chờ xác nhận
    Confirmed,  // Đã xác nhận
    Shipping,   // Đang giao
    Done,       // Hoàn thành
    Cancelled   // Đã huỷ
}
