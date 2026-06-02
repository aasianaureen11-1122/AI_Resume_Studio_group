using System.ComponentModel.DataAnnotations;
namespace AI_Resume.Models
{
    public class Education
    {
        public int Id { get; set; }
        public int ResumeId { get; set; }
        public string? Degree { get; set; }
        public string? FieldOfStudy { get; set; }
        public string? Institution { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public string? Description { get; set; }

        public Resume? Resume { get; set; }
    }
}