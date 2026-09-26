namespace CVPilot.Models;

public class ATSAnalysisResult
{
    public int OverallScore { get; set; }

    public int ContactScore { get; set; }

    public int SectionScore { get; set; }

    public int SkillsScore { get; set; }

    public int ExperienceScore { get; set; }

    public int EducationScore { get; set; }

    public int KeywordScore { get; set; }

    public int FormattingScore { get; set; }

    public List<string> FoundSections { get; set; } = new();

    public List<string> FoundSkills { get; set; } = new();

    public List<string> Recommendations { get; set; } = new();

    public string Message { get; set; } = string.Empty;
}