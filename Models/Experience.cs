using SQLite;

namespace CVPilot.Models;

public class Experience
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int ResumeId { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string JobTitle { get; set; } = string.Empty;

    public string EmploymentType { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string StartMonth { get; set; } = string.Empty;

    public int StartYear { get; set; }

    public string EndMonth { get; set; } = string.Empty;

    public int EndYear { get; set; }

    public bool IsCurrentlyWorking { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    [Ignore]
    public string Duration =>
        IsCurrentlyWorking
            ? $"{StartMonth} {StartYear} - Present"
            : $"{StartMonth} {StartYear} - {EndMonth} {EndYear}";
}
