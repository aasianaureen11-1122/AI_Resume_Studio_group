namespace AI_Resume.Models
{
    public class Education
    {
        public int Id { get; set; }
        public string Degree { get; set; } = string.Empty;
        public string Institution { get; set; } = string.Empty;
        public string FieldOfStudy { get; set; } = string.Empty;
        public int StartYear { get; set; }
        public int EndYear { get; set; }

        public int ResumeId { get; set; }
        public Resume Resume { get; set; } = null!;
    }
}
