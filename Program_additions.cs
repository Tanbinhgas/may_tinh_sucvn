// ===== THÊM VÀO Program.cs (sau builder.Services.AddRazorPages(...)) =====
//
// Đăng ký IHttpClientFactory cho ChatbotController
// (thay thế new HttpClient() antipattern cũ)
builder.Services.AddHttpClient("Gemini");

// Đăng ký API Controllers (ChatbotController dùng [ApiController])
builder.Services.AddControllers();

// ===== VÀ THÊM VÀO pipeline (sau app.MapRazorPages()) =====
app.MapControllers();
