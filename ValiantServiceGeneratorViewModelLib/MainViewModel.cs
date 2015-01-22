using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VSCDBusinessLib;

namespace ValiantServiceGeneratorViewModelLib
{
    public class MainViewModel : ViewModelBase  {

        BackgroundWorker bg;

        public const string Source = "Source";
        public const string Destination = "Destination";
        public const string Controller = "Controller";
        public const string Interface = "Interface";
        public const string Schema = "Schema";
        public const string DataManager = "Data Manager Definition";
        public const string BusinessNamespace = "Business Namespace";
        public const string DataManagerDirectory = "Data Manager Directory";

        public event EventHandler CloseForm;

        private bool _complete = false;
        public bool Complete {
            get { return _complete; }
            set {
                if (HasPropertyChanged<bool>(_complete, value)) {
                    _complete = value;                    
                    this.OnPropertyChanged("Complete");
                    if (_complete) {
                        CommandManager.InvalidateRequerySuggested();
                    }
                }
            }

        }

        private string _sourceDirectory;
        public string SourceDirectory {
            get { return _sourceDirectory; }
            set {
                if (HasPropertyChanged<string>(_sourceDirectory, value)) {
                    _sourceDirectory = value;
                    this.OnPropertyChanged("SourceDirectory");
                }
            }
        }

        private string _destinationDirectory;
        public string DestinationDirectory {
            get { return _destinationDirectory; }
            set {
                if (HasPropertyChanged<string>(_destinationDirectory, value)) {
                    _destinationDirectory = value;
                    this.OnPropertyChanged("DestinationDirectory");
                }
            }
        }

        private string _controllerDirectory;
        public string ControllerDirectory {
            get { return _controllerDirectory; }
            set {
                if (HasPropertyChanged<string>(_controllerDirectory, value)) {
                    _controllerDirectory = value;
                    this.OnPropertyChanged("ControllerDirectory");
                }
            }
        }

        private string _interfaceDefinitions;
        public string InterfaceDefinitions  {
            get { return _interfaceDefinitions; }
            set {
                if (HasPropertyChanged<string>(_interfaceDefinitions, value)) {
                    _interfaceDefinitions = value;
                    this.OnPropertyChanged("InterfaceDefinitions");
                }
            }
        }

        private string _schemaDirectory;
        public string SchemaDirectory {
            get { return _schemaDirectory; }
            set {
                if (HasPropertyChanged<string>(_schemaDirectory, value)) {
                    _schemaDirectory = value;
                    this.OnPropertyChanged("SchemaDirectory");
                }
            }
        }

        private string _dataManagerDefinitions;
        public string DataManagerDefinitions {
            get { return _dataManagerDefinitions; }
            set {
                if (HasPropertyChanged<string>(_dataManagerDefinitions, value)) {
                    _dataManagerDefinitions = value;
                    this.OnPropertyChanged("DataManagerDefinitions");
                }
            }
        }

        private string _dataManagerNamespace;
        public string DataManagerNamespace {
            get { return _dataManagerNamespace; }
            set {
                if (HasPropertyChanged<string>(_dataManagerNamespace, value)) {
                    _dataManagerNamespace = value;
                    this.OnPropertyChanged("DataManagerNamespace");
                }
            }
        }

        private string _serviceNamespace;
        public string ServiceNamespace {
            get { return _serviceNamespace; }
            set {
                if (HasPropertyChanged<string>(_serviceNamespace, value)) {
                    _serviceNamespace = value;
                    this.OnPropertyChanged("ServiceNamespace");
                }
            }
        }

        private string _managerDirectory;
        public string ManagerDirectory {
            get { return _managerDirectory; }
            set {
                if (HasPropertyChanged<string>(_managerDirectory, value)) {
                    _managerDirectory = value;
                    this.OnPropertyChanged("ManagerDirectory");
                }
            }
        }

        private string _suffix;
        public string Suffix {
            get { return _suffix; }
            set {
                if (HasPropertyChanged<string>(_suffix, value)) {
                    _suffix = value;
                    this.OnPropertyChanged("Suffix");
                }
            }
        }

        private bool _showProgress = false;
        public bool ShowProgress {
            get { return _showProgress; }
            set {
                if (HasPropertyChanged<bool>(_showProgress, value)) {
                    _showProgress = value;
                    this.OnPropertyChanged("ShowProgress");
                }
            }
        }

        public SynchCommand<MainViewModel> StartCommand { get; set; }
        public SynchCommand<MainViewModel> CancelCommand { get; set; }
        public SynchCommand<MainViewModel> QuitCommand { get; set; }

        public MainViewModel() {
            StartCommand = new SynchCommand<MainViewModel>(StartGenerator, CanExecute);
            CancelCommand = new SynchCommand<MainViewModel>(CancelGenerator, CanCancelGenerator);
            QuitCommand = new SynchCommand<MainViewModel>(Quit, CanQuit);         

        }

        protected void OnCloseForm() {
            if (this.CloseForm != null) {
                CloseForm(this, EventArgs.Empty);
            }

        }

        private void StartGenerator(MainViewModel vm) {
            ShowProgress = true;
            bg = new BackgroundWorker();
            bg.WorkerReportsProgress = true;
            bg.WorkerSupportsCancellation = true;
            bg.DoWork += bg_DoWork;
            bg.RunWorkerCompleted += bg_RunWorkerCompleted;
            bg.RunWorkerAsync(vm);
        }

        private bool CanExecute(MainViewModel vm) {

            if (vm != null && Directory.Exists(vm.ControllerDirectory) &&
                Directory.Exists(vm.DestinationDirectory) &&
                Directory.Exists(vm.SchemaDirectory) &&
                Directory.Exists(vm.SourceDirectory) &&
                File.Exists(vm.InterfaceDefinitions)) {
                return true;
            } else {
                return false;
            }
        }

        private void CancelGenerator(MainViewModel vm) {
            bg.CancelAsync();
        }

        private bool CanCancelGenerator(MainViewModel vm) {
            if (bg != null && bg.IsBusy) {
                return true;
            } else if (bg != null && bg.CancellationPending) {
                return false;
            } else {
                return false;                    
            }
        }

        private void Quit(MainViewModel vm) {
            vm.OnCloseForm();

        }

        private bool CanQuit(MainViewModel vm) {
            
            if (bg == null) { 
                return true;
            } else if (bg != null && bg.IsBusy == false && bg.CancellationPending == false) {
                return true;
            } else if (Complete) {
                return true;
            } else {
                return false;
            }
        }


        public void SetFileSelected(SelectedFilePath selectedFile) {
            switch (selectedFile.FileType) {
                case Controller:
                    ControllerDirectory = selectedFile.FilePath;
                    break;
                case Interface:
                    InterfaceDefinitions = selectedFile.FilePath;
                    break;
                case Source:
                    SourceDirectory = selectedFile.FilePath;
                    break;
                case Destination:
                    DestinationDirectory = selectedFile.FilePath;
                    break;
                case Schema:
                    SchemaDirectory = selectedFile.FilePath;
                    break;
                case DataManager:
                    DataManagerDefinitions = selectedFile.FilePath;
                    break;
                case BusinessNamespace:
                    DataManagerNamespace = selectedFile.FilePath;
                    break;
                case DataManagerDirectory:
                    ManagerDirectory = selectedFile.FilePath;
                    break;

            }
        }

        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            ShowProgress = false;
            Complete = true;
        }

        //void bg_ProgressChanged(object sender, ProgressChangedEventArgs e) {
        //    throw new NotImplementedException();
        //}

        void bg_DoWork(object sender, DoWorkEventArgs e) {
            Complete = false;
            MainViewModel mvm = e.Argument as MainViewModel;
            ServiceDataFileManager sdfm = null;
            DataManagerFileManager dmfm = null;
            DataControllerFileManager dcfm = null;
            DataFileCollection datafiles = null;
            ServiceInterfaceCollection serviceInterfaceCollection = null;
            if (mvm != null) {
                using (sdfm = new ServiceDataFileManager(mvm.SchemaDirectory, mvm.InterfaceDefinitions, mvm.SourceDirectory, mvm.DestinationDirectory)) {
                    sdfm.GenerateServiceData();
                    datafiles = sdfm.DataFiles;
                    serviceInterfaceCollection = sdfm.ServiceInterfaces;
                }

                if (datafiles != null) {
                    using (dcfm = new DataControllerFileManager(datafiles, ControllerDirectory, ServiceNamespace, serviceInterfaceCollection)) {
                        dcfm.GenerateControllers();
                    }
                }

                using (dmfm = new DataManagerFileManager(mvm.SchemaDirectory, mvm.DataManagerDefinitions, mvm.DataManagerNamespace, mvm.ManagerDirectory)) {
                    dmfm.GenerateDataManagerCode();
                }
            }
        }

       



    }

    public class SelectedFilePath {
        public string FileType;
        public string FilePath;

    }
}
