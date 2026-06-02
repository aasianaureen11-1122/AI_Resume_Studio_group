using System.ComponentModel.DataAnnotations;
namespace AI_Resume.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public int ResumeId { get; set; }
        public string? Name { get; set; }
        public string? Level { get; set; }

        public Resume? Resume { get; set; }
    }
}