using Microsoft.AspNetCore.Mvc;
using may_tinh_sucvn.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace may_tinh_sucvn.Controllers;

/// <summary>
/// API endpoint tư vấn linh kiện qua Gemini 2.5 Flash.
/// HttpClient được inject qua IHttpClientFactory (đăng ký bằng AddHttpClient trong Program.cs)
/// — tránh socket exhaustion của new HttpClient() antipattern.
/// API key đọc từ cấu hình (user-secrets / env), không được hard-code hay commit.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ChatbotController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;
    private readonly ILogger<ChatbotController> _logger;

    public ChatbotController(IHttpClientFactory httpClientFactory,
                             IConfiguration config,
                             ILogger<ChatbotController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Ask([FromBody] ChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request?.Question))
            return BadRequest(new { success = false, message = "Câu hỏi không được để trống." });

        var apiKey = _config["Gemini:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            _logger.LogError("Gemini:ApiKey chưa được cấu hình.");
            return StatusCode(503, new { success = false, message = "Chatbot chưa được cấu hình. Vui lòng thử lại sau." });
        }

        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

        var body = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = $"Bạn là trợ lý tư vấn linh kiện máy tính cho cửa hàng TKL Computer. " +
                                   $"Hãy trả lời ngắn gọn, chính xác bằng tiếng Việt. " +
                                   $"Câu hỏi: {request.Question}"
                        }
                    }
                }
            }
        };

        try
        {
            var client = _httpClientFactory.CreateClient("Gemini");
            client.Timeout = TimeSpan.FromSeconds(30);

            var json = JsonSerializer.Serialize(body);
            using var content = new StringContent(json, Encoding.UTF8, new MediaTypeHeaderValue("application/json"));
            using var response = await client.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Gemini API trả về {StatusCode}: {Body}", (int)response.StatusCode, err);
                return StatusCode(502, new { success = false, message = "Chatbot tạm thời không khả dụng." });
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseBody);
            var reply = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "Không có phản hồi.";

            return Ok(new { success = true, reply });
        }
        catch (TaskCanceledException)
        {
            _logger.LogWarning("Gemini API timeout khi xử lý câu hỏi: {Question}", request.Question);
            return StatusCode(504, new { success = false, message = "Chatbot phản hồi quá chậm. Vui lòng thử lại." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi gọi Gemini API.");
            return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi. Vui lòng thử lại sau." });
        }
    }
}
