namespace AI_Resume.Models
{
    public class ResumeAnalysis
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string Feedback { get; set; } = string.Empty;
        public string MissingSkills { get; set; } = string.Empty;
        public string Suggestions { get; set; } = string.Empty;
        public DateTime AnalyzedAt { get; set; } = DateTime.Now;

        public int ResumeId { get; set; }
        public Resume Resume { get; set; } = null!;
    }
}