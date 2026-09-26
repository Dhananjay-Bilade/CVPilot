using CVPilot.Database;
using CVPilot.Models;
using CVPilot.Views.Sections;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Storage;

namespace CVPilot.Views.Resume;

public partial class CreateResumePage : ContentPage
{
    private readonly DatabaseService _databaseService;

    private const string DefaultCategoryPreferenceKey =
        "DefaultResumeCategory";

    public CreateResumePage()
    {
        InitializeComponent();

        LoadDefaultCategory();

        _databaseService =
            IPlatformApplication.Current!
                .Services
                .GetRequiredService<DatabaseService>();
    }

    private void LoadDefaultCategory()
    {
        string defaultCategory =
            Preferences.Get(
                DefaultCategoryPreferenceKey,
                "Professional");

        switch (defaultCategory)
        {
            case "Fresher":
                CategoryPicker.SelectedIndex = 1;
                break;

            case "Student":
                CategoryPicker.SelectedIndex = 2;
                break;

            case "Academic":
                CategoryPicker.SelectedIndex = 3;
                break;

            default:
                CategoryPicker.SelectedIndex = 0;
                break;
        }
    }

    private async void Continue_Clicked(
        object sender,
        EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ResumeName.Text))
        {
            await DisplayAlertAsync(
                "Validation",
                "Please enter Resume Name.",
                "OK");

            return;
        }

        CVPilot.Models.Resume resume =
            new CVPilot.Models.Resume
            {
                ResumeName =
                    ResumeName.Text.Trim(),

                ResumeCategory =
                    CategoryPicker.SelectedItem?.ToString()
                    ?? "Professional",

                TemplateName = "Classic",

                CompletionPercentage = 0,

                IsCompleted = false,

                CreatedOn = DateTime.Now,

                UpdatedOn = DateTime.Now
            };

        await _databaseService.SaveResumeAsync(resume);

        await Shell.Current.GoToAsync(
            $"{nameof(ResumeSectionsPage)}?ResumeId={resume.Id}");
    }
}