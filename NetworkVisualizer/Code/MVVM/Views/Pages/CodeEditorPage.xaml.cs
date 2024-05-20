using NetworkVisualizer.Code.MVVM.ViewModels;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui;

namespace NetworkVisualizer.Code.MVVM.Views.Pages;

public partial class CodeEditorPage : Page
{
    public CodeEditorPage(ISnackbarService snackbarService, IContentDialogService dialogService)
    {
        InitializeComponent();
        Loaded += (_, _) =>
        {
            webView21.Source = new Uri(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Assets\Monaco\index.html"));
        };
        DataContext = new CodeEditorViewModel(webView21, snackbarService, dialogService).Init();
    }

    private void ContextMenuClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Working!");
    }

    private async void GetTextFromEditor()
    {
        var value = await webView21.ExecuteScriptAsync("editor.getValue();");
        MessageBox.Show(value);
    }

    private async void SetTextToEditor(string text)
    {
        await webView21.ExecuteScriptAsync($"editor.setValue('{text}');");
    }
}
