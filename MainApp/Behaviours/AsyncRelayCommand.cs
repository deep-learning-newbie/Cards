using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MainApp.Behaviours
{
    public class AsyncRelayCommand<T> : ICommand
    {
        #region attributes
        private static bool DefaultCanExecute(T parameter) => true;

        private Func<T, Task> _executeAsync;
        private Func<T, bool> _canExecuteAsync;
        private long _isExecuting;
        #endregion

        #region events
        private event EventHandler CanExecuteChangedInternal;
        public event EventHandler CanExecuteChanged
        {
            add
            {
                //CommandManager.RequerySuggested += value;
                CanExecuteChangedInternal += value;
            }

            remove
            {
                //CommandManager.RequerySuggested -= value;
                CanExecuteChangedInternal -= value;
            }
        }
        #endregion

        #region constructors
        public AsyncRelayCommand(Func<T, Task> execute) : this(execute, DefaultCanExecute)
        {
        }
        public AsyncRelayCommand(Func<T, Task> execute, Func<T, bool> canExecute)
        {
            _executeAsync = execute ?? throw new ArgumentNullException("execute");
            _canExecuteAsync = canExecute ?? throw new ArgumentNullException("canExecute");
        }
        #endregion

        #region ICommand implementation
        public bool CanExecute(object parameter)
        {
            if (Interlocked.Read(ref _isExecuting) != 0)
                return false;

            return _canExecuteAsync((T)parameter);
        }
        public async void Execute(object parameter)
        {
            Interlocked.Exchange(ref _isExecuting, 1);
            FireCanExecuteChanged();

            try
            {
                await _executeAsync((T)parameter);
            }
            finally
            {
                Interlocked.Exchange(ref _isExecuting, 0);
                FireCanExecuteChanged();
            }
        }
        #endregion

        #region public API
        public void FireCanExecuteChanged()
        {
            EventHandler handler = CanExecuteChangedInternal;
            if (handler != null)
            {
                //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
                handler.Invoke(this, EventArgs.Empty);
            }
        }
        public void Destroy()
        {
            _canExecuteAsync = _ => false;
            _executeAsync = _ => { return null; };
        }
        #endregion
    }

    public class AsyncRelayCommand : AsyncRelayCommand<object>
    {
        public AsyncRelayCommand(Func<object, Task> execute) : base(execute)
        {

        }

        public AsyncRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute) : base(execute, canExecute)
        {
        }
    }
}
