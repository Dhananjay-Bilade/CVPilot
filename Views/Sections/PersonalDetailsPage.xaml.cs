using CVPilot.Database;
using CVPilot.Models;
using Microsoft.Extensions.DependencyInjection;
using CVPilot.Views.Resume;

namespace CVPilot.Views.Sections;

[QueryProperty(nameof(ResumeId), "ResumeId")]
public partial class PersonalDetailsPage : ContentPage
{
    private readonly DatabaseService _databaseService;

    public int ResumeId { get; set; }

    private PersonalDetails? _personalDetails;

    public PersonalDetailsPage()
    {
        InitializeComponent();

        GenderPicker.SelectedIndex = 0;

        _databaseService =
            IPlatformApplication.Current!.Services.GetRequiredService<DatabaseService>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (ResumeId <= 0)
            return;

        _personalDetails = await _databaseService.GetPersonalDetailsAsync(ResumeId);

        if (_personalDetails == null)
            return;

        FullNameEntry.Text = _personalDetails.FullName;
        ProfessionalTitleEntry.Text = _personalDetails.ProfessionalTitle;
        EmailEntry.Text = _personalDetails.Email;
        PhoneEntry.Text = _personalDetails.PhoneNumber;
        AlternatePhoneEntry.Text = _personalDetails.AlternatePhone;

        // Date of Birth
        DobPicker.Date = _personalDetails.DateOfBirth == default
            ? DateTime.Today
            : _personalDetails.DateOfBirth;

        if (!string.IsNullOrWhiteSpace(_personalDetails.Gender))
            GenderPicker.SelectedItem = _personalDetails.Gender;

        NationalityEntry.Text = _personalDetails.Nationality;
        AddressEditor.Text = _personalDetails.Address;
        CityEntry.Text = _personalDetails.City;
        StateEntry.Text = _personalDetails.State;
        CountryEntry.Text = _personalDetails.Country;
        PincodeEntry.Text = _personalDetails.Pincode;

        LinkedInEntry.Text = _personalDetails.LinkedInUrl;
        GitHubEntry.Text = _personalDetails.GitHubUrl;
        PortfolioEntry.Text = _personalDetails.PortfolioUrl;

        SummaryEditor.Text = _personalDetails.ProfessionalSummary;
    }

    //private async void Save_Clicked(object sender, EventArgs e)
    //{
    //    if (string.IsNullOrWhiteSpace(FullNameEntry.Text))
    //    {
    //        await DisplayAlertAsync(
    //            "Validation",
    //            "Please enter Full Name.",
    //            "OK");

    //        return;
    //    }

    //    if (_personalDetails == null)
    //    {
    //        _personalDetails = new PersonalDetails
    //        {
    //            ResumeId = ResumeId,
    //            CreatedOn = DateTime.Now
    //        };
    //    }

    //    _personalDetails.FullName = FullNameEntry.Text ?? "";
    //    _personalDetails.ProfessionalTitle = ProfessionalTitleEntry.Text ?? "";
    //    _personalDetails.Email = EmailEntry.Text ?? "";
    //    _personalDetails.PhoneNumber = PhoneEntry.Text ?? "";
    //    _personalDetails.AlternatePhone = AlternatePhoneEntry.Text ?? "";

    //    // Date of Birth
    //    _personalDetails.DateOfBirth = DobPicker.Date ?? DateTime.Today;

    //    _personalDetails.Gender = GenderPicker.SelectedItem?.ToString() ?? "";
    //    _personalDetails.Nationality = NationalityEntry.Text ?? "";
    //    _personalDetails.Address = AddressEditor.Text ?? "";
    //    _personalDetails.City = CityEntry.Text ?? "";
    //    _personalDetails.State = StateEntry.Text ?? "";
    //    _personalDetails.Country = CountryEntry.Text ?? "";
    //    _personalDetails.Pincode = PincodeEntry.Text ?? "";

    //    _personalDetails.LinkedInUrl = LinkedInEntry.Text ?? "";
    //    _personalDetails.GitHubUrl = GitHubEntry.Text ?? "";
    //    _personalDetails.PortfolioUrl = PortfolioEntry.Text ?? "";
    //    _personalDetails.ProfessionalSummary = SummaryEditor.Text ?? "";

    //    _personalDetails.UpdatedOn = DateTime.Now;

    //    if (_personalDetails.Id == 0)
    //        await _databaseService.SavePersonalDetailsAsync(_personalDetails);
    //    else
    //        await _databaseService.UpdatePersonalDetailsAsync(_personalDetails);

    //    await DisplayAlertAsync(
    //        "Success",
    //        "Personal Details saved successfully.",
    //        "OK");

    //    await Shell.Current.GoToAsync(
    //$"{nameof(ResumeSectionsPage)}?ResumeId={ResumeId}");
    //}

    private async void Save_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FullNameEntry.Text))
        {
            await DisplayAlertAsync(
                "Validation",
                "Please enter Full Name.",
                "OK");

            return;
        }

        if (_personalDetails == null)
        {
            _personalDetails = new PersonalDetails
            {
                ResumeId = ResumeId,
                CreatedOn = DateTime.Now
            };
        }

        _personalDetails.FullName = FullNameEntry.Text ?? "";
        _personalDetails.ProfessionalTitle = ProfessionalTitleEntry.Text ?? "";
        _personalDetails.Email = EmailEntry.Text ?? "";
        _personalDetails.PhoneNumber = PhoneEntry.Text ?? "";
        _personalDetails.AlternatePhone = AlternatePhoneEntry.Text ?? "";

        // Date of Birth
        _personalDetails.DateOfBirth = DobPicker.Date ?? DateTime.Today;

        _personalDetails.Gender =
            GenderPicker.SelectedItem?.ToString() ?? "";

        _personalDetails.Nationality =
            NationalityEntry.Text ?? "";

        _personalDetails.Address =
            AddressEditor.Text ?? "";

        _personalDetails.City =
            CityEntry.Text ?? "";

        _personalDetails.State =
            StateEntry.Text ?? "";

        _personalDetails.Country =
            CountryEntry.Text ?? "";

        _personalDetails.Pincode =
            PincodeEntry.Text ?? "";

        _personalDetails.LinkedInUrl =
            LinkedInEntry.Text ?? "";

        _personalDetails.GitHubUrl =
            GitHubEntry.Text ?? "";

        _personalDetails.PortfolioUrl =
            PortfolioEntry.Text ?? "";

        _personalDetails.ProfessionalSummary =
            SummaryEditor.Text ?? "";

        _personalDetails.UpdatedOn = DateTime.Now;

        // Save new record
        if (_personalDetails.Id == 0)
        {
            await _databaseService.SavePersonalDetailsAsync(
                _personalDetails);
        }
        // Update existing record
        else
        {
            await _databaseService.UpdatePersonalDetailsAsync(
                _personalDetails);
        }

        await DisplayAlertAsync(
            "Success",
            "Personal Details saved successfully.",
            "OK");

        // Return to the existing Resume Sections page
        await Navigation.PopAsync();
    }
}

