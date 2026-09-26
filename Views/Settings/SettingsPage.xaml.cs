using Microsoft.Maui.Storage;

namespace CVPilot.Views.Settings;

public partial class SettingsPage : ContentPage
{
    private const string ThemePreferenceKey = "AppTheme";
    private const string DefaultCategoryPreferenceKey = "DefaultResumeCategory";

    public SettingsPage()
    {
        InitializeComponent();

        LoadSavedTheme();
        LoadSavedCategory();
    }

    private void LoadSavedTheme()
    {
        string savedTheme =
            Preferences.Get(
                ThemePreferenceKey,
                "Light");

        switch (savedTheme)
        {
            case "Dark":
                ThemePicker.SelectedIndex = 1;
                break;

            case "System":
                ThemePicker.SelectedIndex = 2;
                break;

            default:
                ThemePicker.SelectedIndex = 0;
                break;
        }
    }

    private void ThemePicker_SelectedIndexChanged(
        object sender,
        EventArgs e)
    {
        if (ThemePicker.SelectedIndex == -1)
            return;

        switch (ThemePicker.SelectedIndex)
        {
            case 0:
                Application.Current!.UserAppTheme =
                    AppTheme.Light;

                Preferences.Set(
                    ThemePreferenceKey,
                    "Light");
                break;

            case 1:
                Application.Current!.UserAppTheme =
                    AppTheme.Dark;

                Preferences.Set(
                    ThemePreferenceKey,
                    "Dark");
                break;

            case 2:
                Application.Current!.UserAppTheme =
                    AppTheme.Unspecified;

                Preferences.Set(
                    ThemePreferenceKey,
                    "System");
                break;
        }
    }

    private void LoadSavedCategory()
    {
        string savedCategory =
            Preferences.Get(
                DefaultCategoryPreferenceKey,
                "Professional");

        switch (savedCategory)
        {
            case "Fresher":
                DefaultCategoryPicker.SelectedIndex = 1;
                break;

            case "Student":
                DefaultCategoryPicker.SelectedIndex = 2;
                break;

            case "Academic":
                DefaultCategoryPicker.SelectedIndex = 3;
                break;

            default:
                DefaultCategoryPicker.SelectedIndex = 0;
                break;
        }
    }

    private void DefaultCategoryPicker_SelectedIndexChanged(
        object sender,
        EventArgs e)
    {
        if (DefaultCategoryPicker.SelectedIndex == -1)
            return;

        string selectedCategory =
            DefaultCategoryPicker.SelectedItem?.ToString()
            ?? "Professional";

        Preferences.Set(
            DefaultCategoryPreferenceKey,
            selectedCategory);
    }
}