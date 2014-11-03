
using System;
namespace VSCDBusinessLib {
    public class DataFile {
        public string InterfaceName { get; set; }
        private string _dataFileName;
        public string DataFileName {
            get { return _dataFileName; }
            set {
                if (string.IsNullOrEmpty(_dataFileName) || (value.EndsWith(".cs") == false)) {
                    _dataFileName = value + ".cs";
                } else {
                    _dataFileName = value;
                }
            }
        }

      
    }
}
