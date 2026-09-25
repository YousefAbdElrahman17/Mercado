// Services/GeminiService.cs
using System.Text;
using System.Text.Json;

namespace Mercado.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public GeminiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<string> AskAsync(string context, string question)
        {
            var apiKey = _config["Gemini:ApiKey"];
            var model = "gemini-3.6-flash";
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            var fullPrompt = $"""
                You are a smart assistant for the Mercado Inventory Management System.
                Answer based only on the data provided below. If the question is unrelated to it,
                say that you specialize in inventory-related questions only.

                Available data:
                {context}

                User question: {question}
                """;

            var requestBody = new
            {
                contents = new[] { new { parts = new[] { new { text = fullPrompt } } } }
            };

            var response = await _httpClient.PostAsync(url,
                new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            return doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "Sorry, I couldn't generate a response right now.";
        }
    }
}