using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSCDBusinessLib {
    public class CodeFileWriter {

        private List<CodeFile> _codeFileLib;
        private string _writeDirectoryPath;

        public CodeFileWriter(List<CodeFile> codeFiles, string directoryPath) {
            _codeFileLib = codeFiles;
            _writeDirectoryPath = directoryPath;

        }

        public void WriteCodeFiles() {
            try {
                DirectoryInfo di = new DirectoryInfo(_writeDirectoryPath);
                if (di.Exists) {
                    foreach (CodeFile codeFile in _codeFileLib) {
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
