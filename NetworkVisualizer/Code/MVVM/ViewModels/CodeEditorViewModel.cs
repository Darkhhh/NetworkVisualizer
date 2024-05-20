using Microsoft.Web.WebView2.Wpf;
using NetworkVisualizer.Code.Core.Data;
using NetworkVisualizer.Code.Core.Extensions;
using NetworkVisualizer.Code.Core.MVVM.Commands;
using NetworkVisualizer.Code.MVVM.Models.CodeEditorPage;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NetworkVisualizer.Code.MVVM.ViewModels;

public enum EditorCommandsEnum
{
    NewNetwork, NewAnalyser, NewEmpty, BuildAll, DeleteAll
}

public enum EditorContextMenuCommandsEnum
{
    Delete, Rename
}

public class CodeEditorViewModel(WebView2 editorWrap, ISnackbarService snackbar, IContentDialogService dialogService) : ViewModelBase
{
    #region Properties

    private ICommand _menuCommand = null!, _listBoxContextMenu = null!;
    public ICommand MenuCommand => _menuCommand ??= new EnumCommand<EditorCommandsEnum>(MenuCommandHandler, () => true);
    public ICommand ListBoxCommand => _listBoxContextMenu ??= new EnumCommand<EditorContextMenuCommandsEnum>(ContextMenuHandler, () => true);

    public ObservableCollection<CodeDocument> Docs { get; set; } = new();

    private CodeDocument _selectedDocument = null!;
    public CodeDocument SelectedDocument
    {
        get => _selectedDocument;
        set
        {
            _selectedDocument = value;
            if (_selectedDocument != null) editorWrap.SetTextAsync(_selectedDocument.Code);
        }
    }

    #endregion


    #region Models

    private readonly CodeEditorContentDialogsModel _contentDialogs = new(editorWrap, dialogService);
    private readonly CodeEditorDocumentsHandler _documents = new();

    #endregion


    public CodeEditorViewModel Init()
    {
        return this;
    }


    private void MenuCommandHandler(EditorCommandsEnum command)
    {
        snackbar.Show("File Creation", "New Network File Created!", ControlAppearance.Info, new SymbolIcon(SymbolRegular.Fluent24), TimeSpan.FromSeconds(3));

        switch (command)
        {
            case EditorCommandsEnum.NewNetwork:
                _documents.AddNewNetwork(Docs);
                break;
            case EditorCommandsEnum.NewAnalyser:
                _documents.AddNewAnalyserDocument(Docs);
                break;
            case EditorCommandsEnum.NewEmpty:
                _documents.AddNewEmptyDocument(Docs);
                break;
            case EditorCommandsEnum.BuildAll:
                throw new NotImplementedException();
                break;
            case EditorCommandsEnum.DeleteAll:
                _contentDialogs.CreateAllFilesDeleteContentDialog(Docs);
                break;           
        }
    }

    private void ContextMenuHandler(EditorContextMenuCommandsEnum command)
    {
        switch (command)
        {
            case EditorContextMenuCommandsEnum.Delete:
                _contentDialogs.CreateFileDeleteContentDialog(Docs, SelectedDocument);
                break;
            case EditorContextMenuCommandsEnum.Rename:
                _contentDialogs.CreateRenameContentDialog(SelectedDocument);
                break;
        }       
    }    
}
