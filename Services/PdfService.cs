using DinkToPdf;
using DinkToPdf.Contracts;
using System.Text;
using AI_Resume.Models;

namespace AI_Resume.Services
{
    public interface IPdfService
    {
        Task<byte[]> GenerateResumePdfAsync(Resume resume);
    }

    public class PdfService : IPdfService
    {
        private readonly IConverter _converter;

        public PdfService(IConverter converter)
        {
            _converter = converter;
        }

        public Task<byte[]> GenerateResumePdfAsync(Resume resume)
        {
            string html = BuildResumeHtml(resume);

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode   = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize   = PaperKind.A4,
                    Margins     = new MarginSettings { Top = 20, Bottom = 20, Left = 15, Right = 15 }
                },
                Objects = {
                    new ObjectSettings {
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            byte[] pdf = _converter.Convert(doc);
            return Task.FromResult(pdf);
        }

        // ── HTML BUILDER ──────────────────────────────────────────────────────

        private string BuildResumeHtml(Resume resume)
        {
            string css = (resume.Template ?? "Classic") switch
            {
                "Modern" => ModernCss(),
                "Minimal" => MinimalCss(),
                _ => ClassicCss()
            };

            var sb = new StringBuilder();
            sb.Append($@"<!DOCTYPE html>
<html>
<head>
<meta charset='UTF-8'>
<style>{css}</style>
</head>
<body>
<div class='resume'>

  <header class='rh'>
    <h1>{System.Net.WebUtility.HtmlEncode(resume.FullName ?? "")}</h1>
    <p class='tagline'>{System.Net.WebUtility.HtmlEncode(resume.JobTitle ?? "")}</p>
    <div class='contact-row'>
      <span>{System.Net.WebUtility.HtmlEncode(resume.Email ?? "")}</span>
      {(string.IsNullOrEmpty(resume.Phone) ? "" : $"<span> | {System.Net.WebUtility.HtmlEncode(resume.Phone)}</span>")}
      {(string.IsNullOrEmpty(resume.Location) ? "" : $"<span> | {System.Net.WebUtility.HtmlEncode(resume.Location)}</span>")}
      {(string.IsNullOrEmpty(resume.LinkedIn) ? "" : $"<span> | {System.Net.WebUtility.HtmlEncode(resume.LinkedIn)}</span>")}
    </div>
  </header>

  {(string.IsNullOrEmpty(resume.Summary) ? "" : $@"
  <section>
    <h2 class='st'>Professional Summary</h2>
    <p class='summary'>{System.Net.WebUtility.HtmlEncode(resume.Summary)}</p>
  </section>")}

  {BuildWorkSection(resume)}
  {BuildEducationSection(resume)}
  {BuildSkillsSection(resume)}

</div>
</body>
</html>");
            return sb.ToString();
        }

        private string BuildWorkSection(Resume resume)
        {
            if (resume.WorkExperiences == null || !resume.WorkExperiences.Any())
                return "";

            var sb = new StringBuilder();
            sb.Append("<section><h2 class='st'>Work Experience</h2>");

            foreach (var w in resume.WorkExperiences.OrderByDescending(x => x.StartDate))
            {
                // Works whether EndDate is DateTime or DateTime?
                string endDate;
                try
                {
                    endDate = w.IsCurrent ? "Present" : w.EndDate.ToString("MMM yyyy");
                }
                catch
                {
                    endDate = "Present";
                }

                sb.Append($@"
  <div class='entry'>
    <div class='eh'>
      <span class='etitle'>{System.Net.WebUtility.HtmlEncode(w.JobTitle ?? "")}</span>
      <span class='edate'>{w.StartDate.ToString("MMM yyyy")} – {endDate}</span>
    </div>
    <span class='esub'>{System.Net.WebUtility.HtmlEncode(w.Company ?? "")}{(string.IsNullOrEmpty(w.Location) ? "" : ", " + System.Net.WebUtility.HtmlEncode(w.Location))}</span>
    {(string.IsNullOrEmpty(w.Description) ? "" : $"<p class='ebody'>{System.Net.WebUtility.HtmlEncode(w.Description)}</p>")}
  </div>");
            }

            sb.Append("</section>");
            return sb.ToString();
        }

        private string BuildEducationSection(Resume resume)
        {
            if (resume.Educations == null || !resume.Educations.Any())
                return "";

            var sb = new StringBuilder();
            sb.Append("<section><h2 class='st'>Education</h2>");

            foreach (var e in resume.Educations.OrderByDescending(x => x.StartYear))
            {
                // Works whether EndYear is int or int?
                string endYear;
                try
                {
                    endYear = (e.EndYear > 0) ? e.EndYear.ToString() : "Present";
                }
                catch
                {
                    endYear = "Present";
                }

                sb.Append($@"
  <div class='entry'>
    <div class='eh'>
      <span class='etitle'>{System.Net.WebUtility.HtmlEncode(e.Degree ?? "")} in {System.Net.WebUtility.HtmlEncode(e.FieldOfStudy ?? "")}</span>
      <span class='edate'>{e.StartYear} – {endYear}</span>
    </div>
    <span class='esub'>{System.Net.WebUtility.HtmlEncode(e.Institution ?? "")}</span>
    {(string.IsNullOrEmpty(e.Description) ? "" : $"<p class='ebody'>{System.Net.WebUtility.HtmlEncode(e.Description)}</p>")}
  </div>");
            }

            sb.Append("</section>");
            return sb.ToString();
        }

        private string BuildSkillsSection(Resume resume)
        {
            if (resume.Skills == null || !resume.Skills.Any())
                return "";

            string pills = string.Join("",
                resume.Skills.Select(s =>
                    $"<span class='pill'>{System.Net.WebUtility.HtmlEncode(s.Name ?? "")}</span>"));

            return $"<section><h2 class='st'>Skills</h2><div class='skills'>{pills}</div></section>";
        }

        // ── TEMPLATE: CLASSIC ─────────────────────────────────────────────────

        private string ClassicCss() => @"
body {
  font-family: Georgia, 'Times New Roman', serif;
  color: #1a1a1a;
  font-size: 10.5pt;
  margin: 0; padding: 0;
}
.resume  { max-width: 760px; margin: 0 auto; padding: 20px; }
.rh      { border-bottom: 2px solid #1a1a1a; padding-bottom: 12px; margin-bottom: 16px; }
h1       { font-size: 24pt; margin: 0 0 3px; letter-spacing: 0.5px; }
.tagline { color: #555; font-size: 11pt; margin: 0 0 6px; }
.contact-row { font-size: 9pt; color: #666; }
.st {
  font-size: 10pt; text-transform: uppercase; letter-spacing: 2px;
  border-bottom: 1px solid #ccc; padding-bottom: 3px; margin: 18px 0 8px; color: #1a1a1a;
}
.entry  { margin-bottom: 13px; }
.eh     { display: flex; justify-content: space-between; margin-bottom: 1px; }
.etitle { font-weight: bold; font-size: 10.5pt; }
.edate  { font-size: 9pt; color: #666; }
.esub   { font-style: italic; font-size: 9.5pt; color: #444; display: block; margin-bottom: 3px; }
.ebody  { font-size: 9.5pt; line-height: 1.55; margin: 3px 0 0; }
.summary { font-size: 9.5pt; line-height: 1.65; margin: 0; }
.skills { display: flex; flex-wrap: wrap; gap: 6px; margin-top: 4px; }
.pill   { border: 1px solid #999; border-radius: 3px; padding: 2px 8px; font-size: 9pt; }";

        // ── TEMPLATE: MODERN ──────────────────────────────────────────────────

        private string ModernCss() => @"
body {
  font-family: Arial, Helvetica, sans-serif;
  color: #212121; font-size: 10pt; margin: 0; padding: 0;
}
.resume  { max-width: 760px; margin: 0 auto; }
.rh      { background: #1e3a5f; color: #fff; padding: 26px 30px 20px; }
h1       { font-size: 22pt; margin: 0 0 3px; color: #fff; }
.tagline { color: #90b8d8; margin: 0 0 8px; font-size: 11pt; }
.contact-row { font-size: 9pt; color: #b0cce0; }
.st {
  font-size: 9pt; text-transform: uppercase; letter-spacing: 2px;
  border-left: 3px solid #1e3a5f; padding-left: 8px; margin: 20px 0 8px; color: #1e3a5f;
}
section  { padding: 0 30px; }
.entry   { margin-bottom: 13px; padding-left: 11px; }
.eh      { display: flex; justify-content: space-between; margin-bottom: 1px; }
.etitle  { font-weight: 700; font-size: 10pt; color: #1e3a5f; }
.edate   { font-size: 9pt; color: #777; background: #f0f4f8; padding: 1px 6px; border-radius: 3px; }
.esub    { font-size: 9pt; color: #555; display: block; margin-bottom: 3px; }
.ebody   { font-size: 9.5pt; line-height: 1.55; margin: 3px 0 0; }
.summary { font-size: 9.5pt; line-height: 1.65; margin: 0; padding: 0 30px; }
.skills  { display: flex; flex-wrap: wrap; gap: 7px; margin-top: 4px; }
.pill    { background: #1e3a5f; color: #fff; border-radius: 20px; padding: 3px 12px; font-size: 9pt; }";

        // ── TEMPLATE: MINIMAL ─────────────────────────────────────────────────

        private string MinimalCss() => @"
body {
  font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif;
  color: #1a1a1a; font-size: 10pt; margin: 0; padding: 0;
}
.resume  { max-width: 760px; margin: 0 auto; padding: 20px; }
.rh      { padding-bottom: 18px; margin-bottom: 6px; }
h1       { font-size: 26pt; font-weight: 300; letter-spacing: -0.5px; margin: 0 0 3px; }
.tagline { color: #999; font-weight: 300; margin: 0 0 5px; font-size: 11pt; }
.contact-row { font-size: 9pt; color: #aaa; }
.st {
  font-size: 7.5pt; text-transform: uppercase;
  letter-spacing: 3px; color: #bbb; margin: 22px 0 8px; border: none;
}
.entry  { border-bottom: 0.5px solid #eee; padding-bottom: 11px; margin-bottom: 11px; }
.eh     { display: flex; justify-content: space-between; margin-bottom: 1px; }
.etitle { font-weight: 600; font-size: 10pt; }
.edate  { font-size: 9pt; color: #999; }
.esub   { font-size: 9pt; color: #888; display: block; margin-bottom: 3px; }
.ebody  { font-size: 9.5pt; line-height: 1.6; color: #444; margin: 3px 0 0; }
.summary { font-size: 9.5pt; line-height: 1.7; color: #444; margin: 0; }
.skills { display: flex; flex-wrap: wrap; gap: 5px; margin-top: 4px; }
.pill   { border-bottom: 1px solid #ccc; padding: 0 0 2px; font-size: 9pt; margin-right: 8px; }";
    }
}

