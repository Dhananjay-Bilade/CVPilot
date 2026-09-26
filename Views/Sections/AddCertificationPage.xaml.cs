using CVPilot.Database;
using CVPilot.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CVPilot.Views.Sections;

[QueryProperty(nameof(ResumeId), "ResumeId")]
[QueryProperty(nameof(CertificationId), "CertificationId")]
public partial class AddCertificationPage : ContentPage
{
    private readonly DatabaseService _databaseService =
        IPlatformApplication.Current!
        .Services
        .GetRequiredService<DatabaseService>();

    public int ResumeId { get; set; }

    public int CertificationId { get; set; }

    private Certification? _certification;

    public AddCertificationPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (CertificationId <= 0)
            return;

        Title = "Edit Certification";

        SaveButton.Text = "Update Certification";

        _certification =
            await _databaseService.GetCertificationAsync(CertificationId);

        if (_certification == null)
            return;

        CertificationNameEntry.Text = _certification.CertificationName;
        OrganizationEntry.Text = _certification.IssuingOrganization;
        CredentialIdEntry.Text = _certification.CredentialId;
        CredentialUrlEntry.Text = _certification.CredentialUrl;

        IssueDatePicker.Date = _certification.IssueDate;

        NoExpiryCheckBox.IsChecked = _certification.DoesNotExpire;

        if (_certification.ExpiryDate.HasValue)
            ExpiryDatePicker.Date = _certification.ExpiryDate.Value;

        ExpiryDatePicker.IsEnabled = !_certification.DoesNotExpire;
        ExpiryDateLabel.IsVisible = !_certification.DoesNotExpire;
        ExpiryDatePicker.IsVisible = !_certification.DoesNotExpire;
    }

    private void NoExpiryCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        ExpiryDatePicker.IsEnabled = !e.Value;
        ExpiryDatePicker.IsVisible = !e.Value;
        ExpiryDateLabel.IsVisible = !e.Value;
    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CertificationNameEntry.Text))
        {
            await DisplayAlertAsync(
                "Validation",
                "Please enter certification name.",
                "OK");

            return;
        }

        if (_certification == null)
        {
            _certification = new Certification
            {
                ResumeId = ResumeId,
                CreatedOn = DateTime.Now
            };
        }

        _certification.CertificationName = CertificationNameEntry.Text ?? "";
        _certification.IssuingOrganization = OrganizationEntry.Text ?? "";
        _certification.CredentialId = CredentialIdEntry.Text ?? "";
        _certification.CredentialUrl = CredentialUrlEntry.Text ?? "";
        if (IssueDatePicker.Date.HasValue)
            _certification.IssueDate = IssueDatePicker.Date.Value;
        _certification.DoesNotExpire = NoExpiryCheckBox.IsChecked;
        _certification.ExpiryDate =
            NoExpiryCheckBox.IsChecked
                ? null
                : ExpiryDatePicker.Date;

        _certification.UpdatedOn = DateTime.Now;

        if (CertificationId == 0)
            await _databaseService.SaveCertificationAsync(_certification);
        else
            await _databaseService.UpdateCertificationAsync(_certification);

        await DisplayAlertAsync(
            "Success",
            "Certification saved successfully.",
            "OK");

        await Shell.Current.GoToAsync("..");
    }
}

