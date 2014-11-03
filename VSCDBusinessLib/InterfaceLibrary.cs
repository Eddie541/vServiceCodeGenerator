using System.Collections.Generic;

namespace VSCDBusinessLib {
    public class InterfaceLibrary : List<CodeFile> {

        //public void AddServiceInterface(CodeFile cf) {
        //    this.Add(cf);
        //}
        
    }

    public class ServiceInterface : ClassDefinition {
              
        public List<ServiceProperty> ServiceProperties = new List<ServiceProperty>();

        public ServiceInterface(string interfaceName) {
            Name = interfaceName;
            CodeType = CodeFileType.Interface;
        }
        public void AddServiceProperty(ServiceProperty sp) {
            ServiceProperties.Add(sp);
        }

        protected override void SetDefinitionContent() {
            foreach (ServiceProperty property in ServiceProperties) {
                DefinitionContents.AppendLine(string.Format("{0}{1}","\t", property.ToString()));
            }
        }

        

    }

    public class ServiceProperty : Property {
        public bool IsNullable;
        public override string ToString() {
            return string.Format("{0} {1} {2}", this.ReturnType, this.Name, "{ get; set; }");
        }
    }
  
}
