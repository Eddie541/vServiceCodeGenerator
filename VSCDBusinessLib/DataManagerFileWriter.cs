using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSCDBusinessLib {
    public class DataManagerFileWriter : ManagerIOBase {

        private DataManagerLibrary _dataLib;
        private string _dataManagerDirectory;
        public DataManagerFileWriter(DataManagerLibrary dataLib, string dataManagerDirectory) {
            _dataLib = dataLib;
            _dataManagerDirectory = dataManagerDirectory;

        }


        public void WriteDataManagerFiles() {

            try {
                DirectoryInfo di = new DirectoryInfo(_dataManagerDirectory);
                if (di.Exists) {
                    foreach (CodeFile codeFile in _dataLib) {
                        string path = Path.Combine(_dataManagerDirectory, codeFile.FileName);
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
