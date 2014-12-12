using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UtilitiesLib;

namespace VSCDBusinessLib {
    public class ServiceDataFileManager : ManagerIOBase, IDisposable {        

        public DataFileCollection DataFiles { get; private set; }
        public ServiceInterfaceCollection ServiceInterfaces { get; private set; }
        
        private XMLBuilder _builder;
        private ServiceDataFileReader _fileReader;
        private CodeFileWriter _fileWriter;
        private string _schemaDirectory;
        private string _interfaceDefinitionFile;
        private string _sourceDirectory;
        private string _destinationDirectory;
        private bool disposed = false;
        

        public ServiceDataFileManager(string schemaDirectory, string interfaceDefinitionFile,
            string sourceDirectory, string destinationDirectory) {
                _schemaDirectory = schemaDirectory;
                _interfaceDefinitionFile = interfaceDefinitionFile;
                _sourceDirectory = sourceDirectory;
                _destinationDirectory = destinationDirectory;
                _builder = new XMLBuilder(); // dummy initialization
                

            
        }

        public void GenerateServiceData() {
            if (InitializeBuilder( _interfaceDefinitionFile, _schemaDirectory, ref _builder)) {
                IEnumerable<DataFile> dataFiles = from d in _builder.CurrentDocument.Descendants(XMLBuilder.GetElementXName("ServiceDataInterface", BusinessConstants.JmlfdcNamespace))
                                                  select new DataFile() {
                                                      DataFileName = ((string)d.Element(XMLBuilder.GetElementXName("DataFileName", BusinessConstants.JmlfdcNamespace))),
                                                      InterfaceName = ((string)d.Element(XMLBuilder.GetElementXName("InterfaceName", BusinessConstants.JmlfdcNamespace))),
                                                      ControllerKeyType = ((string)d.Element(XMLBuilder.GetElementXName("ControllerKeyType", BusinessConstants.JmlfdcNamespace)))
                                                  };
                DataFiles = new DataFileCollection(dataFiles);

                _fileReader = new ServiceDataFileReader(_sourceDirectory, DataFiles);
                _fileReader.SelectDataFiles();
                _fileReader.ReadDataFiles();
                ServiceInterfaces = _fileReader.GetServiceInterfaces();
                _fileWriter = new CodeFileWriter(_fileReader.InterfaceLib, _destinationDirectory);
                _fileWriter.WriteCodeFiles();
            }
        
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
                _builder = null;
                _fileReader = null;
                _fileWriter = null;
            }

            disposed = true;
        }
    }
}
