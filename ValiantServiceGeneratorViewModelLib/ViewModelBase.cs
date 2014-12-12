using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValiantServiceGeneratorViewModelLib {
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable {

        private bool disposed = false;
           
        private string _progressMessage;
        public string ProgressMessage {
            get { return _progressMessage; }
            set {
                if (HasPropertyChanged<string>(_progressMessage, value)) {
                    _progressMessage = value;
                    this.OnPropertyChanged("ProgressMessage");
                }
            }
        }



        private string _errorMessage;
        public string ErrorMessage {
            get { return _errorMessage; }
            set {
                if (HasPropertyChanged<string>(_errorMessage, value)) {
                    _errorMessage = value;
                    if (string.IsNullOrEmpty(_errorMessage) == false) {
                        HasError = true;
                    } else {
                        HasError = false;
                    }
                    this.OnPropertyChanged("ErrorMessage");
                }
            }
        }

        private bool _hasError = false;
        public bool HasError {
            get { return _hasError; }
            set {
                if (HasPropertyChanged<bool>(_hasError, value)) {
                    _hasError = value;
                    this.OnPropertyChanged("HasError");
                }
            }
        }


           
        private string _header;
        public string Header {
            get { return _header; }
            set {
                if (HasPropertyChanged<string>(_header, value)) {
                    _header = value;
                    this.OnPropertyChanged("Header");
                }
            }
        }

        protected void SetProgressMessage(string message) {
            ProgressMessage = message;
        }

        protected void SetErrorMessage(Exception ex) {
            this.ErrorMessage = ex.Message;
        }


        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual bool HasPropertyChanged<T>(T storage, T value) {

            if (object.Equals(storage, value)) {
                return false;
            } else {
                return true;

            }

        }

        protected void OnPropertyChanged(string propertyName) {
            var handler = this.PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ViewModelBase() {
            SetActions();
        }

        protected virtual void SetActions() {
        }



        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposed) {
                return;
            }
            if (disposing) {
                if (PropertyChanged != null) {
                    this.PropertyChanged = null;
                }
            }

            disposed = true;
        }
    }
}
