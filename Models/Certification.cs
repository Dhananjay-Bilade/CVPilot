using SQLite;

namespace CVPilot.Models;

public class Certification
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int ResumeId { get; set; }

    public string CertificationName { get; set; } = string.Empty;

    public string IssuingOrganization { get; set; } = string.Empty;

    public string CredentialId { get; set; } = string.Empty;

    public string CredentialUrl { get; set; } = string.Empty;

    public DateTime IssueDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public bool DoesNotExpire { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    [Ignore]
    public string ExpiryText =>
        DoesNotExpire
            ? "No Expiration"
            : ExpiryDate?.ToString("dd MMM yyyy") ?? "-";
}
