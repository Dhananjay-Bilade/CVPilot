using CVPilot.Database;
using CVPilot.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CVPilot.Views.Sections;

[QueryProperty(nameof(ResumeId), "ResumeId")]
public partial class EducationPage : ContentPage
{
    private readonly DatabaseService _databaseService =
        IPlatformApplication.Current!.Services.GetRequiredService<DatabaseService>();

    public int ResumeId { get; set; }

    public EducationPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadEducationAsync();
    }

    private async Task LoadEducationAsync()
    {
        var educationList = await _databaseService.GetEducationByResumeIdAsync(ResumeId);

        EducationCollection.ItemsSource = educationList;
    }

    private async void AddEducation_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(AddEducationPage)}?ResumeId={ResumeId}");
    }

    private async void Edit_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        if (!int.TryParse(button.CommandParameter?.ToString(), out int educationId))
            return;

        await Shell.Current.GoToAsync(
            $"{nameof(AddEducationPage)}?ResumeId={ResumeId}&EducationId={educationId}");
    }

    private async void Delete_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        if (!int.TryParse(button.CommandParameter?.ToString(), out int educationId))
            return;

        var education = await _databaseService.GetEducationAsync(educationId);

        if (education == null)
            return;

        bool confirm = await DisplayAlertAsync(
            "Delete Education",
            $"Are you sure you want to delete '{education.Degree}'?",
            "Delete",
            "Cancel");

        if (!confirm)
            return;

        await _databaseService.DeleteEducationAsync(education);

        await LoadEducationAsync();

        await DisplayAlertAsync(
            "Deleted",
            "Education deleted successfully.",
            "OK");
    }
}

