using CVPilot.Database;
using CVPilot.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CVPilot.Views.Sections;

[QueryProperty(nameof(ResumeId), "ResumeId")]
public partial class ReferencesPage : ContentPage
{
    private readonly DatabaseService _databaseService =
        IPlatformApplication.Current!
        .Services
        .GetRequiredService<DatabaseService>();

    public int ResumeId { get; set; }

    public ReferencesPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadReferences();
    }

    private async Task LoadReferences()
    {
        ReferencesCollection.ItemsSource =
            await _databaseService.GetReferencesByResumeIdAsync(ResumeId);
    }

    private async void AddReference_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(AddReferencePage)}?ResumeId={ResumeId}");
    }

    private async void Edit_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        if (!int.TryParse(button.CommandParameter?.ToString(), out int id))
            return;

        await Shell.Current.GoToAsync(
            $"{nameof(AddReferencePage)}?ResumeId={ResumeId}&ReferenceId={id}");
    }

    private async void Delete_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        if (!int.TryParse(button.CommandParameter?.ToString(), out int id))
            return;

        var reference = await _databaseService.GetReferenceAsync(id);

        if (reference == null)
            return;

        bool result = await DisplayAlertAsync(
            "Delete Reference",
            $"Delete {reference.FullName}?",
            "Delete",
            "Cancel");

        if (!result)
            return;

        await _databaseService.DeleteReferenceAsync(reference);

        await LoadReferences();

        await DisplayAlertAsync(
            "Success",
            "Reference deleted successfully.",
            "OK");
    }
}