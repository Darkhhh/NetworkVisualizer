using System.Windows.Controls;

namespace NetworkVisualizer.Code.MVVM.Views.Controls;
public partial class MessageDialogUserControl : UserControl
{
    public MessageDialogUserControl(string message)
    {
        InitializeComponent();
        Message.Text = message;
    }
}
