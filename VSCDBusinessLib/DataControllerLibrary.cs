using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSCDBusinessLib {
    public class DataControllerLibrary : List<CodeFile> {

    }

    public class DataController : ClassDefinition {


        public List<Method> Methods { get; set; }


        public DataController() {
            this.CodeType = CodeFileType.Class;
            Methods = new List<Method>();    
        }

        public void AddMethod(Method method) {
            Methods.Add(method);
        }

        protected override void SetDefinitionContent() {

            List<Method> methods = this.Methods.OrderBy(m => m.Name.TrimStart(new char[] { '_' })).ToList();

            foreach (Method method in methods) {
                DefinitionContents.AppendLine(string.Format("{0}{1}", "\t", method.ToString()));
            }
        }

    }
}
