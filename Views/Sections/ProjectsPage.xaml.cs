using CVPilot.Database;
using Microsoft.Extensions.DependencyInjection;

namespace CVPilot.Views.Sections;

[QueryProperty(nameof(ResumeId), "ResumeId")]
public partial class ProjectsPage : ContentPage
{
    private readonly DatabaseService _databaseService =
        IPlatformApplication.Current!.Services.GetRequiredService<DatabaseService>();

    public int ResumeId { get; set; }

    public ProjectsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadProjectsAsync();
    }

    private async Task LoadProjectsAsync()
    {
        ProjectsCollection.ItemsSource =
            await _databaseService.GetProjectsByResumeIdAsync(ResumeId);
    }

    private async void AddProject_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(AddProjectPage)}?ResumeId={ResumeId}");
    }

    private async void Edit_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            int.TryParse(button.CommandParameter?.ToString(), out int id))
        {
            await Shell.Current.GoToAsync(
                $"{nameof(AddProjectPage)}?ResumeId={ResumeId}&ProjectId={id}");
        }
    }

    private async void Delete_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        if (!int.TryParse(button.CommandParameter?.ToString(), out int id))
            return;

        var project = await _databaseService.GetProjectAsync(id);

        if (project == null)
            return;

        bool confirm = await DisplayAlertAsync(
            "Delete Project",
            $"Delete '{project.ProjectName}'?",
            "Delete",
            "Cancel");

        if (!confirm)
            return;

        await _databaseService.DeleteProjectAsync(project);

        await LoadProjectsAsync();
    }

    private async void Github_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            !string.IsNullOrWhiteSpace(button.CommandParameter?.ToString()))
        {
            await Launcher.Default.OpenAsync(button.CommandParameter.ToString()!); //error due to !
        }
    }

    private async void Live_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            !string.IsNullOrWhiteSpace(button.CommandParameter?.ToString()))
        {
            await Launcher.Default.OpenAsync(button.CommandParameter.ToString()!);// error due to !
        }
    }
}

