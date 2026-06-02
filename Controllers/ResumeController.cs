using AI_Resume.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AI_Resume.Services.ai_integration;
using AI_Resume.Services;
using UglyToad.PdfPig;

[Authorize]
public class ResumeController : Controller
{
    private readonly ResumeService _resumeService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IGroqAiService _aiService;
    private readonly IPdfService _pdfService;

    public ResumeController(ResumeService resumeService,
        UserManager<ApplicationUser> userManager,
        IGroqAiService aiService,
        IPdfService pdfService)
    {
        _resumeService = resumeService;
        _userManager = userManager;
        _aiService = aiService;
        _pdfService = pdfService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var resumes = await _resumeService.GetUserResumesAsync(userId!);
        return View(resumes);
    }

    public IActionResult Create()
    {
        return View(new Resume());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Resume resume)
    {
        ModelState.Remove("UserId");
        ModelState.Remove("User");
        ModelState.Remove("AIScore");
        ModelState.Remove("AISummary");
        ModelState.Remove("AISkillGaps");
        ModelState.Remove("AIImprovements");
        ModelState.Remove("AIGeneratedResume");

        foreach (var key in ModelState.Keys
            .Where(k => k.EndsWith(".Resume")).ToList())
        {
            ModelState.Remove(key);
        }

        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value!.Errors.Count > 0)
                .Select(x => $"{x.Key}: {x.Value!.Errors.First().ErrorMessage}")
                .ToList();
            TempData["DebugErrors"] = string.Join(" | ", errors);
            return View(resume);
        }

        resume.Skills = resume.Skills?
            .Where(s => !string.IsNullOrWhiteSpace(s.Name))
            .ToList() ?? new List<Skill>();

        resume.Educations = resume.Educations?
            .Where(e => !string.IsNullOrWhiteSpace(e.Institution)
                     || !string.IsNullOrWhiteSpace(e.Degree))
            .ToList() ?? new List<Education>();

        resume.WorkExperiences = resume.WorkExperiences?
            .Where(w => !string.IsNullOrWhiteSpace(w.Company)
                     || !string.IsNullOrWhiteSpace(w.JobTitle))
            .ToList() ?? new List<WorkExperience>();

        var userId = _userManager.GetUserId(User);
        await _resumeService.CreateResumeAsync(resume, userId!);

        var edu = resume.Educations.Any()
            ? string.Join("; ", resume.Educations.Select(e =>
                $"{e.Degree} in {e.FieldOfStudy} at {e.Institution} " +
                $"({e.StartYear}-{(e.EndYear.HasValue ? e.EndYear.Value.ToString() : "Present")})"))
            : "None provided";

        var skills = resume.Skills.Any()
            ? string.Join(", ", resume.Skills.Select(s => $"{s.Name} ({s.Level})"))
            : "None provided";

        var exp = resume.WorkExperiences.Any()
            ? string.Join("; ", resume.WorkExperiences.Select(w =>
                $"{w.JobTitle} at {w.Company}, {w.Location} " +
                $"({w.StartDate?.ToString("MMM yyyy")} - " +
                $"{(w.IsCurrent ? "Present" : w.EndDate?.ToString("MMM yyyy") ?? "Present")}) " +
                $"{w.Description}"))
            : "None provided";

        var resumeText =
            $"Name: {resume.FullName} | " +
            $"Email: {resume.Email} | " +
            $"Phone: {resume.Phone} | " +
            $"Location: {resume.Location} | " +
            $"LinkedIn: {resume.LinkedIn} | " +
            $"Target Role: {resume.JobTitle} | " +
            $"Summary: {resume.Summary} | " +
            $"Education: {edu} | " +
            $"Skills: {skills} | " +
            $"Experience: {exp}";

        var aiFeedback = await _aiService.AnalyzeResumeAsync(resumeText);
        var aiGenerated = await _aiService.GenerateResumeAsync(resumeText);

        resume.AIScore = aiFeedback.Score;
        resume.AISummary = aiFeedback.Summary;
        resume.AISkillGaps = string.Join(", ", aiFeedback.SkillGaps);
        resume.AIImprovements = string.Join(", ", aiFeedback.Improvements);
        resume.AIGeneratedResume = aiGenerated;

        await _resumeService.UpdateResumeAsync(resume);

        TempData["AIScore"] = aiFeedback.Score.ToString();
        TempData["AISummary"] = aiFeedback.Summary ?? string.Empty;
        TempData["AISkillGaps"] = resume.AISkillGaps ?? string.Empty;
        TempData["AIImprovements"] = resume.AIImprovements ?? string.Empty;

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var userId = _userManager.GetUserId(User);
        var resume = await _resumeService.GetResumeByIdAsync(id, userId!);
        if (resume == null) return NotFound();
        return View(resume);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var userId = _userManager.GetUserId(User);
        var resume = await _resumeService.GetResumeByIdAsync(id, userId!);
        if (resume == null) return NotFound();
        return View(resume);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Resume resume)
    {
        ModelState.Remove("UserId");
        ModelState.Remove("User");
        foreach (var key in ModelState.Keys
            .Where(k => k.EndsWith(".Resume")).ToList())
        {
            ModelState.Remove(key);
        }
        if (!ModelState.IsValid) return View(resume);
        await _resumeService.UpdateResumeAsync(resume);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = _userManager.GetUserId(User);
        var resume = await _resumeService.GetResumeByIdAsync(id, userId!);
        if (resume == null) return NotFound();
        return View(resume);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var userId = _userManager.GetUserId(User);
        await _resumeService.DeleteResumeAsync(id, userId!);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> GenerateAI(int id)
    {
        var userId = _userManager.GetUserId(User);
        var resume = await _resumeService.GetResumeByIdAsync(id, userId!);
        if (resume == null) return NotFound();

        var edu = (resume.Educations != null && resume.Educations.Any())
            ? string.Join("; ", resume.Educations.Select(e =>
                $"{e.Degree} in {e.FieldOfStudy} at {e.Institution} " +
                $"({e.StartYear}-{(e.EndYear.HasValue ? e.EndYear.Value.ToString() : "Present")})"))
            : "None provided";

        var skills = (resume.Skills != null && resume.Skills.Any())
            ? string.Join(", ", resume.Skills.Select(s => $"{s.Name} ({s.Level})"))
            : "None provided";

        var exp = (resume.WorkExperiences != null && resume.WorkExperiences.Any())
            ? string.Join("; ", resume.WorkExperiences.Select(w =>
                $"{w.JobTitle} at {w.Company}, {w.Location} " +
                $"({w.StartDate?.ToString("MMM yyyy")} - " +
                $"{(w.IsCurrent ? "Present" : w.EndDate?.ToString("MMM yyyy") ?? "Present")}) " +
                $"{w.Description}"))
            : "None provided";

        var userInfo =
            $"Name: {resume.FullName} | " +
            $"Email: {resume.Email} | " +
            $"Phone: {resume.Phone} | " +
            $"Location: {resume.Location} | " +
            $"LinkedIn: {resume.LinkedIn} | " +
            $"Target Role: {resume.JobTitle} | " +
            $"Summary: {resume.Summary} | " +
            $"Education: {edu} | " +
            $"Skills: {skills} | " +
            $"Experience: {exp}";

        var generatedResume = await _aiService.GenerateResumeAsync(userInfo);
        resume.AIGeneratedResume = generatedResume;
        await _resumeService.UpdateResumeAsync(resume);

        ViewBag.GeneratedResume = generatedResume;
        ViewBag.ResumeName = resume.FullName;
        return View();
    }

    public IActionResult UploadResume() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadResume(IFormFile resumeFile)
    {
        if (resumeFile == null || resumeFile.Length == 0)
        { ViewBag.Error = "Please select a PDF file."; return View(); }

        if (!resumeFile.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
        { ViewBag.Error = "Only PDF files are allowed."; return View(); }

        string resumeText = "";
        using (var stream = resumeFile.OpenReadStream())
        using (var pdfDoc = PdfDocument.Open(stream))
            foreach (var page in pdfDoc.GetPages())
                resumeText += page.Text + "\n";

        if (string.IsNullOrWhiteSpace(resumeText))
        { ViewBag.Error = "Could not read text from the PDF."; return View(); }

        var analysis = await _aiService.AnalyzeUploadedResumeAsync(resumeText);
        ViewBag.Score = analysis.Score;
        ViewBag.Summary = analysis.Summary;
        ViewBag.SkillGaps = analysis.SkillGaps;
        ViewBag.Improvements = analysis.Improvements;
        return View("UploadResult");
    }

    // ── THIS IS THE ACTION THE DOWNLOAD BUTTON CALLS ──
    [HttpGet]
    public async Task<IActionResult> DownloadPdf(int id)
    {
        var userId = _userManager.GetUserId(User);
        var resume = await _resumeService.GetResumeByIdAsync(id, userId!);
        if (resume == null) return NotFound();

        byte[] pdfBytes = await _pdfService.GenerateResumePdfAsync(resume);
        string fileName = $"{resume.FullName?.Replace(" ", "_") ?? "Resume"}_CV.pdf";
        return File(pdfBytes, "application/pdf", fileName);
    }
}