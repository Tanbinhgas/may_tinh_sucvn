namespace may_tinh_sucvn.Services;

public interface IOrderService
{
    /// <summary>
    /// Huỷ đơn + hoàn lại tồn kho trong một transaction (dùng chung cho khách và admin).
    /// - customerUserId != null: lối khách — chỉ huỷ đơn của chính mình và chỉ khi đang "Chờ xác nhận".
    /// - customerUserId == null: lối admin — huỷ được khi đơn chưa hoàn thành và chưa huỷ.
    /// Idempotent: đơn đã huỷ sẽ không bị hoàn kho lần hai.
    /// </summary>
    Task<(bool Ok, string Message)> CancelOrderAsync(int orderId, int? customerUserId = null);
}
