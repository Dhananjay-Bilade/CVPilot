using CVPilot.Database;
using Microsoft.Extensions.DependencyInjection;
using ResumeModel = CVPilot.Models.Resume;
using CVPilot.Views.Sections;

using System.Globalization;

namespace CVPilot.Views.Resume;

public partial class MyResumePage : ContentPage
{
    private readonly DatabaseService _databaseService;

    public MyResumePage()
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

        await LoadResumesAsync();
    }

    private async Task LoadResumesAsync()
    {
        try
        {
            var resumes =
                await _databaseService.GetAllResumesAsync();

            ResumeCollectionView.ItemsSource = resumes;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Error",
                $"Unable to load resumes.\n\n{ex.Message}",
                "OK");
        }
    }

    private async void CreateNewResume_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(CreateResumePage));
    }

    private async void Preview_Clicked(
        object sender,
        EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not ResumeModel resume)
            return;

        await Shell.Current.GoToAsync(
            $"{nameof(ResumePreviewPage)}?ResumeId={resume.Id}");
    }

    private async void Edit_Clicked(
        object sender,
        EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not ResumeModel resume)
            return;

        await Shell.Current.GoToAsync(
            $"{nameof(ResumeSectionsPage)}?ResumeId={resume.Id}");
    }

    private async void Export_Clicked(
        object sender,
        EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not ResumeModel resume)
            return;

        try
        {
            await Shell.Current.GoToAsync(
                $"{nameof(ResumePreviewPage)}?ResumeId={resume.Id}");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Error",
                $"Unable to open resume.\n\n{ex.Message}",
                "OK");
        }
    }

    private async void Delete_Clicked(
        object sender,
        EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not ResumeModel resume)
            return;

        bool confirm =
            await DisplayAlertAsync(
                "Delete Resume",
                $"Are you sure you want to delete \"{resume.ResumeName}\"?",
                "Delete",
                "Cancel");

        if (!confirm)
            return;

        try
        {
            await _databaseService.DeleteResumeAsync(resume);

            await LoadResumesAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Error",
                $"Unable to delete resume.\n\n{ex.Message}",
                "OK");
        }
    }
}

public class PercentageToProgressConverter : IValueConverter
{
    public object Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        if (value is int percentage)
            return Math.Clamp(percentage / 100.0, 0.0, 1.0);

        return 0.0;
    }

    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}