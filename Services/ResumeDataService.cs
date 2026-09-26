using CVPilot.Database;
using CVPilot.Models;

namespace CVPilot.Services;

public class ResumeDataService
{
    private readonly DatabaseService _database;

    public ResumeDataService(DatabaseService database)
    {
        _database = database;
    }

    public async Task<CompleteResume> GetCompleteResumeAsync(int resumeId)
    {
        return new CompleteResume
        {
            Resume = await _database.GetResumeAsync(resumeId),

            PersonalDetails =
                await _database.GetPersonalDetailsAsync(resumeId),

            Educations =
                await _database.GetEducationByResumeIdAsync(resumeId),

            Experiences =
                await _database.GetExperienceByResumeIdAsync(resumeId),

            Skills =
                await _database.GetSkillsByResumeIdAsync(resumeId),

            Projects =
                await _database.GetProjectsByResumeIdAsync(resumeId),

            Objective =
                await _database.GetObjectiveAsync(resumeId),

            Certifications =
                await _database.GetCertificationsByResumeIdAsync(resumeId),

            Languages =
                await _database.GetLanguagesByResumeIdAsync(resumeId),

            References =
                await _database.GetReferencesByResumeIdAsync(resumeId),

            Achievements =
                await _database.GetAchievementsByResumeIdAsync(resumeId)
        };
    }
}
