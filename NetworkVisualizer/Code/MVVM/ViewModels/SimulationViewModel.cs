using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using NetworkVisualizer.Code.Core.Data;
using NetworkVisualizer.Code.Core.MVVM.Commands;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace NetworkVisualizer.Code.MVVM.ViewModels;

public enum SimulationMenuCommand
{
    Start, Pause, Reset, Clear
}

public class SimulationViewModel : ViewModelBase
{
    private bool _isRunning = false;



    #region Properties

    private LineSeries<double> _lineSeries = new();
    public ISeries[] Series { get; set; }

    public Canvas MainCanvas { get; private set; }

    private int _sliderSpeed;
    public int SliderSpeed
    {
        get => _sliderSpeed;
        set
        {
            _sliderSpeed = value;
            OnPropertyChanged(nameof(SliderSpeed));
        }
    }

    private ICommand _menuCommand = null!;
    public ICommand MenuCommand => _menuCommand ??= new EnumCommandExtended<SimulationMenuCommand>(MenuHandler, CanBeExecutedMenuHandler);

    public ObservableCollection<NetworkAnalysisInfo> AnalysisInfos { get; set; } = new();

    #endregion


    public SimulationViewModel()
    {
        MainCanvas = new Canvas();
        Series = [_lineSeries];
        AnalysisInfos.Add(new NetworkAnalysisInfo { ParameterName = "Connected", ParameterValue = "False" });
        AnalysisInfos.Add(new NetworkAnalysisInfo { ParameterName = "All Edges", ParameterValue = "0" });
    }

    private void MenuHandler(SimulationMenuCommand command)
    {
        switch (command)
        {
            case SimulationMenuCommand.Start:
                _isRunning = true;
                break;
            case SimulationMenuCommand.Pause:
                _isRunning = false;
                break;
            case SimulationMenuCommand.Reset:
                _isRunning = false;
                break;
            case SimulationMenuCommand.Clear:
                _isRunning = false;
                break;
        }
    }
    private bool CanBeExecutedMenuHandler(SimulationMenuCommand command)
    {
        return command switch
        {
            SimulationMenuCommand.Start => !_isRunning,
            SimulationMenuCommand.Pause => _isRunning,
            SimulationMenuCommand.Reset => !_isRunning,
            SimulationMenuCommand.Clear => !_isRunning,
            _ => true,
        };
    }
}
