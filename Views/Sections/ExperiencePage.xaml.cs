
using CVPilot.Database;
using CVPilot.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CVPilot.Views.Sections;

[QueryProperty(nameof(ResumeId), "ResumeId")]
public partial class ExperiencePage : ContentPage
{
    private readonly DatabaseService _databaseService =
        IPlatformApplication.Current!.Services.GetRequiredService<DatabaseService>();

    public int ResumeId { get; set; }

    public ExperiencePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadExperienceAsync();
    }

    private async Task LoadExperienceAsync()
    {
        var list = await _databaseService.GetExperienceByResumeIdAsync(ResumeId);

        ExperienceCollection.ItemsSource = list;
    }

    private async void AddExperience_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(AddExperiencePage)}?ResumeId={ResumeId}");
    }

    private async void Edit_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        if (!int.TryParse(button.CommandParameter?.ToString(), out int id))
            return;

        await Shell.Current.GoToAsync(
            $"{nameof(AddExperiencePage)}?ResumeId={ResumeId}&ExperienceId={id}");
    }

    private async void Delete_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        if (!int.TryParse(button.CommandParameter?.ToString(), out int id))
            return;

        var experience = await _databaseService.GetExperienceAsync(id);

        if (experience == null)
            return;

        bool result = await DisplayAlertAsync(
            "Delete Experience",
            $"Are you sure you want to delete '{experience.JobTitle}'?",
            "Delete",
            "Cancel");

        if (!result)
            return;

        await _databaseService.DeleteExperienceAsync(experience);

        await LoadExperienceAsync();

        await DisplayAlertAsync(
            "Deleted",
            "Experience deleted successfully.",
            "OK");
    }
}

