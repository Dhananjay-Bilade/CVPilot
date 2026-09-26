namespace CVPilot.Models;

public class JobMatchResult
{
    public int MatchPercentage { get; set; }

    public List<string> MatchedKeywords { get; set; } = new();

    public List<string> MissingKeywords { get; set; } = new();

    public List<string> Recommendations { get; set; } = new();
}