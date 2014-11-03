using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValiantServiceGeneratorViewModelLib {
    public class SynchCommand<T> : SynchCommandBase<T>  where T : class {

        public SynchCommand(Action<T> action, Func<T,bool> validation)
            : base(action, validation) {

        }


        public override bool CanExecute(object parameter) {
            if (parameter is T) {
                return _validation.Invoke(parameter as T);
            } else {
                return false;
            }
        }

        public override void Execute(object parameter) {
            _action.Invoke(parameter as T);           
        }
    }
}
