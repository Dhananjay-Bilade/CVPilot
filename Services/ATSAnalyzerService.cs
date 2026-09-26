using CVPilot.Models;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace CVPilot.Services;

public class ATSAnalyzerService
{
    private static readonly string[] CommonSkills =
    {
        "python",
        "java",
        "c#",
        ".net",
        "dotnet",
        "sql",
        "mysql",
        "postgresql",
        "javascript",
        "typescript",
        "html",
        "css",
        "react",
        "angular",
        "node.js",
        "nodejs",
        "power bi",
        "tableau",
        "excel",
        "machine learning",
        "deep learning",
        "artificial intelligence",
        "data analysis",
        "data science",
        "pandas",
        "numpy",
        "scikit-learn",
        "git",
        "azure",
        "aws",
        "api",
        "rest",
        "rest api",
        "sqlite",
        "android",
        "maui"
    };

    public async Task<ATSAnalysisResult> AnalyzeAsync(
        FileResult file)
    {
        await using Stream stream =
            await file.OpenReadAsync();

        string extension =
            Path.GetExtension(file.FileName)
                .ToLowerInvariant();

        string text;

        if (extension == ".pdf")
        {
            text = ExtractPdfText(stream);
        }
        else if (extension == ".docx")
        {
            text = ExtractDocxText(stream);
        }
        else
        {
            throw new NotSupportedException(
                "Only PDF and DOCX files are supported.");
        }

        return AnalyzeText(text);
    }

    private string ExtractPdfText(Stream stream)
    {
        using PdfLoadedDocument document =
            new PdfLoadedDocument(stream);

        StringBuilder text =
            new StringBuilder();

        foreach (PdfLoadedPage page in document.Pages)
        {
            text.AppendLine(
                page.ExtractText(true));
        }

        return text.ToString();
    }

    private string ExtractDocxText(Stream stream)
    {
        using MemoryStream memoryStream =
            new MemoryStream();

        stream.CopyTo(memoryStream);

        memoryStream.Position = 0;

        using ZipArchive archive =
            new ZipArchive(
                memoryStream,
                ZipArchiveMode.Read);

        ZipArchiveEntry? documentEntry =
            archive.GetEntry("word/document.xml");

        if (documentEntry == null)
            return string.Empty;

        using Stream documentStream =
            documentEntry.Open();

        using StreamReader reader =
            new StreamReader(documentStream);

        string xml =
            reader.ReadToEnd();

        StringBuilder text =
            new StringBuilder();

        string withoutTags =
            Regex.Replace(
                xml,
                "<[^>]+>",
                " ");

        withoutTags =
            System.Net.WebUtility
                .HtmlDecode(withoutTags);

        text.Append(
            Regex.Replace(
                withoutTags,
                @"\s+",
                " "));

        return text.ToString();
    }

    private ATSAnalysisResult AnalyzeText(
        string text)
    {
        string normalizedText =
            text.ToLowerInvariant();

        ATSAnalysisResult result =
            new ATSAnalysisResult();

        if (string.IsNullOrWhiteSpace(text))
        {
            result.OverallScore = 0;

            result.Message =
                "No readable text was found in the resume.";

            result.Recommendations.Add(
                "Upload a text-based PDF or DOCX resume.");

            return result;
        }

        // CONTACT INFORMATION
        int contactScore = 0;

        bool hasEmail =
            Regex.IsMatch(
                normalizedText,
                @"[\w\.-]+@[\w\.-]+\.\w+");

        bool hasPhone =
            Regex.IsMatch(
                normalizedText,
                @"(\+?\d[\d\s\-\(\)]{8,}\d)");

        bool hasLinkedIn =
            normalizedText.Contains("linkedin");

        bool hasGithub =
            normalizedText.Contains("github");

        if (hasEmail)
            contactScore += 6;

        if (hasPhone)
            contactScore += 5;

        if (hasLinkedIn)
            contactScore += 2;

        if (hasGithub)
            contactScore += 2;

        result.ContactScore =
            Math.Min(contactScore, 15);

        if (!hasEmail)
        {
            result.Recommendations.Add(
                "Add a professional email address.");
        }

        if (!hasPhone)
        {
            result.Recommendations.Add(
                "Add a reachable phone number.");
        }


        // SECTIONS
        string[] sections =
        {
            "summary",
            "professional summary",
            "objective",
            "skills",
            "technical skills",
            "experience",
            "work experience",
            "education",
            "projects",
            "certifications",
            "achievements",
            "languages"
        };

        foreach (string section in sections)
        {
            if (normalizedText.Contains(section))
            {
                if (!result.FoundSections.Contains(section))
                    result.FoundSections.Add(section);
            }
        }

        int sectionCount =
            result.FoundSections.Count;

        result.SectionScore =
            Math.Min(
                20,
                sectionCount * 2);

        if (!normalizedText.Contains("skills") &&
            !normalizedText.Contains("technical skills"))
        {
            result.Recommendations.Add(
                "Add a clearly labeled Skills section.");
        }

        if (!normalizedText.Contains("experience"))
        {
            result.Recommendations.Add(
                "Add an Experience section if applicable.");
        }

        if (!normalizedText.Contains("education"))
        {
            result.Recommendations.Add(
                "Add an Education section.");
        }


        // SKILLS
        foreach (string skill in CommonSkills)
        {
            if (normalizedText.Contains(skill))
            {
                result.FoundSkills.Add(skill);
            }
        }

        result.FoundSkills =
            result.FoundSkills
                .Distinct()
                .OrderBy(x => x)
                .ToList();

        result.SkillsScore =
            Math.Min(
                20,
                result.FoundSkills.Count * 2);

        if (result.FoundSkills.Count < 5)
        {
            result.Recommendations.Add(
                "Add more relevant technical skills and tools.");
        }


        // EXPERIENCE
        bool hasExperience =
            normalizedText.Contains("experience") ||
            normalizedText.Contains("internship") ||
            normalizedText.Contains("developer") ||
            normalizedText.Contains("engineer");

        bool hasActionWords =
            normalizedText.Contains("developed") ||
            normalizedText.Contains("created") ||
            normalizedText.Contains("implemented") ||
            normalizedText.Contains("designed") ||
            normalizedText.Contains("managed") ||
            normalizedText.Contains("analyzed");

        int experienceScore = 0;

        if (hasExperience)
            experienceScore += 8;

        if (hasActionWords)
            experienceScore += 7;

        result.ExperienceScore =
            experienceScore;

        if (!hasExperience)
        {
            result.Recommendations.Add(
                "Add relevant work experience, internships, or practical projects.");
        }

        if (!hasActionWords)
        {
            result.Recommendations.Add(
                "Use strong action verbs such as Developed, Implemented, Designed, and Analyzed.");
        }


        // EDUCATION
        bool hasEducation =
            normalizedText.Contains("education") ||
            normalizedText.Contains("bachelor") ||
            normalizedText.Contains("master") ||
            normalizedText.Contains("degree") ||
            normalizedText.Contains("university") ||
            normalizedText.Contains("college");

        result.EducationScore =
            hasEducation ? 10 : 0;

        if (!hasEducation)
        {
            result.Recommendations.Add(
                "Add your education details clearly.");
        }


        // KEYWORDS
        int keywordScore = 0;

        string[] importantKeywords =
        {
            "develop",
            "developed",
            "software",
            "project",
            "technology",
            "database",
            "application",
            "analysis",
            "team",
            "problem solving"
        };

        foreach (string keyword in importantKeywords)
        {
            if (normalizedText.Contains(keyword))
                keywordScore++;
        }

        result.KeywordScore =
            Math.Min(
                10,
                keywordScore);

        if (keywordScore < 5)
        {
            result.Recommendations.Add(
                "Use more keywords relevant to your target job role.");
        }


        // BASIC FORMATTING / ATS READABILITY
        int formattingScore = 0;

        if (text.Length >= 1000)
            formattingScore += 4;

        if (text.Length >= 2000)
            formattingScore += 2;

        if (result.FoundSections.Count >= 5)
            formattingScore += 2;

        if (!normalizedText.Contains("table"))
            formattingScore += 2;

        result.FormattingScore =
            Math.Min(
                10,
                formattingScore);

        if (text.Length < 1000)
        {
            result.Recommendations.Add(
                "Your resume appears too short. Add relevant details, projects, or achievements.");
        }


        // FINAL SCORE
        result.OverallScore =
            result.ContactScore +
            result.SectionScore +
            result.SkillsScore +
            result.ExperienceScore +
            result.EducationScore +
            result.KeywordScore +
            result.FormattingScore;

        result.OverallScore =
            Math.Clamp(
                result.OverallScore,
                0,
                100);


        // MESSAGE
        if (result.OverallScore >= 80)
        {
            result.Message =
                "Excellent ATS readiness. Your resume has a strong structure and good keyword coverage.";
        }
        else if (result.OverallScore >= 65)
        {
            result.Message =
                "Good ATS readiness, but there are some areas you can improve.";
        }
        else if (result.OverallScore >= 50)
        {
            result.Message =
                "Your resume has a reasonable foundation but needs improvement for better ATS compatibility.";
        }
        else
        {
            result.Message =
                "Your resume needs significant improvement before applying through ATS systems.";
        }

        return result;
    }
}
