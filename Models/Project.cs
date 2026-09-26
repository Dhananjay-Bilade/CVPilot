using SQLite;

namespace CVPilot.Models;

public class Project
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int ResumeId { get; set; }

    public string ProjectName { get; set; } = string.Empty;

    public string Technologies { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string GithubLink { get; set; } = string.Empty;

    public string LiveLink { get; set; } = string.Empty;

    public string StartMonth { get; set; } = string.Empty;

    public int StartYear { get; set; }

    public string EndMonth { get; set; } = string.Empty;

    public int EndYear { get; set; }

    public bool IsOngoing { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    [Ignore]
    public string Duration =>
        IsOngoing
            ? $"{StartMonth} {StartYear} - Present"
            : $"{StartMonth} {StartYear} - {EndMonth} {EndYear}";
}
