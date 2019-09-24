using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExamClient.Resources
{
    public class MVVMCommand : ICommand
    {

        public MVVMCommand(Action<object> executeAction)
            : this(executeAction, null)
        {
        }

        public MVVMCommand(Action<object> executeAction,
                                Predicate<object> canExecute)
        {
            if (executeAction == null)
                throw new ArgumentNullException("executeAction");

            _executeAction = executeAction;
            _canExecute = canExecute;
        }

        private readonly Predicate<object> _canExecute;
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null) return true;

            return _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;
        public void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        private readonly Action<object> _executeAction;
        public void Execute(object parameter)
        {
            _executeAction(parameter);
        }
    }
}
