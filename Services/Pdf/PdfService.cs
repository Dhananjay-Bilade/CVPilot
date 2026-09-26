using CVPilot.Models;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
namespace CVPilot.Services.Pdf;
#if ANDROID
using CVPilot.Platforms.Android.Services;
#endif
public class PdfService
{
    public async Task<string> CreateTestPdfAsync()
    {
        return await Task.Run(() =>
        {
            // 1. Create PDF document
            using PdfDocument document = new PdfDocument();

            // 2. Add A4 page
            PdfPage page = document.Pages.Add();

            // 3. Get graphics object
            PdfGraphics graphics = page.Graphics;

            // 4. Create font
            PdfFont titleFont =
                new PdfStandardFont(
                    PdfFontFamily.Helvetica,
                    20,
                    PdfFontStyle.Bold);

            PdfFont normalFont =
                new PdfStandardFont(
                    PdfFontFamily.Helvetica,
                    11);

            // 5. Create brushes
            PdfBrush blackBrush =
                new PdfSolidBrush(Syncfusion.Drawing.Color.Black);

            PdfBrush blueBrush =
                new PdfSolidBrush(Syncfusion.Drawing.Color.DarkBlue);

            // 6. Draw title
            graphics.DrawString(
                "CVPilot PDF Engine Test",
                titleFont,
                blueBrush,
                new Syncfusion.Drawing.PointF(40, 50));

            // 7. Draw normal text
            graphics.DrawString(
                "Syncfusion PDF generation is working.",
                normalFont,
                blackBrush,
                new Syncfusion.Drawing.PointF(40, 100));

            graphics.DrawString(
                "This PDF was generated directly on Android.",
                normalFont,
                blackBrush,
                new Syncfusion.Drawing.PointF(40, 125));

            graphics.DrawString(
                "CVPilot PDF Engine - Test Successful",
                normalFont,
                blackBrush,
                new Syncfusion.Drawing.PointF(40, 150));

            // 8. Create output folder
            string folder =
                Path.Combine(
                    FileSystem.AppDataDirectory,
                    "Resumes");

            Directory.CreateDirectory(folder);

            // 9. PDF file path
            string pdfPath =
                Path.Combine(
                    folder,
                    "CVPilot-Test.pdf");

            // 10. Save PDF
            using FileStream stream =
                File.Create(pdfPath);

            document.Save(stream);

            return pdfPath;
        });
    }

    public async Task<string> ExportResumeAsync(CompleteResume resume)
    {
        using PdfDocument document = new PdfDocument();

        // =====================================================
        // PAGE SETTINGS
        // =====================================================

        document.PageSettings.Size = PdfPageSize.A4;

        const float topMargin = 50.4f;       // 0.7 inch
        const float bottomMargin = 50.4f;    // 0.7 inch
        const float leftMargin = 46.8f;      // 0.65 inch
        const float rightMargin = 46.8f;     // 0.65 inch

        PdfPage page = document.Pages.Add();
        PdfGraphics graphics = page.Graphics;

        // =====================================================
        // CONTENT AREA
        // =====================================================

        float x = leftMargin;
        float y = topMargin;

        float contentWidth =
            page.GetClientSize().Width
            - leftMargin
            - rightMargin;

        // =====================================================
        // FONTS
        // =====================================================

        PdfFont nameFont =
            new PdfStandardFont(
                PdfFontFamily.Helvetica,
                22,
                PdfFontStyle.Bold);

        PdfFont titleFont =
            new PdfStandardFont(
                PdfFontFamily.Helvetica,
                12,
                PdfFontStyle.Italic);

        PdfFont normalFont =
            new PdfStandardFont(
                PdfFontFamily.Helvetica,
                9);

        PdfFont linkFont =
            new PdfStandardFont(
                PdfFontFamily.Helvetica,
                9);

        // =====================================================
        // BRUSHES
        // =====================================================

        PdfBrush blackBrush =
            new PdfSolidBrush(
                Syncfusion.Drawing.Color.Black);

        PdfBrush blueBrush =
            new PdfSolidBrush(
                Syncfusion.Drawing.Color.DarkBlue);

        PersonalDetails? person = resume.PersonalDetails;

            if (person == null)
                throw new Exception("Personal details not found.");

            // =========================
            // NAME
            // =========================

            graphics.DrawString(
                person.FullName ?? string.Empty,
                nameFont,
                blackBrush,
                new Syncfusion.Drawing.PointF(x, y));

            y += 26;

            // =========================
            // CONTACT INFORMATION
            // =========================

            string contact = BuildContactLine(person); // declare exception above 

            graphics.DrawString(
                contact,
                normalFont,
                blackBrush,
                new Syncfusion.Drawing.PointF(x, y));

            y += 12;

            // =========================
            // CLICKABLE LINKS
            // =========================

            float linkX = x;

            if (!string.IsNullOrWhiteSpace(person.LinkedInUrl))
            {
                graphics.DrawString(
                    "LinkedIn: ",
                    normalFont,
                    blackBrush,
                    new Syncfusion.Drawing.PointF(x, y));

                    float labelWidth =
                    normalFont.MeasureString("LinkedIn: ").Width;

                AddHyperlink(
                    page,
                    graphics,
                    person.LinkedInUrl,
                    person.LinkedInUrl,
                    x + labelWidth,
                    y,
                    normalFont,
                    blueBrush);

                y += 12;
            }

            if (!string.IsNullOrWhiteSpace(person.GitHubUrl))
            {
                graphics.DrawString(
                    "GitHub: ",
                    normalFont,
                    blackBrush,
                    new Syncfusion.Drawing.PointF(x, y));

                float labelWidth =
                    normalFont.MeasureString("GitHub: ").Width;

                AddHyperlink(
                    page,
                    graphics,
                    person.GitHubUrl,
                    person.GitHubUrl,
                    x + labelWidth,
                    y,
                    normalFont,
                    blueBrush);

                y += 12;
            }

            if (!string.IsNullOrWhiteSpace(person.PortfolioUrl))
            {
                graphics.DrawString(
                    "Portfolio: ",
                    normalFont,
                    blackBrush,
                    new Syncfusion.Drawing.PointF(x, y));

                float labelWidth =
                    normalFont.MeasureString("Portfolio: ").Width;

                AddHyperlink(
                    page,
                    graphics,
                    person.PortfolioUrl,
                    person.PortfolioUrl,
                    x + labelWidth,
                    y,
                    normalFont,
                    blueBrush);

                y += 12;
            }

            y += 10;

            // =========================
            // HEADER LINE
            // =========================

            graphics.DrawLine(
                new PdfPen(Syncfusion.Drawing.Color.Black, 1),
                new Syncfusion.Drawing.PointF(x, y),
                new Syncfusion.Drawing.PointF(x + contentWidth, y));

            // =========================
            // PROFESSIONAL SUMMARY
            // =========================

            if (resume.Objective != null &&
                !string.IsNullOrWhiteSpace(resume.Objective.ObjectiveText))
            {
                y += 12;

                PdfFont sectionFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        11,
                        PdfFontStyle.Bold);

                graphics.DrawString(
                    "PROFESSIONAL SUMMARY",
                    sectionFont,
                    blackBrush,
                    new Syncfusion.Drawing.PointF(x, y));

                y += 14;

                graphics.DrawLine(
                    new PdfPen(Syncfusion.Drawing.Color.Black, 1),
                    new Syncfusion.Drawing.PointF(x, y),
                    new Syncfusion.Drawing.PointF(
                        x + contentWidth,
                        y));

                y += 8;

                PdfFont bodyFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        9);

                PdfTextElement summaryText =
                    new PdfTextElement(
                        resume.Objective.ObjectiveText,
                        bodyFont,
                        blackBrush);

                PdfLayoutFormat layoutFormat =
                    new PdfLayoutFormat
                    {
                        Layout = PdfLayoutType.Paginate,
                        Break = PdfLayoutBreakType.FitPage
                    };

                PdfLayoutResult Layoutresult =
                    summaryText.Draw(
                        page,
                        new Syncfusion.Drawing.RectangleF(
                        x,
                        y,
                        contentWidth,
                        100),
                        layoutFormat);

                y = Layoutresult.Bounds.Bottom + 8;
            }

            // =========================
            // TECHNICAL SKILLS
            // =========================

            if (resume.Skills != null && resume.Skills.Any())
            {
                y += 6;

                PdfFont sectionFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        11,
                        PdfFontStyle.Bold);

                graphics.DrawString(
                    "TECHNICAL SKILLS",
                    sectionFont,
                    blackBrush,
                    new Syncfusion.Drawing.PointF(x, y));

                y += 14;

                // Section underline
                graphics.DrawLine(
                    new PdfPen(Syncfusion.Drawing.Color.Black, 1),
                    new Syncfusion.Drawing.PointF(x, y),
                    new Syncfusion.Drawing.PointF(
                        x + contentWidth,
                        y));

                y += 8;

                PdfFont skillFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        9);

                PdfFont skillCategoryFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        9,
                        PdfFontStyle.Bold);

                // Same grouping logic as ResumePreviewPage
                var groupedSkills =
                    resume.Skills
                        .GroupBy(s =>
                            string.IsNullOrWhiteSpace(s.SkillCategory)
                                ? "Other"
                                : s.SkillCategory);

                // Left side = category
                // Right side = skills
                float categoryX = x;
                float skillsX = x + 100;

                foreach (var group in groupedSkills)
                {
                    string skills =
                        string.Join(
                            " | ",
                            group.Select(s => s.SkillName));

                    // Category
                    graphics.DrawString(
                        group.Key + " :",
                        skillCategoryFont,
                        blackBrush,
                        new Syncfusion.Drawing.PointF(
                            categoryX,
                            y));

                    // Skills
                    graphics.DrawString(
                        skills,
                        skillFont,
                        blackBrush,
                        new Syncfusion.Drawing.PointF(
                            skillsX,
                            y));

                    y += 14;
                }

                y += 3;

                // Bottom divider
                graphics.DrawLine(
                    new PdfPen(Syncfusion.Drawing.Color.LightGray, 0.5f),
                    new Syncfusion.Drawing.PointF(x, y),
                    new Syncfusion.Drawing.PointF(
                        x + contentWidth,
                        y));

                y += 6;
            }
            // =========================
            // EXPERIENCE
            // =========================

            if (resume.Experiences != null &&
                resume.Experiences.Any())
            {
                EnsurePageSpace(
                    document,
                    ref page,
                    ref graphics,
                    ref y,
                    60);

                y += 6;

                PdfFont sectionFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        11,
                        PdfFontStyle.Bold);

                graphics.DrawString(
                    "EXPERIENCE",
                    sectionFont,
                    blackBrush,
                    new Syncfusion.Drawing.PointF(x, y));

                y += 14;

                // Section line
                graphics.DrawLine(
                    new PdfPen(Syncfusion.Drawing.Color.Black, 1),
                    new Syncfusion.Drawing.PointF(x, y),
                    new Syncfusion.Drawing.PointF(
                        x + contentWidth,
                        y));

                y += 8;

                PdfFont experienceTitleFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        10,
                        PdfFontStyle.Bold);

                PdfFont experienceNormalFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        9);

                PdfFont experienceItalicFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        9,
                        PdfFontStyle.Italic);

                foreach (var experience in resume.Experiences)
                {
                    // =========================
                    // JOB TITLE
                    // =========================

                    graphics.DrawString(
                        experience.JobTitle ?? "",
                        experienceTitleFont,
                        blackBrush,
                        new Syncfusion.Drawing.PointF(x, y));

                    // =========================
                    // DURATION - RIGHT SIDE
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            experience.Duration))
                    {
                        float durationX =
    page.GetClientSize().Width - rightMargin - 70;

                        graphics.DrawString(
                            experience.Duration,
                            experienceNormalFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                durationX,
                                y));
                    }

                    y += 14;

                    // =========================
                    // COMPANY
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            experience.CompanyName))
                    {
                        graphics.DrawString(
                            experience.CompanyName,
                            experienceNormalFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                x,
                                y));

                        y += 13;
                    }

                    // =========================
                    // LOCATION
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            experience.Location))
                    {
                        graphics.DrawString(
                            experience.Location,
                            experienceItalicFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                x,
                                y));

                        y += 13;
                    }

                    // =========================
                    // DESCRIPTION
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            experience.Description))
                    {
                        PdfTextElement descriptionText =
                            new PdfTextElement(
                                "• " + experience.Description,
                                experienceNormalFont,
                                blackBrush);

                        PdfLayoutFormat layoutFormat =
                            new PdfLayoutFormat
                            {
                                Layout = PdfLayoutType.Paginate,
                                Break = PdfLayoutBreakType.FitPage
                            };

                        PdfLayoutResult Layoutresult =
                            descriptionText.Draw(
                                page,
                                new Syncfusion.Drawing.RectangleF(
                                x + 10,
                                y,
                                contentWidth - 10,
                                80),
                                layoutFormat);

                        y = Layoutresult.Bounds.Bottom + 6;
                    }

                    y += 5;
                }

                // Bottom divider
                graphics.DrawLine(
                    new PdfPen(Syncfusion.Drawing.Color.LightGray, 0.5f),
                    new Syncfusion.Drawing.PointF(x, y),
                    new Syncfusion.Drawing.PointF(
                        x + contentWidth,
                        y));

                y += 10;
            }
            // =========================
            // PROJECTS
            // =========================

            if (resume.Projects != null &&
                resume.Projects.Any())
            {
                EnsurePageSpace(
                    document,
                    ref page,
                    ref graphics,
                    ref y,
                    60);

                y += 6;

                PdfFont projectSectionFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        11,
                        PdfFontStyle.Bold);

                graphics.DrawString(
                    "PROJECTS",
                    projectSectionFont,
                    blackBrush,
                    new Syncfusion.Drawing.PointF(x, y));

                y += 14;

                // Section line
                graphics.DrawLine(
                    new PdfPen(Syncfusion.Drawing.Color.Black, 1),
                    new Syncfusion.Drawing.PointF(x, y),
                    new Syncfusion.Drawing.PointF(
                        x + contentWidth,
                        y));

                y += 8;

                PdfFont projectTitleFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        10,
                        PdfFontStyle.Bold);

                PdfFont projectNormalFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        9);

                foreach (var project in resume.Projects)
                {
                    EnsurePageSpace(
                        document,
                        ref page,
                        ref graphics,
                        ref y,
                        65);
                    // =========================
                    // PROJECT NAME
                    // =========================

                    graphics.DrawString(
                        project.ProjectName ?? "",
                        projectTitleFont,
                        blackBrush,
                        new Syncfusion.Drawing.PointF(
                            x,
                            y));

                    // =========================
                    // DURATION - RIGHT
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            project.Duration))
                    {
                        float durationX =
    page.GetClientSize().Width - rightMargin - 70;

                        graphics.DrawString(
                            project.Duration,
                            projectNormalFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                durationX,
                                y));
                    }

                    y += 14;

                    // =========================
                    // TECHNOLOGIES
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            project.Technologies))
                    {
                        graphics.DrawString(
                            project.Technologies,
                            projectNormalFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                x,
                                y));

                        y += 13;
                    }

                    // =========================
                    // DESCRIPTION
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            project.Description))
                    {
                        PdfTextElement projectDescription =
                            new PdfTextElement(
                                "• " + project.Description,
                                projectNormalFont,
                                blackBrush);

                        PdfLayoutFormat projectLayoutFormat =
                            new PdfLayoutFormat
                            {
                                Layout = PdfLayoutType.Paginate,
                                Break = PdfLayoutBreakType.FitPage
                            };

                        PdfLayoutResult projectResult =
                            projectDescription.Draw(
                                page,
                                new Syncfusion.Drawing.RectangleF(
                                x + 10,
                                y,
                                contentWidth - 10,
                                80),
                                projectLayoutFormat);

                        y = projectResult.Bounds.Bottom + 10;
                    }

                    y += 5;
                }

                // Bottom divider
                graphics.DrawLine(
                    new PdfPen(Syncfusion.Drawing.Color.LightGray, 0.5f),
                    new Syncfusion.Drawing.PointF(x, y),
                    new Syncfusion.Drawing.PointF(
                        x + contentWidth,
                        y));

                y += 10;
            }
            // =========================
            // EDUCATION
            // =========================

            if (resume.Educations != null &&
                resume.Educations.Any())
            {
                EnsurePageSpace(
                    document,
                    ref page,
                    ref graphics,
                    ref y,
                    60);
                y += 6;

                PdfFont educationSectionFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        11,
                        PdfFontStyle.Bold);

                graphics.DrawString(
                    "EDUCATION",
                    educationSectionFont,
                    blackBrush,
                    new Syncfusion.Drawing.PointF(x, y));

                y += 14;

                // Section line
                graphics.DrawLine(
                    new PdfPen(Syncfusion.Drawing.Color.Black, 1),
                    new Syncfusion.Drawing.PointF(x, y),
                    new Syncfusion.Drawing.PointF(
                        x + contentWidth,
                        y));

                y += 8;

                PdfFont educationTitleFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        10,
                        PdfFontStyle.Bold);

                PdfFont educationNormalFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        9);

                PdfFont educationItalicFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        9,
                        PdfFontStyle.Italic);

                foreach (var education in resume.Educations)
                {
                    EnsurePageSpace(
                        document,
                        ref page,
                        ref graphics,
                        ref y,
                        65);
                    // =========================
                    // DEGREE
                    // =========================

                    graphics.DrawString(
                        education.Degree ?? "",
                        educationTitleFont,
                        blackBrush,
                        new Syncfusion.Drawing.PointF(
                            x,
                            y));

                    // =========================
                    // DURATION - RIGHT SIDE
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            education.Duration))
                    {
                    float educationDurationX =
                        page.GetClientSize().Width
                        - rightMargin
                        - 70;

                    graphics.DrawString(
                            education.Duration,
                            educationNormalFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                educationDurationX,
                                y));
                    }

                    y += 14;

                    // =========================
                    // INSTITUTE
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            education.Institute))
                    {
                        graphics.DrawString(
                            education.Institute,
                            educationNormalFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                x,
                                y));

                        y += 13;
                    }

                    // =========================
                    // UNIVERSITY
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            education.University))
                    {
                        graphics.DrawString(
                            education.University,
                            educationNormalFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                x,
                                y));

                        y += 13;
                    }

                    // =========================
                    // FIELD OF STUDY
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            education.FieldOfStudy))
                    {
                        graphics.DrawString(
                            education.FieldOfStudy,
                            educationItalicFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                x,
                                y));

                        y += 13;
                    }

                    // =========================
                    // GRADE
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            education.Grade))
                    {
                        graphics.DrawString(
                            "Grade: " + education.Grade,
                            educationNormalFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                x,
                                y));

                        y += 13;
                    }

                    // =========================
                    // DESCRIPTION
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            education.Description))
                    {
                        PdfTextElement educationDescription =
                            new PdfTextElement(
                                education.Description,
                                educationNormalFont,
                                blackBrush);

                        PdfLayoutFormat educationLayoutFormat =
                            new PdfLayoutFormat
                            {
                                Layout = PdfLayoutType.Paginate,
                                Break = PdfLayoutBreakType.FitPage
                            };

                        PdfLayoutResult educationResult =
                            educationDescription.Draw(
                                page,
                                new Syncfusion.Drawing.RectangleF(
                                x,
                                y,
                                contentWidth,
                                80),
                                educationLayoutFormat);

                        y = educationResult.Bounds.Bottom + 6;
                    }

                    y += 5;
                }

                // Bottom divider
                graphics.DrawLine(
                    new PdfPen(Syncfusion.Drawing.Color.LightGray, 0.5f),
                    new Syncfusion.Drawing.PointF(x, y),
                    new Syncfusion.Drawing.PointF(
                        x + contentWidth,
                        y));

                y += 10;
            }
            // =========================
            // CERTIFICATIONS
            // =========================

            if (resume.Certifications != null &&
                resume.Certifications.Any())
            {
                EnsurePageSpace(
                    document,
                    ref page,
                    ref graphics,
                    ref y,
                    60);
                y += 6;

                PdfFont certificationSectionFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        11,
                        PdfFontStyle.Bold);

                graphics.DrawString(
                    "CERTIFICATIONS",
                    certificationSectionFont,
                    blackBrush,
                    new Syncfusion.Drawing.PointF(x, y));

                y += 14;

                // Section line
                graphics.DrawLine(
                    new PdfPen(Syncfusion.Drawing.Color.Black, 1),
                    new Syncfusion.Drawing.PointF(x, y),
                    new Syncfusion.Drawing.PointF(
                        x + contentWidth,
                        y));

                y += 8;

                PdfFont certificationTitleFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        10,
                        PdfFontStyle.Bold);

                PdfFont certificationNormalFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        9);

                foreach (var certification in resume.Certifications)
                {
                    // =========================
                    // CERTIFICATION NAME
                    // =========================

                    graphics.DrawString(
                        certification.CertificationName ?? "",
                        certificationTitleFont,
                        blackBrush,
                        new Syncfusion.Drawing.PointF(
                            x,
                            y));

                    y += 14;

                    // =========================
                    // ISSUING ORGANIZATION
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            certification.IssuingOrganization))
                    {
                        graphics.DrawString(
                            certification.IssuingOrganization,
                            certificationNormalFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                x,
                                y));

                        y += 13;
                    }

                    // =========================
                    // EXPIRY
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            certification.ExpiryText))
                    {
                        graphics.DrawString(
                            "Expiry: " + certification.ExpiryText,
                            certificationNormalFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                x,
                                y));

                        y += 13;
                    }

                    // =========================
                    // CREDENTIAL ID
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            certification.CredentialId))
                    {
                        graphics.DrawString(
                            "Credential ID: " +
                            certification.CredentialId,
                            certificationNormalFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                x,
                                y));

                        y += 13;
                    }

                    y += 5;
                }

                // Bottom divider
                graphics.DrawLine(
                    new PdfPen(Syncfusion.Drawing.Color.LightGray, 0.5f),
                    new Syncfusion.Drawing.PointF(x, y),
                    new Syncfusion.Drawing.PointF(
                        x + contentWidth,
                        y));

                y += 10;
            }
            // =========================
            // ACHIEVEMENTS
            // =========================

            if (resume.Achievements != null &&
                resume.Achievements.Any())
            {
                EnsurePageSpace(
                    document,
                    ref page,
                    ref graphics,
                    ref y,
                    60);
                y += 6;

                PdfFont achievementSectionFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        11,
                        PdfFontStyle.Bold);

                graphics.DrawString(
                    "ACHIEVEMENTS",
                    achievementSectionFont,
                    blackBrush,
                    new Syncfusion.Drawing.PointF(x, y));

                y += 14;

                // Section line
                graphics.DrawLine(
                    new PdfPen(Syncfusion.Drawing.Color.Black, 1),
                    new Syncfusion.Drawing.PointF(x, y),
                    new Syncfusion.Drawing.PointF(
                        x + contentWidth,
                        y));

                y += 8;

                PdfFont achievementTitleFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        10,
                        PdfFontStyle.Bold);

                PdfFont achievementNormalFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        9);

                foreach (var achievement in resume.Achievements)
                {
                    // =========================
                    // ACHIEVEMENT TITLE
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            achievement.Title))
                    {
                        graphics.DrawString(
                            "• " + achievement.Title,
                            achievementTitleFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                x,
                                y));

                        y += 14;
                    }

                    // =========================
                    // DESCRIPTION
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            achievement.Description))
                    {
                        PdfTextElement achievementDescription =
                            new PdfTextElement(
                                achievement.Description,
                                achievementNormalFont,
                                blackBrush);

                        PdfLayoutFormat achievementLayoutFormat =
                            new PdfLayoutFormat
                            {
                                Layout = PdfLayoutType.Paginate,
                                Break = PdfLayoutBreakType.FitPage
                            };

                        PdfLayoutResult achievementResult =
                            achievementDescription.Draw(
                                page,
                                new Syncfusion.Drawing.RectangleF(
                                    x + 15,
                                    y,
                                    contentWidth - 15,
                                    80),
                                achievementLayoutFormat);

                        y = achievementResult.Bounds.Bottom + 6;
                    }
                }

                // Bottom divider
                graphics.DrawLine(
                    new PdfPen(Syncfusion.Drawing.Color.LightGray, 0.5f),
                    new Syncfusion.Drawing.PointF(x, y),
                    new Syncfusion.Drawing.PointF(
                        x + contentWidth,
                        y));

                y += 10;
            }
            // =========================
            // LANGUAGES
            // =========================

            if (resume.Languages != null &&
                resume.Languages.Any())
            {
                EnsurePageSpace(
                    document,
                    ref page,
                    ref graphics,
                    ref y,
                    50);
                y += 6;

                PdfFont languageSectionFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        11,
                        PdfFontStyle.Bold);

                graphics.DrawString(
                    "LANGUAGES",
                    languageSectionFont,
                    blackBrush,
                    new Syncfusion.Drawing.PointF(x, y));

                y += 14;

                // Section line
                graphics.DrawLine(
                    new PdfPen(Syncfusion.Drawing.Color.Black, 1),
                    new Syncfusion.Drawing.PointF(x, y),
                    new Syncfusion.Drawing.PointF(
                        x + contentWidth,
                        y));

                y += 8;

                PdfFont languageNameFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        9,
                        PdfFontStyle.Bold);

                PdfFont languageNormalFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        9);

                foreach (var language in resume.Languages)
                {
                    // =========================
                    // LANGUAGE NAME
                    // =========================

                    graphics.DrawString(
                        language.LanguageName ?? "",
                        languageNameFont,
                        blackBrush,
                        new Syncfusion.Drawing.PointF(
                            x,
                            y));

                    // =========================
                    // PROFICIENCY
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            language.Proficiency))
                    {
                        graphics.DrawString(
                            language.Proficiency,
                            languageNormalFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                x + 100,
                                y));
                    }

                    // =========================
                    // READ / WRITE / SPEAK
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            language.SkillsSummary))
                    {
                        graphics.DrawString(
                            language.SkillsSummary,
                            languageNormalFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                x + 210,
                                y));
                    }

                    y += 14;
                }

                // Bottom divider
                graphics.DrawLine(
                    new PdfPen(Syncfusion.Drawing.Color.LightGray, 0.5f),
                    new Syncfusion.Drawing.PointF(x, y),
                    new Syncfusion.Drawing.PointF(
                        x + contentWidth,
                        y));

                y += 10;
            }
            // =========================
            // REFERENCES
            // =========================

            if (resume.References != null &&
                resume.References.Any())
            {
                EnsurePageSpace(
                    document,
                    ref page,
                    ref graphics,
                    ref y,
                    70);
                y += 6;

                PdfFont referenceSectionFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        11,
                        PdfFontStyle.Bold);

                graphics.DrawString(
                    "REFERENCES",
                    referenceSectionFont,
                    blackBrush,
                    new Syncfusion.Drawing.PointF(x, y));

                y += 14;

                // Section line
                graphics.DrawLine(
                    new PdfPen(Syncfusion.Drawing.Color.Black, 1),
                    new Syncfusion.Drawing.PointF(x, y),
                    new Syncfusion.Drawing.PointF(
                        x + contentWidth,
                        y));

                y += 8;

                PdfFont referenceNameFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        10,
                        PdfFontStyle.Bold);

                PdfFont referenceNormalFont =
                    new PdfStandardFont(
                        PdfFontFamily.Helvetica,
                        9);

                foreach (var reference in resume.References)
                {
                    // =========================
                    // NAME
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            reference.FullName))
                    {
                        graphics.DrawString(
                            reference.FullName,
                            referenceNameFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                x,
                                y));

                        y += 14;
                    }

                    // =========================
                    // DESIGNATION
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            reference.Designation))
                    {
                        graphics.DrawString(
                            reference.Designation,
                            referenceNormalFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                x,
                                y));

                        y += 13;
                    }

                    // =========================
                    // COMPANY
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            reference.CompanyName))
                    {
                        graphics.DrawString(
                            reference.CompanyName,
                            referenceNormalFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                x,
                                y));

                        y += 13;
                    }

                    // =========================
                    // EMAIL
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            reference.Email))
                    {
                        graphics.DrawString(
                            reference.Email,
                            referenceNormalFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                x,
                                y));

                        y += 13;
                    }

                    // =========================
                    // PHONE
                    // =========================

                    if (!string.IsNullOrWhiteSpace(
                            reference.PhoneNumber))
                    {
                        graphics.DrawString(
                            reference.PhoneNumber,
                            referenceNormalFont,
                            blackBrush,
                            new Syncfusion.Drawing.PointF(
                                x,
                                y));

                        y += 13;
                    }

                    y += 5;
                }

                // Bottom divider
                graphics.DrawLine(
                    new PdfPen(Syncfusion.Drawing.Color.LightGray, 0.5f),
                    new Syncfusion.Drawing.PointF(x, y),
                    new Syncfusion.Drawing.PointF(
                        x + contentWidth,
                        y));

                y += 10;
            }

        // =============================
        // SAVE / EXPORT PDF
        // =============================

        string resumeName =
            string.IsNullOrWhiteSpace(resume.Resume?.ResumeName)
                ? "CVPilot-Resume"
                : resume.Resume.ResumeName.Trim();

        resumeName = SanitizeFileName(resumeName);

        if (string.IsNullOrWhiteSpace(resumeName))
        {
            resumeName = "CVPilot-Resume";
        }

        string fileName = $"{resumeName}.pdf";

        // -------------------------------------
        // CREATE PDF IN MEMORY
        // -------------------------------------

        byte[] pdfBytes;

        try
        {
            using MemoryStream memoryStream = new MemoryStream();

            document.Save(memoryStream);

            pdfBytes = memoryStream.ToArray();
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"PDF generation failed: {ex.Message}",
                ex);
        }

        // Make sure PDF actually contains data
        if (pdfBytes.Length == 0)
        {
            throw new Exception(
                "PDF generation failed because the generated PDF is empty.");
        }

#if ANDROID

        // -------------------------------------
        // OPEN ANDROID FILE PICKER
        // -------------------------------------

        try
        {
            string? savedUri =
                await AndroidPdfSaver.SaveAsync(
                    fileName,
                    pdfBytes);

            // User cancelled the file picker
            if (string.IsNullOrWhiteSpace(savedUri))
            {
                return string.Empty;
            }

            return savedUri;
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"PDF could not be saved: {ex.Message}",
                ex);
        }

#else

throw new PlatformNotSupportedException(
    "PDF file saving is currently implemented for Android.");

#endif
    }

    private static string SanitizeFileName(string fileName)
    {
        foreach (char invalidChar in Path.GetInvalidFileNameChars())
        {
            fileName = fileName.Replace(invalidChar, '_');
        }

        return fileName.Trim();
    }
    private string BuildContactLine(PersonalDetails person)
    {
        List<string> parts = new();

        if (!string.IsNullOrWhiteSpace(person.City))
            parts.Add(person.City);

        if (!string.IsNullOrWhiteSpace(person.State))
            parts.Add(person.State);

        if (!string.IsNullOrWhiteSpace(person.Country))
            parts.Add(person.Country);

        string location = string.Join(", ", parts);

        List<string> contact = new();

        if (!string.IsNullOrWhiteSpace(location))
            contact.Add(location);

        if (!string.IsNullOrWhiteSpace(person.PhoneNumber))
            contact.Add(person.PhoneNumber);

        if (!string.IsNullOrWhiteSpace(person.Email))
            contact.Add(person.Email);

        return string.Join("  |  ", contact);
    }

    private void AddHyperlink(
    PdfPage page,
    PdfGraphics graphics,
    string text,
    string url,
    float x,
    float y,
    PdfFont font,
    PdfBrush brush)
    {
        graphics.DrawString(
            text,
            font,
            brush,
            new Syncfusion.Drawing.PointF(x, y));

        Syncfusion.Drawing.SizeF textSize =
    font.MeasureString(text);

        RectangleF bounds =
            new RectangleF(
                x,
                y,
                textSize.Width,
                textSize.Height);

        PdfUriAnnotation annotation =
            new PdfUriAnnotation(bounds, url);

        page.Annotations.Add(annotation);
    }
    private bool EnsurePageSpace(
    PdfDocument document,
    ref PdfPage page,
    ref PdfGraphics graphics,
    ref float y,
    float requiredHeight)
    {
        // =====================================================
        // A4 PAGE MARGINS
        // =====================================================

        const float topMargin = 50.4f;       // 0.7 inch
        const float bottomMargin = 50.4f;    // 0.7 inch

        float pageHeight =
            page.GetClientSize().Height;

        // Check whether enough space is available
        if (y + requiredHeight <=
            pageHeight - bottomMargin)
        {
            return false;
        }

        // =====================================================
        // CREATE NEW A4 PAGE
        // =====================================================

        page = document.Pages.Add();

        graphics = page.Graphics;

        // Start new page from the same top margin
        y = topMargin;

        return true;
    }

}
