using CVPilot.Database;
using CVPilot.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CVPilot.Views.Sections;

[QueryProperty(nameof(ResumeId), "ResumeId")]
public partial class AchievementsPage : ContentPage
{
    private readonly DatabaseService _databaseService =
        IPlatformApplication.Current!
        .Services
        .GetRequiredService<DatabaseService>();

    public int ResumeId { get; set; }

    public AchievementsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAchievements();
    }

    private async Task LoadAchievements()
    {
        AchievementsCollection.ItemsSource =
            await _databaseService.GetAchievementsByResumeIdAsync(ResumeId);
    }

    private async void AddAchievement_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(AddAchievementPage)}?ResumeId={ResumeId}");
    }

    private async void Edit_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        if (!int.TryParse(button.CommandParameter?.ToString(), out int id))
            return;

        await Shell.Current.GoToAsync(
            $"{nameof(AddAchievementPage)}?ResumeId={ResumeId}&AchievementId={id}");
    }

    private async void Delete_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        if (!int.TryParse(button.CommandParameter?.ToString(), out int id))
            return;

        var achievement = await _databaseService.GetAchievementAsync(id);

        if (achievement == null)
            return;

        bool confirm = await DisplayAlertAsync(
            "Delete Achievement",
            $"Delete '{achievement.Title}'?",
            "Delete",
            "Cancel");

        if (!confirm)
            return;

        await _databaseService.DeleteAchievementAsync(achievement);

        await LoadAchievements();

        await DisplayAlertAsync(
            "Deleted",
            "Achievement deleted successfully.",
            "OK");
    }
}