using CVPilot.Views.Resume;
using CVPilot.Views.Sections;
using CVPilot.Views.Settings;
using CVPilot.Views.ATSChecker;
namespace CVPilot;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(CreateResumePage), typeof(CreateResumePage));
        Routing.RegisterRoute(nameof(SettingsPage),typeof(SettingsPage));
        Routing.RegisterRoute(nameof(MyResumePage), typeof(MyResumePage));
        Routing.RegisterRoute(nameof(ATSCheckerPage),typeof(ATSCheckerPage));
        Routing.RegisterRoute(nameof(ResumeSectionsPage), typeof(ResumeSectionsPage));
        Routing.RegisterRoute(nameof(PersonalDetailsPage), typeof(PersonalDetailsPage));
        Routing.RegisterRoute(nameof(EducationPage), typeof(EducationPage));
        Routing.RegisterRoute(nameof(AddEducationPage), typeof(AddEducationPage));
        Routing.RegisterRoute(nameof(ExperiencePage), typeof(ExperiencePage));
        Routing.RegisterRoute(nameof(AddExperiencePage), typeof(AddExperiencePage));
        Routing.RegisterRoute(nameof(SkillsPage), typeof(SkillsPage));
        Routing.RegisterRoute(nameof(AddSkillPage), typeof(AddSkillPage));
        Routing.RegisterRoute(nameof(ProjectsPage), typeof(ProjectsPage));
        Routing.RegisterRoute(nameof(AddProjectPage), typeof(AddProjectPage));
        Routing.RegisterRoute(nameof(ObjectivePage), typeof(ObjectivePage));
        Routing.RegisterRoute(nameof(CertificationsPage), typeof(CertificationsPage));
        Routing.RegisterRoute(nameof(AddCertificationPage), typeof(AddCertificationPage));
        Routing.RegisterRoute(nameof(LanguagesPage), typeof(LanguagesPage));
        Routing.RegisterRoute(nameof(AddLanguagePage), typeof(AddLanguagePage));
        Routing.RegisterRoute(nameof(ReferencesPage), typeof(ReferencesPage));
        Routing.RegisterRoute(nameof(AddReferencePage), typeof(AddReferencePage));
        Routing.RegisterRoute(nameof(AchievementsPage), typeof(AchievementsPage));
        Routing.RegisterRoute(nameof(AddAchievementPage), typeof(AddAchievementPage));
        Routing.RegisterRoute(nameof(ResumePreviewPage), typeof(ResumePreviewPage));
    }
}