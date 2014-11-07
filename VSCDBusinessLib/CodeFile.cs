using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSCDBusinessLib {
    public class CodeFile {

        public const string Equals = "=";
        public const string GreaterThan = ">";
        public const string LessThan = "<";
        public const string GreaterThanOrEquals = ">=";
        public const string LessThanOrEquals = "<=";
        public const string NotEquals = "!=";


        public List<UsingStatement> Usings;
        public NamespaceDefinition NameSpace;
        public ClassDefinition ClassDefinition;
        public int TabIndent = 0;

        public CodeFile(List<UsingStatement> usings, string namespaceName, ClassDefinition classDef) {
            Usings = usings;
            NameSpace = new NamespaceDefinition(namespaceName);
            ClassDefinition = classDef;
        }

        public string FileName { get { return ClassDefinition.FileName; } }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            foreach(UsingStatement us in Usings) {
                sb.AppendLine(us.ToString());
            }
            sb.AppendLine(NameSpace.ToString());
            sb.AppendLine(ClassDefinition.ToString());
            sb.AppendLine("     }");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }

    public enum CodeFileType {
        Class,
        Interface,
        Structure
    }

    public class UsingStatement {
        public string Assembly;
        public override string ToString() {
            return string.Format("using {0};", Assembly);
        }

    }

    public class NamespaceDefinition {
        public NamespaceDefinition(string name) {
            NamespaceName = name;
        }
        public string NamespaceName;
        public override string ToString() {
            return string.Format("{0} {1} {2}", "namespace", this.NamespaceName, "{");
        }

    }

    public class ClassDefinition {
        private string _modifier = ManagerIOBase.Partial;        
        public string Modifier { get { return _modifier; } set { _modifier = value; } }
        private string _scope = ManagerIOBase.Public;
        public string Scope { get { return _scope; } set { _scope = value; } }
        public string Name { get; set; }
        public string FileName {
            get {
                if (Modifier.Equals(ManagerIOBase.Abstract) || Modifier.Equals(ManagerIOBase.Partial)) {
                    return Name + ".gen.cs";
                } else {
                    return Name + ".cs";
                }
            }
        } 
        protected List<BaseClass> BaseClasses { get; set; }

        //public void AddBaseClass(string baseClassName) {
        //    BaseClasses.Add(new BaseClass() {
        //        BaseName = baseClassName
        //    });
        //}


        protected CodeFileType CodeType;
        
        private StringBuilder Content = new StringBuilder();
        protected StringBuilder DefinitionContents = new StringBuilder();

        public ClassDefinition() {
            BaseClasses = new List<BaseClass>();
        }

        public override string ToString() {
            string definition = "";
            switch (CodeType) {
                case CodeFileType.Class:
                    definition = string.Format("    {0} {1} class {2} {3} {4}", Scope, Modifier, Name, SetBaseClasses(), "{");
                    break;
                case CodeFileType.Interface:
                    definition = string.Format("    {0} {1} interface {2} {3}", Scope, Modifier, Name, "{");
                    break;
                case CodeFileType.Structure:
                    definition = string.Format("    {0} {1} struct {2} {3}", Scope, Modifier, Name, "{");
                    break;
            }
            Content.AppendLine(definition);
            SetDefinitionContent();
            Content.AppendLine(DefinitionContents.ToString());           
            return Content.ToString();
        }

        protected virtual void SetDefinitionContent() {

        }

        private string SetBaseClasses() {
            StringBuilder sb = new StringBuilder();
            int baseCount = BaseClasses.Count;
            int count = 0;
            if (baseCount > 0) {
                sb.Append(" : ");
            }
            foreach (BaseClass baseClass in BaseClasses) {
                count++;
                if (count == baseCount) {
                    sb.Append(baseClass.ToString());
                } else {
                    sb.Append(baseClass.ToString() + ", ");
                }

            }
            return sb.ToString();
        }

        public void AddBaseClass(string baseClassName, List<string> typeParameters = null) {
            BaseClass baseClass = new BaseClass();
            baseClass.BaseName = baseClassName;
            if (typeParameters != null) {
                foreach (string parameter in typeParameters) {
                    baseClass.AddTypeParameter(parameter);
                }
            }
            BaseClasses.Add(baseClass);
        }


    }


    public class BaseClass {
        public string BaseName;
        public void AddTypeParameter(string parameter) {
            TypeParameters.Add(parameter);
        }
        private List<string> _typeParameters;
        public List<string> TypeParameters {
            get {
                if (_typeParameters == null) {
                    _typeParameters = new List<string>();
                }
                return _typeParameters;
            }
            private set { _typeParameters = value; }
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append(BaseName);
            int count = 0;
            int parameterCount = TypeParameters.Count;
            foreach (string tParam in TypeParameters) {
                if (count == 0) {
                    sb.Append("<");
                }
                count++;
                if (count == parameterCount) {
                    sb.Append(tParam);
                    sb.Append(">");
                } else {
                    sb.Append(tParam + ", ");
                }
            }
            return sb.ToString();
            
        }
    }


    public abstract class Property {
        public string Name;
        public string ReturnType;
        public string Scope;
        public string Modifier;
    }

    public class Method {
        public string Name {get; set;}
        public string ReturnType { get; set; }
        public string Modifier { get; set; }
        public string Scope { get; set; }
        public List<MethodParameter> Parameters { get; set; }
        public List<Statement> Statements { get; set; }
        public Method() {
            Parameters = new List<MethodParameter>();
            Statements = new List<Statement>();
        }

        public override string ToString() {
            bool isAbstract = false;
            if (Modifier != null && Modifier.Equals(ManagerIOBase.Abstract)) {
                isAbstract = true;
            }
            StringBuilder sb = new StringBuilder();
            string name = isAbstract ? "_" + Name : Name;
            sb.Append(string.Format("    {0} {1} {2} {3} {4}", Scope, Modifier, ReturnType, name, "("));
                int parameterCount = Parameters.Count;
                int count = 0;
                foreach (MethodParameter mParam in Parameters) {
                    count++;
                    if (count == parameterCount) {
                        sb.Append(mParam.ToString());
                    } else {
                        sb.Append(mParam.ToString() + ",");
                    }
                }
            if (isAbstract) {
                sb.AppendLine(");");
            } else {
                sb.AppendLine(") {");
                foreach (Statement s in Statements) {
                    sb.Append("     " + s.ToString());
                }
                sb.AppendLine("          }");
            }
            
            return sb.ToString();
        }

    }

    public class MethodParameter {
        public string Name { get; set; }
        public string ParameterType { get; set; }
        public override string ToString() {
            return string.Format("{0} {1}", ParameterType, Name); 
        }

    }

    public class Declaration {
        public string Name;
        public string ReturnType;
        public string Modifier;
        public string Scope;
        public override string ToString() {
            return string.Format("{0} {1} {2} {3};", Scope, Modifier, ReturnType, Name);
        }
    }

    public class Statement {
        public bool IsReturn;
        public bool Terminate;
        public StringBuilder Content;
        public string LeftSide;
        public string Operand;
        public string RightSide;
        public int Tabbing = 0;

        public Statement() {
            Content = new StringBuilder();
        }


        public override string ToString() {
            Content.Append(IsReturn ? (InsertTabbing + "return ") : "");
            if (LeftSide != null) {
                Content.AppendFormat(InsertTabbing + "{0}", LeftSide);
            }
            if (Operand != null) {
                Content.AppendFormat(" {0}", Operand);
            }
            if (RightSide != null) {
                Content.AppendFormat(" {0}", RightSide);
            }
            Content.AppendLine(Terminate ? ";" : "");
            return Content.ToString();
        }

        private string InsertTabbing {
            get {
                StringBuilder sb = new StringBuilder();
                for (int t = 0; t < Tabbing; t++) {
                    sb.Append("\t");
                }
                return sb.ToString();
            }
        }

    }
   
}
