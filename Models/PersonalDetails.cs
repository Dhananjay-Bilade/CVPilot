namespace CVPilot.Models;
using SQLite;

public class PersonalDetails
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // Resume Relationship
    public int ResumeId { get; set; }

    // Basic Information
    public string FullName { get; set; } = string.Empty;

    public string ProfessionalTitle { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string Gender { get; set; } = string.Empty;

    public string Nationality { get; set; } = string.Empty;

    // Contact Information
    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string AlternatePhone { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string Pincode { get; set; } = string.Empty;

    // Professional Links
    public string LinkedInUrl { get; set; } = string.Empty;

    public string GitHubUrl { get; set; } = string.Empty;

    public string PortfolioUrl { get; set; } = string.Empty;

    // Summary
    public string ProfessionalSummary { get; set; } = string.Empty;

    // Optional
    public string ProfilePhotoPath { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public DateTime UpdatedOn { get; set; } = DateTime.Now;
}
