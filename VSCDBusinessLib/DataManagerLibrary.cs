using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSCDBusinessLib {
    public class DataManagerLibrary : List<CodeFile> {

    }

    public class DataManager : ClassDefinition {

        public List<DataProperty> Properties { get; set; }
        public List<Method> Methods { get; set; }

        public DataManager() {
            this.CodeType = CodeFileType.Class;         

            Properties = new List<DataProperty>();
            Methods = new List<Method>();           
            
        }

        public void AddProperty(DataProperty property) {
            Properties.Add(property);
        }

        public void AddMethod(Method method) {
            Methods.Add(method);
        }

        protected override void SetDefinitionContent() {

            //this.Properties = this.Properties.OrderBy(p => p.Name.TrimStart(new char[] { '_' })).ToList(); ;
            //this.Methods = this.Methods.OrderBy(m => m.Name.TrimStart(new char [] {'_'})).ToList();
            List<Method> methods = this.Methods.OrderBy(m => m.Name.TrimStart(new char[] { '_' })).ToList();

            foreach (DataProperty property in Properties) {
                DefinitionContents.AppendLine(string.Format("{0}{1}", "\t", property.ToString()));
            }

            foreach (Method method in methods) {
                DefinitionContents.AppendLine(string.Format("{0}{1}", "\t", method.ToString()));
            }
        }


    }

    public class DataProperty : Property {
        public bool AllowSet = true;
        public bool IsInitializer = false;
        public Declaration InstanceStatement;
        public override string ToString() {
            if (IsInitializer && InstanceStatement != null) {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(InstanceStatement.ToString());
                sb.Append(string.Format("       {0} {1} {2} {3}", Scope, this.Modifier, this.ReturnType, this.Name));
                sb.AppendLine("{ get {");
                sb.AppendLine(string.Format("          if ({0} == null) {1}", InstanceStatement.Name, "{"));
                sb.AppendLine(string.Format("               {0} = new {1}();", InstanceStatement.Name, this.ReturnType));
                sb.AppendLine("             }");
                sb.AppendLine(string.Format("             return {0};", InstanceStatement.Name));
                sb.AppendLine("             }");
                sb.AppendLine("         }");
                return sb.ToString();
            } else {
                return string.Format("{0} {1} {2} {3}", Scope, this.ReturnType, this.Name, (AllowSet ? "{ get; set; }" : "{ get; private set;}"));
            }
        }


    }
}
