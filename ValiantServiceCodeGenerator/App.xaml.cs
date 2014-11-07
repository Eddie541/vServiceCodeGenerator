using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ValiantServiceGeneratorViewModelLib;

namespace ValiantServiceCodeGenerator {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        private static Configuration _applicationConfiguration = null;
        private static Configuration ApplicationConfiguration { get { return _applicationConfiguration; } }
        public static MainViewModel MViewModel;

        protected override void OnStartup(StartupEventArgs e) {
            SetConfigurationManager();
            KeyValueConfigurationCollection kvce = ApplicationConfiguration.AppSettings.Settings;
            if (kvce != null) {
                MViewModel = new MainViewModel() {
                    ControllerDirectory = kvce["ControllerDirectory"].Value,
                    DestinationDirectory = kvce["ServiceInterfaceDirectory"].Value,
                    SourceDirectory = kvce["DataDirectory"].Value,
                    SchemaDirectory = kvce["SchemaDirectory"].Value,
                    InterfaceDefinitions = kvce["InterfaceDefinitions"].Value,
                    DataManagerDefinitions = kvce["DataManagerDefinitions"].Value,
                    DataManagerNamespace = kvce["DataManagerNamespace"].Value,
                    ManagerDirectory = kvce["DataManagerDirectory"].Value,
                    ServiceNamespace = kvce["ServiceNamespace"].Value
                };
            }
       
            base.OnStartup(e);
        }


        private void SetConfigurationManager() {
            string codeBase = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string assemblyPathFilename = System.IO.Path.Combine(path, "ValiantServiceCodeGenerator.exe");
            _applicationConfiguration = ConfigurationManager.OpenExeConfiguration(assemblyPathFilename);

        }

    }
}
