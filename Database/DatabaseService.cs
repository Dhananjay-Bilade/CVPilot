using SQLite;
using CVPilot.Models;

namespace CVPilot.Database;

public class DatabaseService
{
    private SQLiteAsyncConnection? _database;

    public async Task InitAsync()
    {
        if (_database != null)
            return;

        string dbPath = Path.Combine(
            FileSystem.AppDataDirectory,
            "CVPilot.db");

        _database = new SQLiteAsyncConnection(dbPath);

        await _database.CreateTableAsync<Resume>();
        await _database.CreateTableAsync<PersonalDetails>();
        await _database.CreateTableAsync<Education>();
        await _database.CreateTableAsync<Experience>();
        await _database.CreateTableAsync<Skill>();
        await _database.CreateTableAsync<Project>();
        await _database.CreateTableAsync<Objective>();
        await _database.CreateTableAsync<Certification>();
        await _database.CreateTableAsync<Language>();
        await _database.CreateTableAsync<Reference>();
        await _database.CreateTableAsync<Achievement>();
    }

    // Save Resume
    public async Task<int> SaveResumeAsync(Resume resume)
    {
        await InitAsync();
        return await _database!.InsertAsync(resume);
    }

    // Save Personal Details
    public async Task<int> SavePersonalDetailsAsync(
    PersonalDetails details)
    {
        await InitAsync();

        int result =
            await _database!
                .InsertAsync(details);

        await RecalculateCompletionPercentageAsync(
            details.ResumeId);

        return result;
    }

    // Get Personal Details
    public async Task<PersonalDetails?> GetPersonalDetailsAsync(int resumeId)
    {
        await InitAsync();

        return await _database!.Table<PersonalDetails>()
                               .FirstOrDefaultAsync(x => x.ResumeId == resumeId);
    }

    // Update Personal Details
    public async Task<int> UpdatePersonalDetailsAsync(
        PersonalDetails details)
    {
        await InitAsync();

        details.UpdatedOn = DateTime.Now;

        int result =
            await _database!.UpdateAsync(details);

        await RecalculateCompletionPercentageAsync(
            details.ResumeId);

        return result;
    }

    // Save Education
    public async Task<int> SaveEducationAsync(
    Education education)
    {
        await InitAsync();

        int result =
            await _database!
                .InsertAsync(education);

        await RecalculateCompletionPercentageAsync(
            education.ResumeId);

        return result;
    }

    // Get All Education By Resume
    public async Task<List<Education>> GetEducationByResumeIdAsync(int resumeId)
    {
        await InitAsync();

        return await _database!
            .Table<Education>()
            .Where(x => x.ResumeId == resumeId)
            .OrderByDescending(x => x.EndYear)
            .ToListAsync();
    }

    // Get Single Education
    public async Task<Education?> GetEducationAsync(int id)
    {
        await InitAsync();

        return await _database!
            .Table<Education>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // Update Education
    public async Task<int> UpdateEducationAsync(
        Education education)
    {
        await InitAsync();

        education.UpdatedOn = DateTime.Now;

        int result =
            await _database!.UpdateAsync(education);

        await RecalculateCompletionPercentageAsync(
            education.ResumeId);

        return result;
    }

    // Delete Education
    public async Task<int> DeleteEducationAsync(
        Education education)
    {
        await InitAsync();

        int result =
            await _database!.DeleteAsync(education);

        await RecalculateCompletionPercentageAsync(
            education.ResumeId);

        return result;
    }

    // Delete Resume
    public async Task<int> DeleteResumeAsync(Resume resume)
    {
        await InitAsync();
        return await _database!.DeleteAsync(resume);
    }

    public async Task<Resume?> GetResumeAsync(int id)
    {
        await InitAsync();

        return await _database!
            .Table<Resume>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // Save Experience
    public async Task<int> SaveExperienceAsync(
    Experience experience)
    {
        await InitAsync();

        int result =
            await _database!
                .InsertAsync(experience);

        await RecalculateCompletionPercentageAsync(
            experience.ResumeId);

        return result;
    }

    // Get All Experience By Resume
    public async Task<List<Experience>> GetExperienceByResumeIdAsync(int resumeId)
    {
        await InitAsync();

        return await _database!
            .Table<Experience>()
            .Where(x => x.ResumeId == resumeId)
            .OrderByDescending(x => x.EndYear)
            .ToListAsync();
    }

    // Get Single Experience
    public async Task<Experience?> GetExperienceAsync(int id)
    {
        await InitAsync();

        return await _database!
            .Table<Experience>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // Update Experience
    public async Task<int> UpdateExperienceAsync(
        Experience experience)
    {
        await InitAsync();

        experience.UpdatedOn = DateTime.Now;

        int result =
            await _database!.UpdateAsync(experience);

        await RecalculateCompletionPercentageAsync(
            experience.ResumeId);

        return result;
    }

    // Delete Experience
    public async Task<int> DeleteExperienceAsync(
        Experience experience)
    {
        await InitAsync();

        int result =
            await _database!.DeleteAsync(experience);

        await RecalculateCompletionPercentageAsync(
            experience.ResumeId);

        return result;
    }

    // Save Skill
    public async Task<int> SaveSkillAsync(
    Skill skill)
    {
        await InitAsync();

        int result =
            await _database!
                .InsertAsync(skill);

        await RecalculateCompletionPercentageAsync(
            skill.ResumeId);

        return result;
    }

    // Get All Skills By Resume
    public async Task<List<Skill>> GetSkillsByResumeIdAsync(int resumeId)
    {
        await InitAsync();

        return await _database!
            .Table<Skill>()
            .Where(x => x.ResumeId == resumeId)
            .OrderByDescending(x => x.SkillLevel)
            .ToListAsync();
    }

    // Get Single Skill
    public async Task<Skill?> GetSkillAsync(int id)
    {
        await InitAsync();

        return await _database!
            .Table<Skill>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // Update Skill
    public async Task<int> UpdateSkillAsync(
        Skill skill)
    {
        await InitAsync();

        skill.UpdatedOn = DateTime.Now;

        int result =
            await _database!.UpdateAsync(skill);

        await RecalculateCompletionPercentageAsync(
            skill.ResumeId);

        return result;
    }

    // Delete Skill
    public async Task<int> DeleteSkillAsync(
        Skill skill)
    {
        await InitAsync();

        int result =
            await _database!.DeleteAsync(skill);

        await RecalculateCompletionPercentageAsync(
            skill.ResumeId);

        return result;
    }

    // Save Project
    public async Task<int> SaveProjectAsync(
    Project project)
    {
        await InitAsync();

        int result =
            await _database!
                .InsertAsync(project);

        await RecalculateCompletionPercentageAsync(
            project.ResumeId);

        return result;
    }

    // Get All Projects By Resume
    public async Task<List<Project>> GetProjectsByResumeIdAsync(int resumeId)
    {
        await InitAsync();

        return await _database!
            .Table<Project>()
            .Where(x => x.ResumeId == resumeId)
            .OrderByDescending(x => x.EndYear)
            .ToListAsync();
    }

    // Get Single Project
    public async Task<Project?> GetProjectAsync(int id)
    {
        await InitAsync();

        return await _database!
            .Table<Project>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // Update Project
    public async Task<int> UpdateProjectAsync(
        Project project)
    {
        await InitAsync();

        project.UpdatedOn = DateTime.Now;

        int result =
            await _database!.UpdateAsync(project);

        await RecalculateCompletionPercentageAsync(
            project.ResumeId);

        return result;
    }

    // Delete Project
    public async Task<int> DeleteProjectAsync(
        Project project)
    {
        await InitAsync();

        int result =
            await _database!.DeleteAsync(project);

        await RecalculateCompletionPercentageAsync(
            project.ResumeId);

        return result;
    }

    // Save Objective
    public async Task<int> SaveObjectiveAsync(
    Objective objective)
    {
        await InitAsync();

        int result =
            await _database!
                .InsertAsync(objective);

        await RecalculateCompletionPercentageAsync(
            objective.ResumeId);

        return result;
    }

    // Get Objective
    public async Task<Objective?> GetObjectiveAsync(int resumeId)
    {
        await InitAsync();

        return await _database!.Table<Objective>()
                              .FirstOrDefaultAsync(x => x.ResumeId == resumeId);
    }

    // Update Objective
    public async Task<int> UpdateObjectiveAsync(
        Objective objective)
    {
        await InitAsync();

        objective.UpdatedOn = DateTime.Now;

        int result =
            await _database!.UpdateAsync(objective);

        await RecalculateCompletionPercentageAsync(
            objective.ResumeId);

        return result;
    }

    // Save Certification
    public async Task<int> SaveCertificationAsync(
    Certification certification)
    {
        await InitAsync();

        int result =
            await _database!
                .InsertAsync(certification);

        await RecalculateCompletionPercentageAsync(
            certification.ResumeId);

        return result;
    }

    // Get All
    public async Task<List<Certification>> GetCertificationsByResumeIdAsync(int resumeId)
    {
        await InitAsync();

        return await _database!
            .Table<Certification>()
            .Where(x => x.ResumeId == resumeId)
            .OrderByDescending(x => x.IssueDate)
            .ToListAsync();
    }

    // Get One
    public async Task<Certification?> GetCertificationAsync(int id)
    {
        await InitAsync();

        return await _database!
            .Table<Certification>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // Update Certification
    public async Task<int> UpdateCertificationAsync(
        Certification certification)
    {
        await InitAsync();

        certification.UpdatedOn = DateTime.Now;

        int result =
            await _database!.UpdateAsync(certification);

        await RecalculateCompletionPercentageAsync(
            certification.ResumeId);

        return result;
    }

    // Delete Certification
    public async Task<int> DeleteCertificationAsync(
        Certification certification)
    {
        await InitAsync();

        int result =
            await _database!.DeleteAsync(certification);

        await RecalculateCompletionPercentageAsync(
            certification.ResumeId);

        return result;
    }

    // Save Language
    public async Task<int> SaveLanguageAsync(
    Language language)
    {
        await InitAsync();

        int result =
            await _database!
                .InsertAsync(language);

        await RecalculateCompletionPercentageAsync(
            language.ResumeId);

        return result;
    }

    public async Task<List<Language>> GetLanguagesByResumeIdAsync(int resumeId)
    {
        await InitAsync();

        return await _database!.Table<Language>()
            .Where(x => x.ResumeId == resumeId)
            .OrderBy(x => x.LanguageName)
            .ToListAsync();
    }

    public async Task<Language?> GetLanguageAsync(int id)
    {
        await InitAsync();

        return await _database!.Table<Language>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // Update Language
    public async Task<int> UpdateLanguageAsync(
        Language language)
    {
        await InitAsync();

        language.UpdatedOn = DateTime.Now;

        int result =
            await _database!.UpdateAsync(language);

        await RecalculateCompletionPercentageAsync(
            language.ResumeId);

        return result;
    }

    // Delete Language
    public async Task<int> DeleteLanguageAsync(
        Language language)
    {
        await InitAsync();

        int result =
            await _database!.DeleteAsync(language);

        await RecalculateCompletionPercentageAsync(
            language.ResumeId);

        return result;
    }

    // Save References
    public async Task<int> SaveReferenceAsync(
    Reference reference)
    {
        await InitAsync();

        int result =
            await _database!
                .InsertAsync(reference);

        await RecalculateCompletionPercentageAsync(
            reference.ResumeId);

        return result;
    }

    public async Task<List<Reference>> GetReferencesByResumeIdAsync(int resumeId)
    {
        await InitAsync();

        return await _database!.Table<Reference>()
            .Where(x => x.ResumeId == resumeId)
            .OrderBy(x => x.FullName)
            .ToListAsync();
    }

    public async Task<Reference?> GetReferenceAsync(int id)
    {
        await InitAsync();

        return await _database!.Table<Reference>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // Update Reference
    public async Task<int> UpdateReferenceAsync(
        Reference reference)
    {
        await InitAsync();

        reference.UpdatedOn = DateTime.Now;

        int result =
            await _database!.UpdateAsync(reference);

        await RecalculateCompletionPercentageAsync(
            reference.ResumeId);

        return result;
    }

    // Delete Reference
    public async Task<int> DeleteReferenceAsync(
        Reference reference)
    {
        await InitAsync();

        int result =
            await _database!.DeleteAsync(reference);

        await RecalculateCompletionPercentageAsync(
            reference.ResumeId);

        return result;
    }

    // Save Achievements
    public async Task<int> SaveAchievementAsync(
    Achievement achievement)
    {
        await InitAsync();

        int result =
            await _database!
                .InsertAsync(achievement);

        await RecalculateCompletionPercentageAsync(
            achievement.ResumeId);

        return result;
    }

    public async Task<List<Achievement>> GetAchievementsByResumeIdAsync(int resumeId)
    {
        await InitAsync();

        return await _database!.Table<Achievement>()
            .Where(x => x.ResumeId == resumeId)
            .OrderByDescending(x => x.AchievementDate)
            .ToListAsync();
    }

    public async Task<Achievement?> GetAchievementAsync(int id)
    {
        await InitAsync();

        return await _database!.Table<Achievement>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // Update Achievement
    public async Task<int> UpdateAchievementAsync(
        Achievement achievement)
    {
        await InitAsync();

        achievement.UpdatedOn = DateTime.Now;

        int result =
            await _database!.UpdateAsync(achievement);

        await RecalculateCompletionPercentageAsync(
            achievement.ResumeId);

        return result;
    }

    // Delete Achievement
    public async Task<int> DeleteAchievementAsync(
        Achievement achievement)
    {
        await InitAsync();

        int result =
            await _database!.DeleteAsync(achievement);

        await RecalculateCompletionPercentageAsync(
            achievement.ResumeId);

        return result;
    }

    // Get All Resumes
    public async Task<List<Resume>> GetAllResumesAsync()
    {
        await InitAsync();

        return await _database!
            .Table<Resume>()
            .OrderByDescending(x => x.UpdatedOn)
            .ToListAsync();
    }

    // Recalculate Resume Completion Percentage
    public async Task<int> RecalculateCompletionPercentageAsync(
        int resumeId)
    {
        await InitAsync();

        var resume =
            await _database!
                .Table<Resume>()
                .FirstOrDefaultAsync(x => x.Id == resumeId);

        if (resume == null)
            return 0;

        int completedSections = 0;

        // 1. Personal Details
        var personalDetails =
            await _database!
                .Table<PersonalDetails>()
                .FirstOrDefaultAsync(
                    x => x.ResumeId == resumeId);

        if (personalDetails != null)
            completedSections++;

        // 2. Education
        var education =
            await _database!
                .Table<Education>()
                .Where(x => x.ResumeId == resumeId)
                .ToListAsync();

        if (education.Count > 0)
            completedSections++;

        // 3. Experience
        var experience =
            await _database!
                .Table<Experience>()
                .Where(x => x.ResumeId == resumeId)
                .ToListAsync();

        if (experience.Count > 0)
            completedSections++;

        // 4. Skills
        var skills =
            await _database!
                .Table<Skill>()
                .Where(x => x.ResumeId == resumeId)
                .ToListAsync();

        if (skills.Count > 0)
            completedSections++;

        // 5. Projects
        var projects =
            await _database!
                .Table<Project>()
                .Where(x => x.ResumeId == resumeId)
                .ToListAsync();

        if (projects.Count > 0)
            completedSections++;

        // 6. Objective
        var objective =
            await _database!
                .Table<Objective>()
                .FirstOrDefaultAsync(
                    x => x.ResumeId == resumeId);

        if (objective != null)
            completedSections++;

        // 7. Certifications
        var certifications =
            await _database!
                .Table<Certification>()
                .Where(x => x.ResumeId == resumeId)
                .ToListAsync();

        if (certifications.Count > 0)
            completedSections++;

        // 8. Languages
        var languages =
            await _database!
                .Table<Language>()
                .Where(x => x.ResumeId == resumeId)
                .ToListAsync();

        if (languages.Count > 0)
            completedSections++;

        // 9. References
        var references =
            await _database!
                .Table<Reference>()
                .Where(x => x.ResumeId == resumeId)
                .ToListAsync();

        if (references.Count > 0)
            completedSections++;

        // 10. Achievements
        var achievements =
            await _database!
                .Table<Achievement>()
                .Where(x => x.ResumeId == resumeId)
                .ToListAsync();

        if (achievements.Count > 0)
            completedSections++;

        // Calculate percentage
        int percentage =
            completedSections * 10;

        if (percentage > 100)
            percentage = 100;

        // Update Resume
        resume.CompletionPercentage = percentage;
        resume.IsCompleted = percentage == 100;
        resume.UpdatedOn = DateTime.Now;

        await _database!.UpdateAsync(resume);

        return percentage;
    }
    public SQLiteAsyncConnection Database => _database!;
}

