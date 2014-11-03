using System.Collections.Generic;
using System.Linq;

namespace VSCDBusinessLib {
    public class DataFileCollection : List<DataFile> {

        
        public DataFileCollection(IEnumerable<DataFile> dataFiles) {
            this.AddRange(dataFiles);
        }

        public bool Contains(string dataFileName) {
            return this.Where(d => d.DataFileName.Equals(dataFileName, System.StringComparison.Ordinal)).Any();           
        }
      
    }
}
