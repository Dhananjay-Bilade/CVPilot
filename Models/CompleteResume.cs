namespace CVPilot.Models;

public class CompleteResume
{
    public Resume? Resume { get; set; }

    public PersonalDetails? PersonalDetails { get; set; }

    public List<Education> Educations { get; set; } = [];

    public List<Experience> Experiences { get; set; } = [];

    public List<Skill> Skills { get; set; } = [];

    public List<Project> Projects { get; set; } = [];

    public Objective? Objective { get; set; }

    public List<Certification> Certifications { get; set; } = [];

    public List<Language> Languages { get; set; } = [];

    public List<Reference> References { get; set; } = [];

    public List<Achievement> Achievements { get; set; } = [];
}
