using AI_Resume.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AI_Resume.Services
{
    public class PdfService : IPdfService
    {
        public Task<byte[]> GenerateResumePdfAsync(Resume resume)
        {
           

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    page.Content().Column(col =>
                    {
                        // ── HEADER ──
                        col.Item().BorderBottom(2).BorderColor("#1a56db").PaddingBottom(10).Column(h =>
                        {
                            h.Item().Text(resume.FullName ?? "")
                                .FontSize(24).Bold().FontColor("#0a1628");

                            if (!string.IsNullOrEmpty(resume.JobTitle))
                                h.Item().Text(resume.JobTitle)
                                    .FontSize(13).FontColor("#1a56db");

                            h.Item().PaddingTop(5).Row(row =>
                            {
                                if (!string.IsNullOrEmpty(resume.Email))
                                    row.AutoItem().Text($"✉ {resume.Email}  ").FontSize(10).FontColor("#555555");
                                if (!string.IsNullOrEmpty(resume.Phone))
                                    row.AutoItem().Text($"📞 {resume.Phone}  ").FontSize(10).FontColor("#555555");
                                if (!string.IsNullOrEmpty(resume.Location))
                                    row.AutoItem().Text($"📍 {resume.Location}  ").FontSize(10).FontColor("#555555");
                                if (!string.IsNullOrEmpty(resume.LinkedIn))
                                    row.AutoItem().Text($"🔗 {resume.LinkedIn}").FontSize(10).FontColor("#555555");
                            });
                        });

                        // ── AI SCORE ──
                        if (resume.AIScore.HasValue)
                        {
                            col.Item().PaddingTop(12).Background("#f0f7ff")
                                .Border(1).BorderColor("#bee3f8")
                                .Padding(10).Row(row =>
                                {
                                    row.AutoItem().Text($"{resume.AIScore}/100")
                                        .FontSize(22).Bold().FontColor("#1a56db");
                                    row.ConstantItem(12);
                                    row.RelativeItem().Column(c =>
                                    {
                                        c.Item().Text("AI Resume Score").Bold().FontSize(11);
                                        if (!string.IsNullOrEmpty(resume.AISummary))
                                            c.Item().Text(resume.AISummary).FontSize(10).FontColor("#555555");
                                    });
                                });
                        }

                        // ── SUMMARY ──
                        if (!string.IsNullOrEmpty(resume.Summary))
                        {
                            col.Item().PaddingTop(14).Column(s =>
                            {
                                s.Item().Text("PROFESSIONAL SUMMARY")
                                    .FontSize(10).Bold().FontColor("#1a56db")
                                    .LetterSpacing(0.05f);
                                s.Item().BorderBottom(1).BorderColor("#e0e4ed").PaddingBottom(3);
                                s.Item().PaddingTop(5).Text(resume.Summary).FontColor("#333333");
                            });
                        }

                        // ── EDUCATION ──
                        if (resume.Educations != null && resume.Educations.Any())
                        {
                            col.Item().PaddingTop(14).Column(s =>
                            {
                                s.Item().Text("EDUCATION")
                                    .FontSize(10).Bold().FontColor("#1a56db");
                                s.Item().BorderBottom(1).BorderColor("#e0e4ed").PaddingBottom(3);
                                foreach (var e in resume.Educations)
                                {
                                    s.Item().PaddingTop(7).Column(entry =>
                                    {
                                        entry.Item().Text($"{e.Degree}{(string.IsNullOrEmpty(e.FieldOfStudy) ? "" : $" in {e.FieldOfStudy}")}")
                                            .Bold().FontColor("#0a1628");
                                        entry.Item().Text(e.Institution ?? "").FontColor("#1a56db").FontSize(10);
                                        entry.Item().Text($"{e.StartYear} – {(e.EndYear.HasValue ? e.EndYear.Value.ToString() : "Present")}")
                                            .FontSize(10).FontColor("#888888");
                                        if (!string.IsNullOrEmpty(e.Description))
                                            entry.Item().Text(e.Description).FontSize(10).FontColor("#444444");
                                    });
                                }
                            });
                        }

                        // ── WORK EXPERIENCE ──
                        if (resume.WorkExperiences != null && resume.WorkExperiences.Any())
                        {
                            col.Item().PaddingTop(14).Column(s =>
                            {
                                s.Item().Text("WORK EXPERIENCE")
                                    .FontSize(10).Bold().FontColor("#1a56db");
                                s.Item().BorderBottom(1).BorderColor("#e0e4ed").PaddingBottom(3);
                                foreach (var w in resume.WorkExperiences)
                                {
                                    s.Item().PaddingTop(7).Column(entry =>
                                    {
                                        entry.Item().Text(w.JobTitle ?? "").Bold().FontColor("#0a1628");
                                        entry.Item().Text($"{w.Company}{(string.IsNullOrEmpty(w.Location) ? "" : $" — {w.Location}")}")
                                            .FontColor("#1a56db").FontSize(10);
                                        entry.Item().Text(
                                            $"{w.StartDate?.ToString("MMM yyyy")} – " +
                                            $"{(w.IsCurrent ? "Present" : w.EndDate?.ToString("MMM yyyy") ?? "Present")}")
                                            .FontSize(10).FontColor("#888888");
                                        if (!string.IsNullOrEmpty(w.Description))
                                            entry.Item().Text(w.Description).FontSize(10).FontColor("#444444");
                                    });
                                }
                            });
                        }

                        // ── SKILLS ──
                        if (resume.Skills != null && resume.Skills.Any())
                        {
                            col.Item().PaddingTop(14).Column(s =>
                            {
                                s.Item().Text("SKILLS")
                                    .FontSize(10).Bold().FontColor("#1a56db");
                                s.Item().BorderBottom(1).BorderColor("#e0e4ed").PaddingBottom(3);
                                s.Item().PaddingTop(6).Text(
                                    string.Join("  •  ", resume.Skills.Select(sk =>
                                        $"{sk.Name}{(string.IsNullOrEmpty(sk.Level) ? "" : $" ({sk.Level})")}"))
                                ).FontColor("#333333");
                            });
                        }

                        // ── AI SUGGESTIONS ──
                        if (!string.IsNullOrEmpty(resume.AIImprovements))
                        {
                            col.Item().PaddingTop(14).Background("#fffbea")
                                .Border(1).BorderColor("#fde68a").Padding(10).Column(s =>
                                {
                                    s.Item().Text("💡 AI Improvement Suggestions").Bold().FontColor("#92400e");
                                    s.Item().PaddingTop(4).Text(resume.AIImprovements).FontSize(10).FontColor("#78350f");
                                });
                        }

                        // ── SKILL GAPS ──
                        if (!string.IsNullOrEmpty(resume.AISkillGaps))
                        {
                            col.Item().PaddingTop(10).Background("#fef2f2")
                                .Border(1).BorderColor("#fecaca").Padding(10).Column(s =>
                                {
                                    s.Item().Text("⚠ Skill Gaps Detected").Bold().FontColor("#991b1b");
                                    s.Item().PaddingTop(4).Text(resume.AISkillGaps).FontSize(10).FontColor("#7f1d1d");
                                });
                        }
                    });
                });
            });

            return Task.FromResult(pdf.GeneratePdf());
        }
    }
}