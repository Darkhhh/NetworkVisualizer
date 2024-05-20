using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetworkVisualizer.Code.Core;
using NetworkVisualizer.Code.MVVM.Views.Pages;
using System.Windows;
using Wpf.Ui;

namespace NetworkVisualizer;

public partial class App : Application
{
    public static IHost AppHost { get; private set; } = null!;

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
        .ConfigureServices((hostContext, services) =>
        {
            // MainWindow
            services.AddSingleton<MainWindow>();

            // ExecutionServices
            services
            .AddSingleton<ISnackbarService, SnackbarService>()
            .AddSingleton<IContentDialogService, ContentDialogService>()
            .AddSingleton<PageService>();

            // Pages
            services
            .AddSingleton<SimulationPage>()
            .AddSingleton<CodeEditorPage>();
        })
        .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost.StartAsync();

        var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
        startupForm.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost.StopAsync();

        base.OnExit(e);
    }

    public static T GetService<T>() where T : class
    {
        return AppHost.Services.GetRequiredService<T>();
    }
}
