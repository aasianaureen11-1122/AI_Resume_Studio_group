// C// Controllers/ResumeController.cs
using AI_Resume.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AI_Resume.Services.ai_integration;

[Authorize]
public class ResumeController : Controller
{
    private readonly ResumeService _resumeService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IGroqAiService _aiService;

    public ResumeController(ResumeService resumeService,
        UserManager<ApplicationUser> userManager,
        IGroqAiService aiService)
    {
        _resumeService = resumeService;
        _userManager = userManager;
        _aiService = aiService;
    }

    // GET: /Resume/Index
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var resumes = await _resumeService.GetUserResumesAsync(userId!);
        return View(resumes);
    }

    // GET: /Resume/Create
    public IActionResult Create()
    {
        return View(new Resume());
    }

    // POST: /Resume/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Resume resume)
    {
        if (!ModelState.IsValid) return View(resume);

        var userId = _userManager.GetUserId(User);
        await _resumeService.CreateResumeAsync(resume, userId!);

        var resumeText = $"Name: {resume.FullName}, " +
                         $"Email: {resume.Email}, " +
                         $"Phone: {resume.Phone}, " +
                         $"Job Title: {resume.JobTitle}, " +
                         $"Summary: {resume.Summary}";
        var aiFeedback = await _aiService.AnalyzeResumeAsync(resumeText);

        // Database mein save karo ← yeh add karo
        resume.AIScore = aiFeedback.Score;
        resume.AISummary = aiFeedback.Summary;
        resume.AISkillGaps = string.Join(", ", aiFeedback.SkillGaps);
        resume.AIImprovements = string.Join(", ", aiFeedback.Improvements);
        await _resumeService.UpdateResumeAsync(resume);

        TempData["AIScore"] = aiFeedback.Score;
        var aiGenerated = await _aiService.GenerateResumeAsync(resumeText);

        resume.AIScore = aiFeedback.Score;
        resume.AISummary = aiFeedback.Summary;
        resume.AISkillGaps = string.Join(", ", aiFeedback.SkillGaps);
        resume.AIImprovements = string.Join(", ", aiFeedback.Improvements);
        resume.AIGeneratedResume = aiGenerated;
        await _resumeService.UpdateResumeAsync(resume);

        TempData["AIScore"] = aiFeedback.Score;
        TempData["AISummary"] = aiFeedback.Summary;
        TempData["AISkillGaps"] = resume.AISkillGaps;
        TempData["AIImprovements"] = resume.AIImprovements;

        return RedirectToAction(nameof(Index));
    }

    // GET: /Resume/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var userId = _userManager.GetUserId(User);
        var resume = await _resumeService.GetResumeByIdAsync(id, userId!);
        if (resume == null) return NotFound();
        return View(resume);
    }

    // GET: /Resume/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var userId = _userManager.GetUserId(User);
        var resume = await _resumeService.GetResumeByIdAsync(id, userId!);
        if (resume == null) return NotFound();
        return View(resume);
    }

    // POST: /Resume/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Resume resume)
    {
        if (!ModelState.IsValid) return View(resume);
        await _resumeService.UpdateResumeAsync(resume);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Resume/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var userId = _userManager.GetUserId(User);
        var resume = await _resumeService.GetResumeByIdAsync(id, userId!);
        if (resume == null) return NotFound();
        return View(resume);
    }

    // POST: /Resume/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var userId = _userManager.GetUserId(User);
        await _resumeService.DeleteResumeAsync(id, userId!);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Resume/GenerateAI/5
    public async Task<IActionResult> GenerateAI(int id)
    {
        var userId = _userManager.GetUserId(User);
        var resume = await _resumeService.GetResumeByIdAsync(id, userId!);
        if (resume == null) return NotFound();

        if (!string.IsNullOrEmpty(resume.AIGeneratedResume))
        {
            ViewBag.GeneratedResume = resume.AIGeneratedResume;
            ViewBag.ResumeName = resume.FullName;
            return View();
        }

        var userInfo = $"Name: {resume.FullName}, " +
                       $"Email: {resume.Email}, " +
                       $"Phone: {resume.Phone}, " +
                       $"Job Title: {resume.JobTitle}, " +
                       $"Summary: {resume.Summary}";

        var generatedResume = await _aiService.GenerateResumeAsync(userInfo);
        resume.AIGeneratedResume = generatedResume;
        await _resumeService.UpdateResumeAsync(resume);

        ViewBag.GeneratedResume = generatedResume;
        ViewBag.ResumeName = resume.FullName;
        return View();
    }
}