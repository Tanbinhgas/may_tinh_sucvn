using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;
using may_tinh_sucvn.Controllers;
using may_tinh_sucvn.Models;
using Xunit;

namespace may_tinh_sucvn.Tests;

public class ChatbotControllerTests
{
    // ------------------------------------------------------------------ helpers

    /// <summary>Dựng IHttpClientFactory trả về response giả định.</summary>
    private static IHttpClientFactory MakeFactory(HttpStatusCode status, string body)
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(status)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            });

        var client = new HttpClient(handler.Object);
        var factory = new Mock<IHttpClientFactory>();
        factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);
        return factory.Object;
    }

    private static IConfiguration MakeConfig(string? apiKey) =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(apiKey is null
                ? Array.Empty<KeyValuePair<string, string?>>()
                : new[] { new KeyValuePair<string, string?>("Gemini:ApiKey", apiKey) })
            .Build();

    private static string GeminiOkBody(string reply) => JsonSerializer.Serialize(new
    {
        candidates = new[]
        {
            new { content = new { parts = new[] { new { text = reply } } } }
        }
    });

    // ------------------------------------------------------------------ tests

    [Fact]
    public async Task Ask_EmptyQuestion_ReturnsBadRequest()
    {
        var ctrl = new ChatbotController(MakeFactory(HttpStatusCode.OK, ""), MakeConfig("key"), NullLogger<ChatbotController>.Instance);

        var result = await ctrl.Ask(new ChatRequest { Question = "   " });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Ask_NullQuestion_ReturnsBadRequest()
    {
        var ctrl = new ChatbotController(MakeFactory(HttpStatusCode.OK, ""), MakeConfig("key"), NullLogger<ChatbotController>.Instance);

        var result = await ctrl.Ask(new ChatRequest { Question = null });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Ask_MissingApiKey_Returns503()
    {
        var ctrl = new ChatbotController(MakeFactory(HttpStatusCode.OK, ""), MakeConfig(null), NullLogger<ChatbotController>.Instance);

        var result = await ctrl.Ask(new ChatRequest { Question = "RAM nào tốt?" }) as ObjectResult;

        Assert.Equal(503, result?.StatusCode);
    }

    [Fact]
    public async Task Ask_GeminiOk_ReturnsSuccessWithReply()
    {
        var expectedReply = "RAM DDR5 32GB là lựa chọn tốt cho gaming.";
        var ctrl = new ChatbotController(
            MakeFactory(HttpStatusCode.OK, GeminiOkBody(expectedReply)),
            MakeConfig("fake-key"),
            NullLogger<ChatbotController>.Instance);

        var result = await ctrl.Ask(new ChatRequest { Question = "RAM nào tốt?" }) as OkObjectResult;

        Assert.NotNull(result);
        var json = JsonSerializer.Serialize(result!.Value);
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.GetProperty("success").GetBoolean());
        Assert.Equal(expectedReply, doc.RootElement.GetProperty("reply").GetString());
    }

    [Fact]
    public async Task Ask_GeminiNon2xx_Returns502()
    {
        var ctrl = new ChatbotController(
            MakeFactory(HttpStatusCode.InternalServerError, "{}"),
            MakeConfig("fake-key"),
            NullLogger<ChatbotController>.Instance);

        var result = await ctrl.Ask(new ChatRequest { Question = "GPU nào?" }) as ObjectResult;

        Assert.Equal(502, result?.StatusCode);
    }
}
