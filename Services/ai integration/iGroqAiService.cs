using AI_Resume.Services.ai_integration.Models;

namespace AI_Resume.Services.ai_integration
{
    public interface IGroqAiService
    {
        Task<AIFeedback> AnalyzeResumeAsync(string resumeText);
        Task<string> GenerateResumeAsync(string userInfo);
    }
}
