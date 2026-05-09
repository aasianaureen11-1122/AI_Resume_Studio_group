namespace AI_Resume.Models
{
    public class WorkExperience
    {
        public int Id { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCurrentJob { get; set; } = false;

        public int ResumeId { get; set; }
        public Resume Resume { get; set; } = null!;
    }
}