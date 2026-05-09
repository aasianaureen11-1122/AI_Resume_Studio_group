using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AI_Resume.Services.ai_integration.Models;

namespace AI_Resume.Services.ai_integration
{
    public class GroqAiService : IGroqAiService
    {
        private readonly HttpClient _http;
        private readonly string _key;
        private readonly string _url =
            "https://api.groq.com/openai/v1/chat/completions";

        public GroqAiService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _key = config["Groq:ApiKey"]!;
        }

        public async Task<AIFeedback> AnalyzeResumeAsync(string resumeText)
        {
            var body = new
            {
                model = "llama3-8b-8192",
                messages = new[]
                {
                    new { role = "system", content = "Reply ONLY with JSON: {\"score\":0,\"skillGaps\":[],\"improvements\":[],\"summary\":\"\"}" },
                    new { role = "user", content = resumeText }
                },
                temperature = 0.3,
                max_tokens = 800
            };

            var req = new HttpRequestMessage(HttpMethod.Post, _url)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };
            req.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _key);

            var res = await _http.SendAsync(req);
            res.EnsureSuccessStatusCode();
            var raw = await res.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(raw);
            var txt = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content").GetString()!;

            return JsonSerializer.Deserialize<AIFeedback>(txt,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new AIFeedback { Summary = "Error" };
        }
    }
}