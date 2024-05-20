using NetworkVisualizer.Code.Core;
using NetworkVisualizer.Code.MVVM.Views.Pages;
using System.Windows;
using Wpf.Ui;

namespace NetworkVisualizer;

public partial class MainWindow : Window
{
    private readonly PageService _pageService;

    public MainWindow(PageService pageService)
    {
        InitializeComponent();

        Loaded += (_, _) => RootNavigation.IsPaneOpen = false;

        App.GetService<ISnackbarService>().SetSnackbarPresenter(SnackbarPresenterItem);
        App.GetService<IContentDialogService>().SetContentPresenter(ContentDialogPresenterItem);
        
        _pageService = pageService;
        RootNavigation.SetPageService(
            _pageService
            .Register(App.GetService<SimulationPage>())
            .Register(App.GetService<CodeEditorPage>()));
    }
}

