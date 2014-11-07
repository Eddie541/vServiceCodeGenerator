
using System;
namespace VSCDBusinessLib {
    public class DataFile {
        private string _interfaceName;
        public string InterfaceName {
            get { return _interfaceName; }
            set {
                _interfaceName = value;
                
                string s = value.Remove(0, 1); //.TrimStart(new char[] { 'I' });
                ServiceDataName = s;

                s = s.Replace("ServiceData", "Controller");               
                ControllerName = s;
            }            
        }

        
        public string ControllerName {
            get;
            private set;
        }

        public string ServiceDataName {
            get;
            private set;
        }

        public string ControllerKeyType { get; set; }

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
