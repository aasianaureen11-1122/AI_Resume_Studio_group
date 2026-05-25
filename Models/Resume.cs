using System.ComponentModel.DataAnnotations;

namespace AI_Resume.Models
{
    public class Resume
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Phone Number")]
        public string? Phone { get; set; }

        [Display(Name = "Job Title / Target Role")]
        public string? JobTitle { get; set; }

        [Display(Name = "Professional Summary")]
        public string? Summary { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Education> Educations { get; set; } = new();
        public List<WorkExperience> WorkExperiences { get; set; } = new();
        public List<Skill> Skills { get; set; } = new();

        public ApplicationUser? User { get; set; }
        public string? AIGeneratedResume { get; set; }
        public int? AIScore { get; set; }
        public string? AISummary { get; set; }
        public string? AISkillGaps { get; set; }
        public string? AIImprovements { get; set; }

        // Added missing properties
        public string? LinkedIn { get; set; }
        public string? Location { get; set; }
        
        public string? Template { get; set; } = "Default";
    }
}