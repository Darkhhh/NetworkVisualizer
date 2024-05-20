using NetworkVisualizer.Code.Core.MVVM;

namespace NetworkVisualizer.Code.Core.Data;

public enum CodeDocumentType
{
    Empty, Network, Analyser, Extension
}

public class CodeDocument : ObservableData
{
    public CodeDocumentType DocumentType { get; set; } = CodeDocumentType.Empty;

    private string _title = string.Empty;
    public string Title 
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }
    
    private string _code = string.Empty;
    public string Code
    {
        get => _code;
        set
        {
            _code = value;
            OnPropertyChanged();
        }
    }
}
