using CVPilot.Views.ATSChecker;
using CVPilot.Views.Settings;

namespace CVPilot.Views.Home;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    private async void CreateResume_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Views.Resume.CreateResumePage));
    }

    private async void MyResume_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Views.Resume.MyResumePage));
    }

    private async void Settings_Clicked(
    object sender,
    EventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(SettingsPage));
    }

    private async void ATSChecker_Clicked(
    object sender,
    EventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(ATSCheckerPage));
    }
}


