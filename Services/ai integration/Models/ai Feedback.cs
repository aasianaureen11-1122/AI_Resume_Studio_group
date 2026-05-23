namespace AI_Resume.Services.ai_integration.Models
{
    public class AIFeedback
    {
        public int Score { get; set; }
        public List<string> SkillGaps { get; set; } = new();
        public List<string> Improvements { get; set; } = new();
        public string Summary { get; set; } = "";
    }
}