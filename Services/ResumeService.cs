// Services/ResumeService.cs
using AI_Resume.Data;
using AI_Resume.Models;
using Microsoft.EntityFrameworkCore;

public class ResumeService
{
    private readonly AppDbContext _context;

    public ResumeService(AppDbContext context)
    {
        _context = context;
    }

    // Get all resumes for a specific user
    public async Task<List<Resume>> GetUserResumesAsync(string userId)
    {
        return await _context.Resumes
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    // Get one resume with all related data
    public async Task<Resume?> GetResumeByIdAsync(int id, string userId)
    {
        return await _context.Resumes
            .Include(r => r.Educations)
            .Include(r => r.WorkExperiences)
            .Include(r => r.Skills)
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
    }

    // Create a new resume
    public async Task<Resume> CreateResumeAsync(Resume resume, string userId)
    {
        resume.UserId = userId;
        resume.CreatedAt = DateTime.UtcNow;
        _context.Resumes.Add(resume);
        await _context.SaveChangesAsync();
        return resume;
    }

    // Update existing resume
    public async Task UpdateResumeAsync(Resume resume)
    {
        _context.Resumes.Update(resume);
        await _context.SaveChangesAsync();
    }

    // Delete resume (only if it belongs to the user)
    public async Task<bool> DeleteResumeAsync(int id, string userId)
    {
        var resume = await _context.Resumes
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
        if (resume == null) return false;

        _context.Resumes.Remove(resume);
        await _context.SaveChangesAsync();
        return true;
    }
    public string? AIGeneratedResume { get; set; }
    public int? AIScore { get; set; }
    public string? AISummary { get; set; }
    public string? AISkillGaps { get; set; }
    public string? AIImprovements { get; set; }
}