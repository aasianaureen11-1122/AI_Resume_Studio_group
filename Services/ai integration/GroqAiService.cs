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
            var requestBody = new
            {
                model = "llama-3.3-70b-versatile",
                messages = new[]
                {
                    new {
                        role = "system",
                        content = "You are a resume reviewer. Analyze the resume and respond ONLY with a JSON object. No extra text. Format: {\"score\": 80, \"skillGaps\": [\"gap1\"], \"improvements\": [\"tip1\"], \"summary\": \"overall summary\"}"
                    },
                    new {
                        role = "user",
                        content = "Analyze this resume: " + resumeText
                    }
                },
                temperature = 0.3,
                max_tokens = 1000
            };

            var json = JsonSerializer.Serialize(requestBody);
            var req = new HttpRequestMessage(HttpMethod.Post, _url)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            req.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _key);

            var res = await _http.SendAsync(req);
            var raw = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
                return new AIFeedback { Summary = "AI service error: " + raw };

            using var doc = JsonDocument.Parse(raw);
            var txt = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content").GetString()!;

            txt = txt.Trim();
            if (txt.Contains("```"))
                txt = txt.Replace("```json", "").Replace("```", "").Trim();

            return JsonSerializer.Deserialize<AIFeedback>(txt,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new AIFeedback { Summary = "Could not parse AI response" };
        }

        public async Task<string> GenerateResumeAsync(string userInfo)
        {
            var requestBody = new
            {
                model = "llama-3.3-70b-versatile",
                messages = new[]
                {
                    new {
                        role = "system",
                        content = """
    You are a professional resume writer. Generate a clean, modern resume.
    
    STRICT RULES:
    - Do NOT write labels like "PROFESSIONAL SUMMARY:" or "EDUCATION:" as plain text in the content
    - Use markdown: ## for section headings, ** for bold, * for bullet points
    - Do NOT invent any information not provided
    - Do NOT add placeholders like [Date] or [Institution]
    - Skip any section that has no data
    
    OUTPUT FORMAT (use exactly this markdown structure):
    
    # Full Name
    Email | Phone | Location | LinkedIn
    
    ---
    
    ## Summary
    Write 2-3 sentence professional summary here.
    
    ## Education
    **Degree in Field** — Institution
    StartYear – EndYear
    
    ## Work Experience
    **Job Title** — Company, Location
    *Start – End*
    * Key responsibility or achievement
    
    ## Skills
    **Skill 1** (Level) · **Skill 2** (Level) · **Skill 3** (Level)
    """ },
                    new {
                        role = "user",
                        content = "Generate a professional resume for: " + userInfo
                    }
                },
                temperature = 0.4,
                max_tokens = 2000
            };

            var json = JsonSerializer.Serialize(requestBody);
            var req = new HttpRequestMessage(HttpMethod.Post, _url)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            req.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _key);

            var res = await _http.SendAsync(req);
            var raw = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
                return "Error generating resume: " + raw;

            using var doc = JsonDocument.Parse(raw);
            return doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content").GetString()!;
        }

        public async Task<AIFeedback> AnalyzeUploadedResumeAsync(string resumeText)
        {
            var requestBody = new
            {
                model = "llama-3.3-70b-versatile",
                messages = new[]
                {
                    new {
                        role = "system",
                        content = "You are a resume reviewer. Analyze the resume and respond ONLY with a JSON object. No extra text. Format: {\"score\": 80, \"skillGaps\": [\"gap1\"], \"improvements\": [\"tip1\"], \"summary\": \"overall summary\"}"
                    },
                    new {
                        role = "user",
                        content = "Analyze this resume: " + resumeText
                    }
                },
                temperature = 0.3,
                max_tokens = 1000
            };

            var json = JsonSerializer.Serialize(requestBody);
            var req = new HttpRequestMessage(HttpMethod.Post, _url)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            req.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _key);

            var res = await _http.SendAsync(req);
            var raw = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
                return new AIFeedback { Summary = "AI service error: " + raw };

            using var doc = JsonDocument.Parse(raw);
            var txt = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content").GetString()!;

            txt = txt.Trim();
            if (txt.Contains("```"))
                txt = txt.Replace("```json", "").Replace("```", "").Trim();

            return JsonSerializer.Deserialize<AIFeedback>(txt,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new AIFeedback { Summary = "Could not parse AI response" };
        }
    }
}