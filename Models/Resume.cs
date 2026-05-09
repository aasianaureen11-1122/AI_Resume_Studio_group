using System.ComponentModel.DataAnnotations;

namespace AI_Resume.Models
{
    public class Resume
    {
        public int Id { get; set; }

        // This links the resume to the logged-in user
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

        // Navigation properties (for related tables)
        public List<Education> Educations { get; set; } = new();
        public List<WorkExperience> WorkExperiences { get; set; } = new();
        public List<Skill> Skills { get; set; } = new();

        // Link back to the user (Identity)
        public ApplicationUser? User { get; set; }
    }
}
