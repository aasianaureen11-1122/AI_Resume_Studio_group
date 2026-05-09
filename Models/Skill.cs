namespace AI_Resume.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty; // Beginner, Intermediate, Expert

        public int ResumeId { get; set; }
        public Resume Resume { get; set; } = null!;
    }
}