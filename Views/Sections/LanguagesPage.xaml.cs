using CVPilot.Database;
using CVPilot.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CVPilot.Views.Sections;

[QueryProperty(nameof(ResumeId), "ResumeId")]
public partial class LanguagesPage : ContentPage
{
    private readonly DatabaseService _databaseService =
        IPlatformApplication.Current!
        .Services
        .GetRequiredService<DatabaseService>();

    public int ResumeId { get; set; }

    public LanguagesPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadLanguagesAsync();
    }

    private async Task LoadLanguagesAsync()
    {
        LanguagesCollection.ItemsSource =
            await _databaseService.GetLanguagesByResumeIdAsync(ResumeId);
    }

    private async void AddLanguage_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(AddLanguagePage)}?ResumeId={ResumeId}");
    }

    private async void Edit_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        if (!int.TryParse(button.CommandParameter?.ToString(), out int id))
            return;

        await Shell.Current.GoToAsync(
            $"{nameof(AddLanguagePage)}?ResumeId={ResumeId}&LanguageId={id}");
    }

    private async void Delete_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        if (!int.TryParse(button.CommandParameter?.ToString(), out int id))
            return;

        var language = await _databaseService.GetLanguageAsync(id);

        if (language == null)
            return;

        bool confirm = await DisplayAlertAsync(
            "Delete Language",
            $"Delete '{language.LanguageName}'?",
            "Delete",
            "Cancel");

        if (!confirm)
            return;

        await _databaseService.DeleteLanguageAsync(language);

        await LoadLanguagesAsync();

        await DisplayAlertAsync(
            "Deleted",
            "Language deleted successfully.",
            "OK");
    }
}