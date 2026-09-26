using CVPilot.Database;
using CVPilot.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CVPilot.Views.Sections;

[QueryProperty(nameof(ResumeId), "ResumeId")]
[QueryProperty(nameof(AchievementId), "AchievementId")]
public partial class AddAchievementPage : ContentPage
{
    private readonly DatabaseService _databaseService =
        IPlatformApplication.Current!
        .Services
        .GetRequiredService<DatabaseService>();

    public int ResumeId { get; set; }

    public int AchievementId { get; set; }

    private Achievement? _achievement;

    public AddAchievementPage()
    {
        InitializeComponent();

        AchievementDatePicker.Date = DateTime.Today;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (AchievementId <= 0)
            return;

        Title = "Edit Achievement";
        SaveButton.Text = "Update Achievement";

        _achievement =
            await _databaseService.GetAchievementAsync(AchievementId);

        if (_achievement == null)
            return;

        TitleEntry.Text = _achievement.Title;
        OrganizationEntry.Text = _achievement.Organization;
        AchievementDatePicker.Date = _achievement.AchievementDate;
        DescriptionEditor.Text = _achievement.Description;
    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            await DisplayAlertAsync(
                "Validation",
                "Achievement title is required.",
                "OK");
            return;
        }

        if (_achievement == null)
        {
            _achievement = new Achievement
            {
                ResumeId = ResumeId,
                CreatedOn = DateTime.Now
            };
        }

        _achievement.Title = TitleEntry.Text.Trim();
        _achievement.Organization = OrganizationEntry.Text?.Trim() ?? "";
        _achievement.AchievementDate = (DateTime)AchievementDatePicker.Date!;
        _achievement.Description = DescriptionEditor.Text?.Trim() ?? "";
        _achievement.UpdatedOn = DateTime.Now;

        if (AchievementId == 0)
            await _databaseService.SaveAchievementAsync(_achievement);
        else
            await _databaseService.UpdateAchievementAsync(_achievement);

        await DisplayAlertAsync(
            "Success",
            "Achievement saved successfully.",
            "OK");

        await Shell.Current.GoToAsync("..");
    }
}