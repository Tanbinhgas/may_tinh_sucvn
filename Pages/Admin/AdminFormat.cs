using may_tinh_sucvn.Models;

namespace may_tinh_sucvn.Pages.Admin;

/// <summary>Tiện ích định dạng dùng chung cho các trang quản trị.</summary>
public static class AdminFormat
{
    public static string Vnd(decimal amount) => amount.ToString("#,##0") + "₫";

    public static string StatusText(OrderStatus s) => s switch
    {
        OrderStatus.Pending   => "Chờ xác nhận",
        OrderStatus.Confirmed => "Đã xác nhận",
        OrderStatus.Shipping  => "Đang giao",
        OrderStatus.Done      => "Hoàn thành",
        OrderStatus.Cancelled => "Đã huỷ",
        _ => s.ToString()
    };

    public static string StatusBadge(OrderStatus s) => s switch
    {
        OrderStatus.Pending   => "amber",
        OrderStatus.Confirmed => "blue",
        OrderStatus.Shipping  => "purple",
        OrderStatus.Done      => "green",
        OrderStatus.Cancelled => "red",
        _ => "gray"
    };
}
