using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace may_tinh_sucvn.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatbotController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public ChatbotController(IConfiguration config)
    {
        _config = config;
        _httpClient = new HttpClient();
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    [HttpPost]
    public async Task<IActionResult> Chat([FromBody] ChatRequest request)
    {
        try
        {
            var apiKey = _config["Gemini:ApiKey"];
            
            if (string.IsNullOrEmpty(apiKey))
            {
                return BadRequest(new { 
                    success = false, 
                    message = "Chưa cấu hình API Key Gemini" 
                });
            }

            if (string.IsNullOrWhiteSpace(request.Question))
            {
                return BadRequest(new { 
                    success = false, 
                    message = "Vui lòng nhập câu hỏi." 
                });
            }

            // ✅ Dùng gemini-2.5-pro (mới nhất)
var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";
            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = request.Question }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync(url, content);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                using var doc = JsonDocument.Parse(jsonResponse);
                var reply = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return Ok(new { 
                    success = true, 
                    reply = reply ?? "Xin lỗi, tôi không có câu trả lời." 
                });
            }
            else
            {
                return BadRequest(new { 
                    success = false, 
                    message = $"Gemini API lỗi: {jsonResponse}" 
                });
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { 
                success = false, 
                message = $"Lỗi: {ex.Message}" 
            });
        }
    }
}

public class ChatRequest
{
    public string Question { get; set; } = "";
}