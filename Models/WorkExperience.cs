using System.ComponentModel.DataAnnotations;

namespace AI_Resume.Models
{
    public class WorkExperience
    {
        public int Id { get; set; }
        public int ResumeId { get; set; }
        public string? JobTitle { get; set; }
        public string? Company { get; set; }
        public string? Location { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrent { get; set; }
        public string? Description { get; set; }
        public Resume? Resume { get; set; }
    }
}