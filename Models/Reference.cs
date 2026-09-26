using SQLite;

namespace CVPilot.Models;

public class Reference
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int ResumeId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Designation { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Relationship { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    [Ignore]
    public string DisplayTitle =>
        $"{FullName} • {Designation}";
}
