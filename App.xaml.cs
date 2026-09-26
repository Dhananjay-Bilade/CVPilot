using CVPilot.Database;

namespace CVPilot;

public partial class App : Application
{
    private readonly DatabaseService _databaseService;

    public App(DatabaseService databaseService)
    {
        InitializeComponent();

        _databaseService = databaseService;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Task.Run(async () =>
        {
            await _databaseService.InitAsync();
        });

        return new Window(new AppShell());
    }
}