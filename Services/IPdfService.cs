using AI_Resume.Models;

namespace AI_Resume.Services
{
    public interface IPdfService
    {
        Task<byte[]> GenerateResumePdfAsync(Resume resume);
    }
}