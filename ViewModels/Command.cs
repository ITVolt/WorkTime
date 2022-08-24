using System;
using System.Windows.Input;

namespace WorkTime.ViewModels
{

    public class Command : ICommand
    {
        private readonly Action action;
        private readonly Func<bool> predicate;

        public event EventHandler CanExecuteChanged;

        public Command(Action action)
        {
            this.action = action;
            this.predicate = () => true;
        }

        public Command(Action action, Func<bool> predicate)
        {
            this.action = action;
            this.predicate = predicate;
        }

        public bool CanExecute(object parameter)
        {
            return predicate();
        }

        public void Execute(object _)
        {
            action();
        }

        protected void OnCanExecutedChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }

    public class Command<T> : ICommand
    {

        private readonly Action<T> action;
        private readonly Func<bool> predicate;

        public event EventHandler CanExecuteChanged;


        public Command(Action<T> action)
        {
            this.action = action;
            this.predicate = () => true;
        }

        public Command(Action<T> action, Func<bool> predicate)
        {
            this.action = action;
            this.predicate = predicate;
        }

        public bool CanExecute(object parameter)
        {
            return predicate();
        }

        public void Execute(object parameter)
        {
            if(parameter is T typedParameter)
            {
                action.Invoke(typedParameter);
            }
        }

        protected void OnCanExecutedChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
