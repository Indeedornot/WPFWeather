using System;
using System.Windows.Input;

namespace WPFWeather.Commands;

public class RelayCommand : CommandBase {
    private readonly Action<object> execute;
    private readonly Func<object, bool>? canExecute;

    public RelayCommand(Action<object> execute, Func<object, bool>? canExecute = null) {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    public override bool CanExecute(object parameter) {
        return canExecute == null || (canExecute is not null && canExecute(parameter));
    }

    public override void Execute(object parameter) {
        execute(parameter);
    }
}
