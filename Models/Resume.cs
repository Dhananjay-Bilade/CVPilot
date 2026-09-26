namespace CVPilot.Models;
using SQLite;

public class Resume
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // Basic Resume Information
    public string ResumeName { get; set; } = string.Empty;

    public string ResumeCategory { get; set; } = string.Empty;

    // Template Information
    public string TemplateName { get; set; } = "Classic";

    // Progress Tracking
    public int CompletionPercentage { get; set; } = 0;

    // Status
    public bool IsCompleted { get; set; } = false;

    // Dates
    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public DateTime UpdatedOn { get; set; } = DateTime.Now;
}
