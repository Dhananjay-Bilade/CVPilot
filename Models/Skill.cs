using SQLite;

namespace CVPilot.Models;

public class Skill
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int ResumeId { get; set; }

    public string SkillName { get; set; } = string.Empty;

    public string SkillCategory { get; set; } = string.Empty;

    public int SkillLevel { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    [Ignore]
    public string LevelText => $"{SkillLevel}%";

    [Ignore]
    public double ProgressValue => SkillLevel / 100.0;
}