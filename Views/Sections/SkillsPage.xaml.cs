using CVPilot.Database;
using CVPilot.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CVPilot.Views.Sections;

[QueryProperty(nameof(ResumeId), "ResumeId")]
public partial class SkillsPage : ContentPage
{
    private readonly DatabaseService _databaseService =
        IPlatformApplication.Current!.Services.GetRequiredService<DatabaseService>();

    public int ResumeId { get; set; }

    public SkillsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadSkillsAsync();
    }

    private async Task LoadSkillsAsync()
    {
        var list = await _databaseService.GetSkillsByResumeIdAsync(ResumeId);

        SkillsCollection.ItemsSource = list;
    }

    private async void AddSkill_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(AddSkillPage)}?ResumeId={ResumeId}");
    }

    private async void Edit_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        if (!int.TryParse(button.CommandParameter?.ToString(), out int id))
            return;

        await Shell.Current.GoToAsync(
            $"{nameof(AddSkillPage)}?ResumeId={ResumeId}&SkillId={id}");
    }

    private async void Delete_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        if (!int.TryParse(button.CommandParameter?.ToString(), out int id))
            return;

        var skill = await _databaseService.GetSkillAsync(id);

        if (skill == null)
            return;

        bool result = await DisplayAlertAsync(
            "Delete Skill",
            $"Delete '{skill.SkillName}'?",
            "Delete",
            "Cancel");

        if (!result)
            return;

        await _databaseService.DeleteSkillAsync(skill);

        await LoadSkillsAsync();

        await DisplayAlertAsync(
            "Deleted",
            "Skill deleted successfully.",
            "OK");
    }
}

