using CVPilot.Database;
using CVPilot.Services;
using CommunityToolkit.Maui;
namespace CVPilot;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        //SyncFusion License
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
            "NxYtFisQPR08Cit/VkJ+XU9Gf1RLVGpAY1J0WGBYb1xzflBPallYT3RfQFtjQHxVd0JnWHpYcnRcR2tfVQ==");

        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<ResumeDataService>();
        builder.Services.AddSingleton<ATSAnalyzerService>();
        builder.Services.AddSingleton<JobDescriptionMatcherService>();

        return builder.Build();
    }
}