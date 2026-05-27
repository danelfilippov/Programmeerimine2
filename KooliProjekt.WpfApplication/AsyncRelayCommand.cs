using System.Windows.Input;

namespace KooliProjekt.WpfApplication
{
    public class AsyncRelayCommand<T> : ICommand
    {
        readonly Func<T, Task> _execute = null;
        readonly Predicate<T> _canExecute = null;

        public AsyncRelayCommand(Func<T, Task> execute) : this(execute, null)
        {
        }

        public AsyncRelayCommand(Func<T, Task> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public async void Execute(object parameter)
        {
            await _execute((T)parameter);
        }
    }
}
