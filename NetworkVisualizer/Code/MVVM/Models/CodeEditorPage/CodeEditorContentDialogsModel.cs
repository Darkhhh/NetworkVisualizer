using Microsoft.Web.WebView2.Wpf;
using NetworkVisualizer.Code.Core.ContentDialogs;
using NetworkVisualizer.Code.Core.Data;
using NetworkVisualizer.Code.Core.Extensions;
using NetworkVisualizer.Code.MVVM.Views.Controls;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NetworkVisualizer.Code.MVVM.Models.CodeEditorPage;

public class CodeEditorContentDialogsModel(WebView2 editorWrap, IContentDialogService dialogService)
{
    public void CreateRenameContentDialog(CodeDocument selectedDocument)
    {
        editorWrap.Visibility = System.Windows.Visibility.Hidden;

        var dialog = new ContentDialogWrap()
        {
            Dialog = new ContentDialog
            {
                Title = "Rename File",
                Content = new RenameDocumentUserControl(selectedDocument.Title),
                SnapsToDevicePixels = true,
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
            },
            OnSuccessClose = (sender, _) => selectedDocument.Title = ((RenameDocumentUserControl)sender.Content).DocumentTitle,
            OnClose = (_, _) => editorWrap.Visibility = System.Windows.Visibility.Visible,
        };
        dialog.ShowAsync(dialogService);
    }


    public void CreateFileDeleteContentDialog(ObservableCollection<CodeDocument> docs, CodeDocument selectedDocument)
    {
        editorWrap.Visibility = System.Windows.Visibility.Hidden;

        var dialog = new ContentDialogWrap()
        {
            Dialog = new ContentDialog
            {
                Title = "Delete File",
                Content = new MessageDialogUserControl($"Delete file {selectedDocument.Title}? This can't be undone."),
                SnapsToDevicePixels = true,
                PrimaryButtonText = "Apply",
                CloseButtonText = "Cancel",
            },
            OnSuccessClose = (sender, _) =>
            {
                docs.Remove(selectedDocument);
                editorWrap.SetTextAsync(string.Empty);
            },
            OnClose = (_, _) => editorWrap.Visibility = System.Windows.Visibility.Visible,
        };
        dialog.ShowAsync(dialogService);
    }


    public void CreateAllFilesDeleteContentDialog(ObservableCollection<CodeDocument> docs)
    {
        editorWrap.Visibility = System.Windows.Visibility.Hidden;

        var dialog = new ContentDialogWrap()
        {
            Dialog = new ContentDialog
            {
                Title = "Delete Files",
                Content = new MessageDialogUserControl($"Delete all files? This can't be undone."),
                SnapsToDevicePixels = true,
                PrimaryButtonText = "Apply",
                CloseButtonText = "Cancel",
            },
            OnSuccessClose = (sender, _) =>
            {
                docs.Clear();
                editorWrap.SetTextAsync(string.Empty);
            },
            OnClose = (_, _) => editorWrap.Visibility = System.Windows.Visibility.Visible,
        };
        dialog.ShowAsync(dialogService);
    }
}
