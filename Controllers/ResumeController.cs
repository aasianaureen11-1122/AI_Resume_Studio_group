// Controllers/ResumeController.cs
using AI_Resume.Models;
using AI_Resume.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize]  // All actions require login
public class ResumeController : Controller
{
    private readonly ResumeService _resumeService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ResumeController(ResumeService resumeService, UserManager<ApplicationUser> userManager)
    {
        _resumeService = resumeService;
        _userManager = userManager;
    }

    // GET: /Resume/Index — list all resumes
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
}