using CVPilot.Database;
using CVPilot.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CVPilot.Views.Sections;

[QueryProperty(nameof(ResumeId), "ResumeId")]
public partial class ObjectivePage : ContentPage
{
    private readonly DatabaseService _databaseService =
        IPlatformApplication.Current!
        .Services
        .GetRequiredService<DatabaseService>();

    public int ResumeId { get; set; }

    private Objective? _objective;

    public ObjectivePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _objective = await _databaseService.GetObjectiveAsync(ResumeId);

        if (_objective != null)
        {
            ObjectiveEditor.Text = _objective.ObjectiveText;
            SaveButton.Text = "Update Objective";
        }
    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ObjectiveEditor.Text))
        {
            await DisplayAlertAsync(
                "Validation",
                "Please enter your career objective.",
                "OK");

            return;
        }

        if (_objective == null)
        {
            _objective = new Objective
            {
                ResumeId = ResumeId,
                ObjectiveText = ObjectiveEditor.Text,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            await _databaseService.SaveObjectiveAsync(_objective);
        }
        else
        {
            _objective.ObjectiveText = ObjectiveEditor.Text;
            _objective.UpdatedOn = DateTime.Now;

            await _databaseService.UpdateObjectiveAsync(_objective);
        }

        await DisplayAlertAsync(
            "Success",
            "Career objective saved successfully.",
            "OK");

        await Shell.Current.GoToAsync("..");
    }
}

