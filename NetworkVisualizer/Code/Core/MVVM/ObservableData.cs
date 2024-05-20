using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NetworkVisualizer.Code.Core.MVVM;

public class ObservableData : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        if (PropertyChanged is null) return;
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}
