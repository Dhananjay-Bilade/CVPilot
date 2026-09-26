using CVPilot.Database;
using CVPilot.Views.Resume;
using Microsoft.Extensions.DependencyInjection;

namespace CVPilot.Views.Sections;

[QueryProperty(nameof(ResumeId), "ResumeId")]
public partial class ResumeSectionsPage : ContentPage
{
    private readonly DatabaseService _databaseService;

    public int ResumeId { get; set; }

    public ResumeSectionsPage()
    {
        InitializeComponent();

        _databaseService =
            IPlatformApplication.Current!
                .Services
                .GetRequiredService<DatabaseService>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadResumeProgressAsync();
    }

    private async Task LoadResumeProgressAsync()
    {
        if (ResumeId <= 0)
            return;

        try
        {
            var resume =
                await _databaseService.GetResumeAsync(ResumeId);

            if (resume == null)
                return;

            ResumeNameLabel.Text =
                string.IsNullOrWhiteSpace(resume.ResumeName)
                    ? "Resume"
                    : resume.ResumeName;

            int percentage =
                await _databaseService
                    .RecalculateCompletionPercentageAsync(ResumeId);

            ProgressPercentageLabel.Text =
                $"{percentage}%";

            double progress =
                percentage / 100.0;

            await ResumeProgressBar.ProgressTo(
                progress,
                300,
                Easing.CubicOut);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Error",
                $"Unable to load resume progress.\n\n{ex.Message}",
                "OK");
        }
    }

    private async void Personal_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(PersonalDetailsPage)}?ResumeId={ResumeId}");
    }

    private async void Education_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(EducationPage)}?ResumeId={ResumeId}");
    }

    private async void Experience_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(ExperiencePage)}?ResumeId={ResumeId}");
    }

    private async void Skills_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(SkillsPage)}?ResumeId={ResumeId}");
    }

    private async void Projects_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(ProjectsPage)}?ResumeId={ResumeId}");
    }

    private async void Objective_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(ObjectivePage)}?ResumeId={ResumeId}");
    }

    private async void Certifications_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(CertificationsPage)}?ResumeId={ResumeId}");
    }

    private async void Languages_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(LanguagesPage)}?ResumeId={ResumeId}");
    }

    private async void References_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(ReferencesPage)}?ResumeId={ResumeId}");
    }

    private async void Achievements_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(AchievementsPage)}?ResumeId={ResumeId}");
    }

    private async void PreviewResume_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(ResumePreviewPage)}?ResumeId={ResumeId}");
    }
}