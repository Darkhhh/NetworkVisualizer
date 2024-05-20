using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NetworkVisualizer.Code.Core.MVVM.Commands;

public class GenericCommand<T> : ICommand where T : class
{
    private Action<T> _action;
    private Func<bool> _canExecute;

    public GenericCommand(Action<T> action, Func<bool>? canExecute = null)
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
        try
        {
            _action(parameter as T);
        }
        catch (Exception)
        {
            return;
        }
    }
}
