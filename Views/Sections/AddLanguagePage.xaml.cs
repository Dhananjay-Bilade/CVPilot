using CVPilot.Database;
using CVPilot.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CVPilot.Views.Sections;

[QueryProperty(nameof(ResumeId), "ResumeId")]
[QueryProperty(nameof(LanguageId), "LanguageId")]
public partial class AddLanguagePage : ContentPage
{
    private readonly DatabaseService _databaseService =
        IPlatformApplication.Current!
        .Services
        .GetRequiredService<DatabaseService>();

    public int ResumeId { get; set; }

    public int LanguageId { get; set; }

    private Language? _language;

    public AddLanguagePage()
    {
        InitializeComponent();

        ProficiencyPicker.SelectedIndex = 0;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (LanguageId <= 0)
            return;

        Title = "Edit Language";

        SaveButton.Text = "Update Language";

        _language = await _databaseService.GetLanguageAsync(LanguageId);

        if (_language == null)
            return;

        LanguageNameEntry.Text = _language.LanguageName;

        ProficiencyPicker.SelectedItem = _language.Proficiency;

        ReadCheckBox.IsChecked = _language.CanRead;
        WriteCheckBox.IsChecked = _language.CanWrite;
        SpeakCheckBox.IsChecked = _language.CanSpeak;
    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(LanguageNameEntry.Text))
        {
            await DisplayAlertAsync(
                "Validation",
                "Please enter a language.",
                "OK");

            return;
        }

        if (_language == null)
        {
            _language = new Language
            {
                ResumeId = ResumeId,
                CreatedOn = DateTime.Now
            };
        }

        _language.LanguageName = LanguageNameEntry.Text.Trim();
        _language.Proficiency = ProficiencyPicker.SelectedItem?.ToString() ?? "Beginner";

        _language.CanRead = ReadCheckBox.IsChecked;
        _language.CanWrite = WriteCheckBox.IsChecked;
        _language.CanSpeak = SpeakCheckBox.IsChecked;

        _language.UpdatedOn = DateTime.Now;

        if (LanguageId == 0)
            await _databaseService.SaveLanguageAsync(_language);
        else
            await _databaseService.UpdateLanguageAsync(_language);

        await DisplayAlertAsync(
            "Success",
            "Language saved successfully.",
            "OK");

        await Shell.Current.GoToAsync("..");
    }
}