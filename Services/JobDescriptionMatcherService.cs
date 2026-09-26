using System.Text.RegularExpressions;
using CVPilot.Models;

namespace CVPilot.Services;

public class JobDescriptionMatcherService
{
    private static readonly string[] ImportantKeywords =
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
        "tensorflow",
        "pytorch",
        "git",
        "github",
        "azure",
        "aws",
        "docker",
        "kubernetes",
        "api",
        "rest api",
        "rest",
        "sqlite",
        "android",
        "maui",
        "communication",
        "leadership",
        "teamwork",
        "problem solving",
        "agile",
        "scrum",
        "debugging",
        "testing",
        "software development",
        "web development",
        "mobile development",
        "database",
        "cloud",
        "linux"
    };

    public JobMatchResult Analyze(
        string resumeText,
        string jobDescription)
    {
        JobMatchResult result =
            new JobMatchResult();

        if (string.IsNullOrWhiteSpace(resumeText) ||
            string.IsNullOrWhiteSpace(jobDescription))
        {
            result.Recommendations.Add(
                "Provide both a resume and job description.");

            return result;
        }

        string normalizedResume =
            NormalizeText(resumeText);

        string normalizedJobDescription =
            NormalizeText(jobDescription);

        List<string> jobKeywords =
            ExtractKeywords(normalizedJobDescription);

        if (jobKeywords.Count == 0)
        {
            result.Recommendations.Add(
                "The job description does not contain enough recognizable keywords.");

            return result;
        }

        foreach (string keyword in jobKeywords)
        {
            if (ContainsKeyword(
                    normalizedResume,
                    keyword))
            {
                result.MatchedKeywords.Add(keyword);
            }
            else
            {
                result.MissingKeywords.Add(keyword);
            }
        }

        result.MatchedKeywords =
            result.MatchedKeywords
                .Distinct()
                .OrderBy(x => x)
                .ToList();

        result.MissingKeywords =
            result.MissingKeywords
                .Distinct()
                .OrderBy(x => x)
                .ToList();

        result.MatchPercentage =
            (int)Math.Round(
                (double)result.MatchedKeywords.Count
                / jobKeywords.Count
                * 100);

        if (result.MatchPercentage < 50)
        {
            result.Recommendations.Add(
                "Your resume has a low match with this job description.");
        }
        else if (result.MatchPercentage < 75)
        {
            result.Recommendations.Add(
                "Your resume has a moderate match. Consider adding relevant missing keywords.");
        }
        else
        {
            result.Recommendations.Add(
                "Your resume has a strong keyword match with this job description.");
        }

        foreach (string keyword in result.MissingKeywords.Take(8))
        {
            result.Recommendations.Add(
                $"Consider including \"{keyword}\" if you genuinely have this skill or experience.");
        }

        return result;
    }

    private List<string> ExtractKeywords(
        string jobDescription)
    {
        List<string> keywords = new();

        foreach (string keyword in ImportantKeywords)
        {
            if (ContainsKeyword(
                    jobDescription,
                    keyword))
            {
                keywords.Add(keyword);
            }
        }

        return keywords;
    }

    private bool ContainsKeyword(
        string text,
        string keyword)
    {
        if (keyword.Contains(" "))
        {
            return text.Contains(
                keyword,
                StringComparison.OrdinalIgnoreCase);
        }

        string pattern =
            $@"\b{Regex.Escape(keyword)}\b";

        return Regex.IsMatch(
            text,
            pattern,
            RegexOptions.IgnoreCase);
    }

    private string NormalizeText(
        string text)
    {
        return Regex.Replace(
                text.ToLowerInvariant(),
                @"\s+",
                " ")
            .Trim();
    }
}