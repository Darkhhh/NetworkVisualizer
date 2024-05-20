using NetworkVisualizer.Code.MVVM.ViewModels;
using System.Windows.Controls;

namespace NetworkVisualizer.Code.MVVM.Views.Pages;

public partial class SimulationPage : Page
{
    public SimulationPage()
    {
        InitializeComponent();

        DataContext = new SimulationViewModel();
    }
}
