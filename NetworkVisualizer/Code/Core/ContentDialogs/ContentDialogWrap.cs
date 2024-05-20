using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NetworkVisualizer.Code.Core.ContentDialogs;

public class ContentDialogWrap
{
    public ContentDialog? Dialog { get; set; }

    public Action<ContentDialog, ContentDialogClosedEventArgs>? OnClose { get; set; } = null;
    public Action<ContentDialog, ContentDialogClosedEventArgs>? OnSuccessClose { get; set; } = null;
    public Action<ContentDialog, ContentDialogClosedEventArgs>? OnCancelClose { get; set; } = null;
    public Action<ContentDialog, ContentDialogClosedEventArgs>? OnSecondaryClose { get; set; } = null;


    public ContentDialogWrap() { }
    public ContentDialogWrap(ContentDialog dialog) => Dialog = dialog;




    public async void ShowAsync(IContentDialogService dialogService)
    {
        if (Dialog is null) return;
        Dialog.Closed += DialogClosed;
        await dialogService.ShowAsync(Dialog, new CancellationToken());
    }

    private void DialogClosed(ContentDialog sender, ContentDialogClosedEventArgs args)
    {
        sender.Closed -= DialogClosed;
        OnClose?.Invoke(sender, args);
        switch (args.Result)
        {
            case ContentDialogResult.None:
                OnCancelClose?.Invoke(sender, args);
                break;
            case ContentDialogResult.Primary:
                OnSuccessClose?.Invoke(sender, args);
                break;
            case ContentDialogResult.Secondary:
                OnSecondaryClose?.Invoke(sender, args);
                break;
        }
    }
}
