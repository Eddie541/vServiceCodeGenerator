using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ValiantServiceGeneratorViewModelLib {
    public abstract class SynchCommandBase<T> : ICommand {

        protected Action<T> _action;
        protected Func<T,bool> _validation;
        public SynchCommandBase(Action<T> action, Func<T,bool> validation) {
            _action = action;
            _validation = validation;

        }
        
        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

     
    }
}
