using CVPilot.Database;
using Microsoft.Extensions.DependencyInjection;

namespace CVPilot.Views.Sections;

[QueryProperty(nameof(ResumeId), "ResumeId")]
public partial class CertificationsPage : ContentPage
{
    private readonly DatabaseService _databaseService =
        IPlatformApplication.Current!
        .Services
        .GetRequiredService<DatabaseService>();

    public int ResumeId { get; set; }

    public CertificationsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadCertificationsAsync();
    }

    private async Task LoadCertificationsAsync()
    {
        CertificationsCollection.ItemsSource =
            await _databaseService.GetCertificationsByResumeIdAsync(ResumeId);
    }

    private async void AddCertification_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(AddCertificationPage)}?ResumeId={ResumeId}");
    }

    private async void Edit_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            int.TryParse(button.CommandParameter?.ToString(), out int id))
        {
            await Shell.Current.GoToAsync(
                $"{nameof(AddCertificationPage)}?ResumeId={ResumeId}&CertificationId={id}");
        }
    }

    private async void Delete_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        if (!int.TryParse(button.CommandParameter?.ToString(), out int id))
            return;

        var certification = await _databaseService.GetCertificationAsync(id);

        if (certification == null)
            return;

        bool confirm = await DisplayAlertAsync(
            "Delete Certification",
            $"Delete '{certification.CertificationName}'?",
            "Delete",
            "Cancel");

        if (!confirm)
            return;

        await _databaseService.DeleteCertificationAsync(certification);

        await LoadCertificationsAsync();
    }
}

