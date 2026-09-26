using CVPilot.Database;
using CVPilot.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CVPilot.Views.Sections;

[QueryProperty(nameof(ResumeId), "ResumeId")]
[QueryProperty(nameof(ProjectId), "ProjectId")]
public partial class AddProjectPage : ContentPage
{
    private readonly DatabaseService _databaseService =
        IPlatformApplication.Current!.Services.GetRequiredService<DatabaseService>();

    public int ResumeId { get; set; }

    public int ProjectId { get; set; }

    private readonly List<string> _months =
    [
        "January","February","March","April","May","June",
        "July","August","September","October","November","December"
    ];

    public AddProjectPage()
    {
        InitializeComponent();

        StartMonthPicker.ItemsSource = _months;
        EndMonthPicker.ItemsSource = _months;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (ProjectId <= 0)
            return;

        Title = "Edit Project";
        SaveButton.Text = "Update Project";

        var project = await _databaseService.GetProjectAsync(ProjectId);

        if (project == null)
            return;

        ProjectNameEntry.Text = project.ProjectName;
        TechnologyEntry.Text = project.Technologies;
        DescriptionEditor.Text = project.Description;
        GithubEntry.Text = project.GithubLink;
        LiveDemoEntry.Text = project.LiveLink;

        StartMonthPicker.SelectedItem = project.StartMonth;
        EndMonthPicker.SelectedItem = project.EndMonth;

        StartYearEntry.Text = project.StartYear.ToString();
        EndYearEntry.Text = project.EndYear.ToString();

        CurrentProjectCheckBox.IsChecked = project.IsOngoing;
    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ProjectNameEntry.Text))
        {
            await DisplayAlertAsync("Validation",
                "Project name is required.",
                "OK");
            return;
        }

        if (!int.TryParse(StartYearEntry.Text, out int startYear))
        {
            await DisplayAlertAsync("Validation",
                "Invalid Start Year.",
                "OK");
            return;
        }

        int.TryParse(EndYearEntry.Text, out int endYear);

        if (ProjectId > 0)
        {
            var project = await _databaseService.GetProjectAsync(ProjectId);

            if (project == null)
                return;

            project.ProjectName = ProjectNameEntry.Text ?? "";
            project.Technologies = TechnologyEntry.Text ?? "";
            project.Description = DescriptionEditor.Text ?? "";
            project.GithubLink = GithubEntry.Text ?? "";
            project.LiveLink = LiveDemoEntry.Text ?? "";
            project.StartMonth = StartMonthPicker.SelectedItem?.ToString() ?? "";
            project.EndMonth = EndMonthPicker.SelectedItem?.ToString() ?? "";
            project.StartYear = startYear;
            project.EndYear = endYear;
            project.IsOngoing = CurrentProjectCheckBox.IsChecked;

            await _databaseService.UpdateProjectAsync(project);
        }
        else
        {
            Project project = new()
            {
                ResumeId = ResumeId,
                ProjectName = ProjectNameEntry.Text ?? "",
                Technologies = TechnologyEntry.Text ?? "",
                Description = DescriptionEditor.Text ?? "",
                GithubLink = GithubEntry.Text ?? "",
                LiveLink = LiveDemoEntry.Text ?? "",
                StartMonth = StartMonthPicker.SelectedItem?.ToString() ?? "",
                EndMonth = EndMonthPicker.SelectedItem?.ToString() ?? "",
                StartYear = startYear,
                EndYear = endYear,
                IsOngoing = CurrentProjectCheckBox.IsChecked,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            await _databaseService.SaveProjectAsync(project);
        }

        await Shell.Current.GoToAsync("..");
    }
}

