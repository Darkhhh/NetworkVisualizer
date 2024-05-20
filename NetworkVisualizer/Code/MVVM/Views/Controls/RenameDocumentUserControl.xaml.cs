using NetworkVisualizer.Code.MVVM.ViewModels;
using System.Windows.Controls;

namespace NetworkVisualizer.Code.MVVM.Views.Controls;
public partial class RenameDocumentUserControl : UserControl
{
    public string DocumentTitle { get; set; } = string.Empty;

    private readonly RenameDocumentViewModel _viewModel;

    public RenameDocumentUserControl(string oldDocumentTitle)
    {
        InitializeComponent();

        _viewModel = new RenameDocumentViewModel(this);
        DataContext = _viewModel;
        _viewModel.DocumentTitle = oldDocumentTitle;
    }
}


public class RenameDocumentViewModel(RenameDocumentUserControl parent) : ViewModelBase
{
    private string _title = string.Empty;

    public string DocumentTitle 
    {
        get => _title; 
        set
        {
            _title = value;
            parent.DocumentTitle = value;
        }
    }
}
