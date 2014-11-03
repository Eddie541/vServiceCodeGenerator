using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using ValiantServiceGeneratorViewModelLib;

namespace ValiantServiceCodeGenerator {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        MainViewModel viewModel { get { return this.DataContext as MainViewModel; } }

        public MainWindow() {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            this.DataContext = App.MViewModel;
            viewModel.CloseForm += delegate { this.Close(); };
        }       

        private void BrowseInterfaceDefinitionButton_Click(object sender, RoutedEventArgs e) {
            SetSelectedFileData(MainViewModel.Interface);  
        }      

        private void BrowseSourceButton_Click(object sender, RoutedEventArgs e) {
            SetSelectedDirectoryData(MainViewModel.Source);
        }

        private void BrowseDestinationButton_Click(object sender, RoutedEventArgs e) {
            SetSelectedDirectoryData(MainViewModel.Destination);
        }

        private void BrowseControllerButton_Click(object sender, RoutedEventArgs e) {
            SetSelectedDirectoryData(MainViewModel.Controller);
        }

        private void BrowseSchemaDirectoryButton_Click(object sender, RoutedEventArgs e) {
            SetSelectedDirectoryData(MainViewModel.Schema);
        }

        private void BrowseDataManagerDefinitionButton_Click(object sender, RoutedEventArgs e) {
            SetSelectedDirectoryData(MainViewModel.DataManager);
        }

        private void BrowseDataManagerDirectoryButton_Click(object sender, RoutedEventArgs e) {
            SetSelectedDirectoryData(MainViewModel.DataManagerDirectory);
        }


        private void SetSelectedFileData(string fileType) {

            CommonOpenFileDialog cofd = new CommonOpenFileDialog();
            cofd.Title = string.Format("Select an {0} file", fileType);
            cofd.IsFolderPicker = false;
           // cofd.InitialDirectory = "";  //todo get from solution file
            cofd.AddToMostRecentlyUsedList = true;
            cofd.AllowNonFileSystemItems = false;
            cofd.EnsurePathExists = true;
            cofd.EnsureReadOnly = false;
            cofd.Multiselect = false;
            cofd.ShowPlacesList = true;
            if (cofd.ShowDialog() == CommonFileDialogResult.Ok) {
                SelectedFilePath sFilePath = new SelectedFilePath() {
                    FilePath = cofd.FileName,
                    FileType = fileType
                };
                if (viewModel != null) {
                    viewModel.SetFileSelected(sFilePath);
                }
            }

        }

        private void SetSelectedDirectoryData(string directoryType) {
            CommonOpenFileDialog cofd = new CommonOpenFileDialog();
            cofd.Title = string.Format("Select a {0} directory", directoryType);
            cofd.IsFolderPicker = true;
            //cofd.InitialDirectory = "";  //todo get from solution file
            cofd.AddToMostRecentlyUsedList = true;
            cofd.AllowNonFileSystemItems = false;
            cofd.EnsurePathExists = true;
            cofd.EnsureReadOnly = false;
            cofd.Multiselect = false;
            cofd.ShowPlacesList = true;

            if (cofd.ShowDialog() == CommonFileDialogResult.Ok) {
                SelectedFilePath sFilePath = new SelectedFilePath() {
                    FilePath = cofd.FileName,
                    FileType = directoryType
                };
                if (viewModel != null) {
                    viewModel.SetFileSelected(sFilePath);
                }
            }

        }

      

    }
}
