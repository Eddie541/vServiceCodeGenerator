using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSCDBusinessLib {
    public class ServiceDataFileWriter {

        private InterfaceLibrary _interfaceLib;
        private string _writeDirectoryPath;

        public ServiceDataFileWriter(InterfaceLibrary interfaceLib, string interfaceDataDirectory) {
            _interfaceLib = interfaceLib;
            _writeDirectoryPath = interfaceDataDirectory;

        }

        public void WriteServiceDataFile() {
            try {
                DirectoryInfo di = new DirectoryInfo(_writeDirectoryPath);
                if (di.Exists) {
                    foreach (CodeFile codeFile in _interfaceLib) {
                        string path = Path.Combine(_writeDirectoryPath, codeFile.FileName);
                        using (FileStream fs = File.Create(path)) {
                            Byte[] data = new UTF8Encoding(true).GetBytes(codeFile.ToString());
                            fs.Write(data, 0, data.Length);
                        }

                    }
                }
            } catch (Exception ex) {

                throw ex;

            }

        }
    }
}
