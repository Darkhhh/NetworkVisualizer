using System.Windows.Input;
using VisualizerLibrary.Utilities;

namespace NetworkVisualizer.Code.Core.MVVM.Commands;

public delegate bool CanBeExecutedWithEnum<TEnum>(TEnum value) where TEnum : struct, Enum;

public class EnumCommandExtended<TEnum> : ICommand where TEnum : struct, Enum
{
    private Action<TEnum> _action;
    private CanBeExecutedWithEnum<TEnum> _canExecute;

    public EnumCommandExtended(Action<TEnum> action, CanBeExecutedWithEnum<TEnum>? canExecute = null)
    {
        _action = action;
        _canExecute = canExecute is not null ? canExecute : (value) => true;
    }

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object? parameter)
    {
        if (parameter is null) return false;
        var str = parameter.ToString();
        if (string.IsNullOrEmpty(str)) return false;
        try
        {
            var e = Converter.GetEnum<TEnum>(str);
            return _canExecute.Invoke(e);
        }
        catch (Exception)
        {
            return false;
        }
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
