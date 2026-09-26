using SQLite;

namespace CVPilot.Models;

public class Achievement
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int ResumeId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Organization { get; set; } = string.Empty;

    public DateTime AchievementDate { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }
}
