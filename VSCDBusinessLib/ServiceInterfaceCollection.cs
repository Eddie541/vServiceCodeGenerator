using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSCDBusinessLib {
    public class ServiceInterfaceCollection : List<ServiceInterface> {

        public bool Contains(string interfaceName) {
            return this.Where(s => s.Name.Equals(interfaceName, System.StringComparison.Ordinal)).Any();
        }

        public ServiceInterface GetServiceInterfaceNamed(string interfaceName) {
            return this.Where(s => s.Name.Equals(interfaceName, System.StringComparison.Ordinal)).SingleOrDefault();
        }
    }
}
