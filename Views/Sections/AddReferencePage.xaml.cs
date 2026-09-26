using CVPilot.Database;
using CVPilot.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CVPilot.Views.Sections;

[QueryProperty(nameof(ResumeId), "ResumeId")]
[QueryProperty(nameof(ReferenceId), "ReferenceId")]
public partial class AddReferencePage : ContentPage
{
    private readonly DatabaseService _databaseService =
        IPlatformApplication.Current!
        .Services
        .GetRequiredService<DatabaseService>();

    public int ResumeId { get; set; }

    public int ReferenceId { get; set; }

    private Reference? _reference;

    public AddReferencePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (ReferenceId <= 0)
            return;

        Title = "Edit Reference";
        SaveButton.Text = "Update Reference";

        _reference = await _databaseService.GetReferenceAsync(ReferenceId);

        if (_reference == null)
            return;

        FullNameEntry.Text = _reference.FullName;
        DesignationEntry.Text = _reference.Designation;
        CompanyEntry.Text = _reference.CompanyName;
        EmailEntry.Text = _reference.Email;
        PhoneEntry.Text = _reference.PhoneNumber;
        RelationshipEntry.Text = _reference.Relationship;
    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FullNameEntry.Text))
        {
            await DisplayAlertAsync("Validation",
                "Please enter full name.",
                "OK");
            return;
        }

        if (_reference == null)
        {
            _reference = new Reference
            {
                ResumeId = ResumeId,
                CreatedOn = DateTime.Now
            };
        }

        _reference.FullName = FullNameEntry.Text.Trim();
        _reference.Designation = DesignationEntry.Text?.Trim() ?? "";
        _reference.CompanyName = CompanyEntry.Text?.Trim() ?? "";
        _reference.Email = EmailEntry.Text?.Trim() ?? "";
        _reference.PhoneNumber = PhoneEntry.Text?.Trim() ?? "";
        _reference.Relationship = RelationshipEntry.Text?.Trim() ?? "";
        _reference.UpdatedOn = DateTime.Now;

        if (ReferenceId == 0)
            await _databaseService.SaveReferenceAsync(_reference);
        else
            await _databaseService.UpdateReferenceAsync(_reference);

        await DisplayAlertAsync(
            "Success",
            "Reference saved successfully.",
            "OK");

        await Shell.Current.GoToAsync("..");
    }
}