using CVPilot.Database;
using CVPilot.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CVPilot.Views.Sections;

[QueryProperty(nameof(ResumeId), "ResumeId")]
[QueryProperty(nameof(ExperienceId), "ExperienceId")]
public partial class AddExperiencePage : ContentPage
{
    private readonly DatabaseService _databaseService =
        IPlatformApplication.Current!.Services.GetRequiredService<DatabaseService>();

    public int ResumeId { get; set; }

    public int ExperienceId { get; set; }

    public AddExperiencePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (ExperienceId > 0)
        {
            Title = "Edit Experience";
            SaveButton.Text = "Update Experience";

            var experience = await _databaseService.GetExperienceAsync(ExperienceId);

            if (experience == null)
                return;

            JobTitleEntry.Text = experience.JobTitle;
            CompanyEntry.Text = experience.CompanyName;
            LocationEntry.Text = experience.Location;

            EmploymentTypePicker.SelectedItem = experience.EmploymentType;

            StartMonthPicker.SelectedItem = experience.StartMonth;
            StartYearEntry.Text = experience.StartYear.ToString();

            EndMonthPicker.SelectedItem = experience.EndMonth;
            EndYearEntry.Text = experience.EndYear.ToString();

            CurrentJobCheckBox.IsChecked = experience.IsCurrentlyWorking;

            DescriptionEditor.Text = experience.Description;
        }
        else
        {
            Title = "Add Experience";
            SaveButton.Text = "Save Experience";
        }
    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(JobTitleEntry.Text))
        {
            await DisplayAlertAsync("Validation", "Please enter Job Title.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(CompanyEntry.Text))
        {
            await DisplayAlertAsync("Validation", "Please enter Company Name.", "OK");
            return;
        }

        int.TryParse(StartYearEntry.Text, out int startYear);
        int.TryParse(EndYearEntry.Text, out int endYear);

        if (ExperienceId > 0)
        {
            var experience =
                await _databaseService.GetExperienceAsync(ExperienceId);

            if (experience == null)
            {
                await DisplayAlertAsync("Error",
                    "Experience not found.",
                    "OK");
                return;
            }

            experience.JobTitle = JobTitleEntry.Text ?? "";
            experience.CompanyName = CompanyEntry.Text ?? "";
            experience.Location = LocationEntry.Text ?? "";
            experience.EmploymentType =
                EmploymentTypePicker.SelectedItem?.ToString() ?? "";

            experience.StartMonth =
                StartMonthPicker.SelectedItem?.ToString() ?? "";

            experience.StartYear = startYear;

            experience.EndMonth =
                EndMonthPicker.SelectedItem?.ToString() ?? "";

            experience.EndYear = endYear;

            experience.IsCurrentlyWorking =
                CurrentJobCheckBox.IsChecked;

            experience.Description =
                DescriptionEditor.Text ?? "";

            await _databaseService.UpdateExperienceAsync(experience);

            await DisplayAlertAsync(
                "Success",
                "Experience updated successfully.",
                "OK");
        }
        else
        {
            Experience experience = new()
            {
                ResumeId = ResumeId,

                JobTitle = JobTitleEntry.Text ?? "",

                CompanyName = CompanyEntry.Text ?? "",

                Location = LocationEntry.Text ?? "",

                EmploymentType =
                    EmploymentTypePicker.SelectedItem?.ToString() ?? "",

                StartMonth =
                    StartMonthPicker.SelectedItem?.ToString() ?? "",

                StartYear = startYear,

                EndMonth =
                    EndMonthPicker.SelectedItem?.ToString() ?? "",

                EndYear = endYear,

                IsCurrentlyWorking =
                    CurrentJobCheckBox.IsChecked,

                Description =
                    DescriptionEditor.Text ?? "",

                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            await _databaseService.SaveExperienceAsync(experience);

            await DisplayAlertAsync(
                "Success",
                "Experience saved successfully.",
                "OK");
        }

        await Shell.Current.GoToAsync("..");
    }
}

