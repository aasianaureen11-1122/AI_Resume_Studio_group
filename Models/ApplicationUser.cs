// Models/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;

namespace AI_Resume.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Resume> Resumes { get; set; } = new List<Resume>();
    }
}