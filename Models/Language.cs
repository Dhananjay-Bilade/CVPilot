using SQLite;

namespace CVPilot.Models;

public class Language
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int ResumeId { get; set; }

    public string LanguageName { get; set; } = string.Empty;

    // Beginner, Intermediate, Advanced, Fluent, Native
    public string Proficiency { get; set; } = "Beginner";

    public bool CanRead { get; set; }

    public bool CanWrite { get; set; }

    public bool CanSpeak { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    [Ignore]
    public string SkillsSummary
    {
        get
        {
            List<string> skills = new();

            if (CanRead) skills.Add("Read");
            if (CanWrite) skills.Add("Write");
            if (CanSpeak) skills.Add("Speak");

            return string.Join(", ", skills);
        }
    }
}