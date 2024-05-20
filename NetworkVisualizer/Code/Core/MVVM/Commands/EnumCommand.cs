using System.Windows.Input;
using VisualizerLibrary.Utilities;

namespace NetworkVisualizer.Code.Core.MVVM.Commands;

public class EnumCommand<TEnum> : ICommand where TEnum : struct, Enum
{
    private Action<TEnum> _action;
    private Func<bool> _canExecute;

    public EnumCommand(Action<TEnum> action, Func<bool>? canExecute = null)
    {
        _action = action;
        _canExecute = canExecute is not null ? canExecute : () => true;
    }

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute.Invoke();
    }

    public void Execute(object? parameter)
    {
        if (parameter is null) return;
        var str = parameter.ToString();
        if (string.IsNullOrEmpty(str)) return;
        try
        {
            var e = Converter.GetEnum<TEnum>(str);
            _action(e);
        }
        catch (Exception)
        {
            return;
        }
    }
}

