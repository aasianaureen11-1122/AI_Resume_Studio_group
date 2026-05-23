namespace AI_Resume.Models
{
    public class SelectTemplateViewModel
    {
        public int ResumeId { get; set; }
        public string ResumeName { get; set; } = "";
        public string CurrentTemplate { get; set; } = "Classic";
        public string SelectedTemplate { get; set; } = "Classic";
    }
}