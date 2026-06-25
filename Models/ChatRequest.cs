namespace may_tinh_sucvn.Models;

/// <summary>Payload JSON nhận từ client khi gọi POST /api/Chatbot.</summary>
public class ChatRequest
{
    public string? Question { get; set; }
}
