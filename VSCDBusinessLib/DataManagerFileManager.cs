using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilitiesLib;

namespace VSCDBusinessLib {
    public class DataManagerFileManager : ManagerIOBase, IDisposable {

        private string _dataManagerXmlFile;
        private string _dataNamespace;
        private string _managerDirectory;
        private string _schemaDirectory;
        private XMLBuilder _builder;
        private bool disposed = false;

        public DataManagerFileManager(string schemaDirectory, string dataManagerXmlFile, string dataNamespace, string managerDirectory ) {
            _dataManagerXmlFile = dataManagerXmlFile;
            _dataNamespace = dataNamespace;
            _managerDirectory = managerDirectory;
            _schemaDirectory = schemaDirectory;
            _builder = new XMLBuilder();

        }


        public void GenerateDataManagerCode() {
            if (_dataManagerXmlFile.IsNullOrEmpty() == false) {
                if (InitializeBuilder(_dataManagerXmlFile, _schemaDirectory, ref _builder)) {
                    DataManagerFileReader dataMgrFileReader = new DataManagerFileReader(_builder, _dataNamespace);
                    dataMgrFileReader.ReadDocument();
                    CodeFileWriter fileWriter = new CodeFileWriter(dataMgrFileReader.DataManagerLib, _managerDirectory);
                    fileWriter.WriteCodeFiles();
                }
            } else {
                throw new ApplicationException("The Data Manager File has not been set");
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
            }

            disposed = true;
        }
    }
}
