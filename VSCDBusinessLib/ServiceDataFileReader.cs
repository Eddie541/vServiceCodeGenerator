using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VSCDBusinessLib {
    public class ServiceDataFileReader {

        private const string NullableIntValue = "Nullable<int>";
        private const string NullableDecimalValue = "Nullable<decimal>";
        private const string NullableFloatValue = "Nullable<float>";
        private const string NullableDoubleValue = "Nullable<double>";
        private const string NullableLongValue = "Nullable<long>";
        private const string NullableShortValue = "Nullable<short>";
        private const string NullableBoolValue = "Nullable<bool>";
        private const string NullableDateTimeValue = "Nullable<System.DateTime>";
        private const string NullableCharValue = "Nullable<char>";
        private const string StringValue = "string";
        private const string IntValue = "int";
        private const string DecimalValue = "decimal";
        private const string LongValue = "long";
        private const string ShortValue = "short";
        private const string FloatValue = "float";
        private const string BoolValue = "bool";
        private const string ObjectValue = "object";
        private const string CharValue = "char";
        private const string ByteValue = "byte";
        private const string DateValue = "System.DateTime";
                
        
        private List<string> PropertyTypes;
        private DataFileCollection _dataFiles;
        private string _dataDirectory = "";
        public List<FileData> SelectedFiles { get; private set; }
        public InterfaceLibrary InterfaceLib { get; private set; }
        private List<UsingStatement> usings; 


        public ServiceDataFileReader(string dataDirectory, DataFileCollection dataFiles) {
            _dataFiles = dataFiles;
            _dataDirectory = dataDirectory;
            SelectedFiles = new List<FileData>();
            InterfaceLib = new InterfaceLibrary();
            usings = new List<UsingStatement>();
            
            SetPropertyTypes();
            SetUsings();

        }

        private void SetPropertyTypes() {
            PropertyTypes = new List<string>();
            string [] pTypes = new string [] {NullableIntValue, NullableBoolValue, NullableCharValue, 
                NullableDateTimeValue, NullableDecimalValue, NullableDoubleValue, NullableFloatValue, 
                NullableLongValue, NullableShortValue, StringValue, IntValue, DecimalValue, LongValue, 
                ShortValue, FloatValue, BoolValue, ObjectValue, CharValue, ByteValue, DateValue};
            PropertyTypes.AddRange(pTypes);

        }

        private void SetUsings() {
            usings.Add(new UsingStatement() {
                Assembly = "System"
            });
        }

        public void SelectDataFiles() {
            DirectoryInfo di = new DirectoryInfo(_dataDirectory);
            foreach (FileInfo fi in di.GetFiles()) {
                DataFile df = _dataFiles.Where(d => d.DataFileName.Equals(fi.Name)).FirstOrDefault();
                if (df != null) {
                    FileData data = new FileData() {
                        FInfo = fi,
                        Data = df
                    };
                    SelectedFiles.Add(data);
                }               

            }
        }

        public void ReadDataFiles() {
            foreach (FileData fi in SelectedFiles) {
                ReadData(fi);
            }
        }

        private void ReadData(FileData fileData) {
            
            IEnumerable<string> lines = File.ReadAllLines(fileData.FInfo.FullName);
            ServiceInterface si = new ServiceInterface(fileData.Data.InterfaceName);
            CodeFile codeFile = new CodeFile(usings, "ServiceDataInterfaceLib", si);
            foreach (string line in lines) {
                if (line.StartsWith("//") == false && string.IsNullOrEmpty(line) == false && string.IsNullOrWhiteSpace(line) == false) {
                    ServiceProperty property = SetPropertyDefinition(line);
                    if (property != null) {
                        si.AddServiceProperty(property);
                    }                    
                }
            }
            InterfaceLib.Add(codeFile);

        }

        private ServiceProperty SetPropertyDefinition(string codeLine) {
            ServiceProperty property = null;
            bool isPropertyDefinition = false;
            string c = codeLine.Trim();
            string [] words = c.Split(new [] {' '});
            if (words.Length >= 3) {
                isPropertyDefinition = PropertyTypes.Where(p => p.Contains(words[1])).Any();
                if (isPropertyDefinition) {
                    property = new ServiceProperty() {
                        IsNullable = words[1].Contains("Nullable"),
                        ReturnType = words[1],
                        Name = words[2]
                    };
                }
            }
            return property;
        }

    }

    public class FileData {
        public FileInfo FInfo { get; set; }
        public DataFile Data { get; set; }
    }
}
