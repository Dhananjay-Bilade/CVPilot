using CVPilot.Database;
using CVPilot.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CVPilot.Views.Sections;

[QueryProperty(nameof(ResumeId), "ResumeId")]
[QueryProperty(nameof(SkillId), "SkillId")]
public partial class AddSkillPage : ContentPage
{
    private readonly DatabaseService _databaseService =
        IPlatformApplication.Current!.Services.GetRequiredService<DatabaseService>();

    public int ResumeId { get; set; }

    public int SkillId { get; set; }

    public AddSkillPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (SkillId > 0)
        {
            Title = "Edit Skill";
            SaveButton.Text = "Update Skill";

            var skill = await _databaseService.GetSkillAsync(SkillId);

            if (skill == null)
                return;

            SkillNameEntry.Text = skill.SkillName;
            CategoryPicker.SelectedItem = skill.SkillCategory;
            SkillLevelSlider.Value = skill.SkillLevel;
            SkillLevelLabel.Text = $"{skill.SkillLevel}%";
        }
        else
        {
            Title = "Add Skill";
            SaveButton.Text = "Save Skill";
        }
    }

    private void SkillLevelSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        SkillLevelLabel.Text = $"{(int)e.NewValue}%";
    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SkillNameEntry.Text))
        {
            await DisplayAlertAsync("Validation", "Please enter skill name.", "OK");
            return;
        }

        if (SkillId > 0)
        {
            var skill = await _databaseService.GetSkillAsync(SkillId);

            if (skill == null)
                return;

            skill.SkillName = SkillNameEntry.Text ?? "";
            skill.SkillCategory = CategoryPicker.SelectedItem?.ToString() ?? "";
            skill.SkillLevel = (int)SkillLevelSlider.Value;

            await _databaseService.UpdateSkillAsync(skill);

            await DisplayAlertAsync(
                "Success",
                "Skill updated successfully.",
                "OK");
        }
        else
        {
            Skill skill = new()
            {
                ResumeId = ResumeId,
                SkillName = SkillNameEntry.Text ?? "",
                SkillCategory = CategoryPicker.SelectedItem?.ToString() ?? "",
                SkillLevel = (int)SkillLevelSlider.Value,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            await _databaseService.SaveSkillAsync(skill);

            await DisplayAlertAsync(
                "Success",
                "Skill saved successfully.",
                "OK");
        }

        await Shell.Current.GoToAsync("..");
    }
}

