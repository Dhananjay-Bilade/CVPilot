using SQLite;

namespace CVPilot.Models;

public class Education
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int ResumeId { get; set; }

    public string Degree { get; set; } = "";

    public string Institute { get; set; } = "";

    public string University { get; set; } = "";

    public string FieldOfStudy { get; set; } = "";

    public string Grade { get; set; } = "";

    public int StartYear { get; set; }

    public int EndYear { get; set; }

    public bool IsCurrentlyStudying { get; set; }

    public string Description { get; set; } = "";

    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public DateTime UpdatedOn { get; set; } = DateTime.Now;

    [Ignore]
    public string Duration =>
    IsCurrentlyStudying
        ? $"{StartYear} - Present"
        : $"{StartYear} - {EndYear}";
}