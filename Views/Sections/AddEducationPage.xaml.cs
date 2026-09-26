using CVPilot.Database;
using CVPilot.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CVPilot.Views.Sections;

[QueryProperty(nameof(ResumeId), "ResumeId")]
[QueryProperty(nameof(EducationId), "EducationId")]
public partial class AddEducationPage : ContentPage
{
    private readonly DatabaseService _databaseService =
        IPlatformApplication.Current!.Services.GetRequiredService<DatabaseService>();

    public int ResumeId { get; set; }

    public int EducationId { get; set; }

    public AddEducationPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (EducationId > 0)
        {
            Title = "Edit Education";
            SaveButton.Text = "Update Education";

            var education = await _databaseService.GetEducationAsync(EducationId);

            if (education == null)
                return;

            DegreeEntry.Text = education.Degree;
            FieldEntry.Text = education.FieldOfStudy;
            InstituteEntry.Text = education.Institute;
            UniversityEntry.Text = education.University;
            GradeEntry.Text = education.Grade;
            StartYearEntry.Text = education.StartYear.ToString();
            EndYearEntry.Text = education.EndYear.ToString();
            CurrentCheckBox.IsChecked = education.IsCurrentlyStudying;
            DescriptionEditor.Text = education.Description;
        }
        else
        {
            Title = "Add Education";
            SaveButton.Text = "Save Education";
        }
    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(DegreeEntry.Text))
        {
            await DisplayAlertAsync("Validation", "Please enter degree.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(InstituteEntry.Text))
        {
            await DisplayAlertAsync("Validation", "Please enter institute.", "OK");
            return;
        }

        int.TryParse(StartYearEntry.Text, out int startYear);
        int.TryParse(EndYearEntry.Text, out int endYear);

        if (EducationId > 0)
        {
            // EDIT MODE
            var education = await _databaseService.GetEducationAsync(EducationId);

            if (education == null)
            {
                await DisplayAlertAsync("Error", "Education record not found.", "OK");
                return;
            }

            education.Degree = DegreeEntry.Text ?? "";
            education.FieldOfStudy = FieldEntry.Text ?? "";
            education.Institute = InstituteEntry.Text ?? "";
            education.University = UniversityEntry.Text ?? "";
            education.Grade = GradeEntry.Text ?? "";
            education.StartYear = startYear;
            education.EndYear = endYear;
            education.IsCurrentlyStudying = CurrentCheckBox.IsChecked;
            education.Description = DescriptionEditor.Text ?? "";

            await _databaseService.UpdateEducationAsync(education);

            await DisplayAlertAsync(
                "Success",
                "Education updated successfully.",
                "OK");
        }
        else
        {
            // ADD MODE
            Education education = new()
            {
                ResumeId = ResumeId,
                Degree = DegreeEntry.Text ?? "",
                FieldOfStudy = FieldEntry.Text ?? "",
                Institute = InstituteEntry.Text ?? "",
                University = UniversityEntry.Text ?? "",
                Grade = GradeEntry.Text ?? "",
                StartYear = startYear,
                EndYear = endYear,
                IsCurrentlyStudying = CurrentCheckBox.IsChecked,
                Description = DescriptionEditor.Text ?? "",
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            await _databaseService.SaveEducationAsync(education);

            await DisplayAlertAsync(
                "Success",
                "Education saved successfully.",
                "OK");
        }

        await Shell.Current.GoToAsync("..");
    }
}

