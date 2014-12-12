using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UtilitiesLib;

namespace VSCDBusinessLib {
    public class DataManagerFileReader : ManagerIOBase {

        private List<ClassDefinition> DataManagers { get; set; }
        private List<UsingStatement> usings;
        public DataManagerLibrary DataManagerLib { get; private set; }

        private string _namespaceName;      

        private XMLBuilder _builder;

        public DataManagerFileReader(XMLBuilder builder, string namespaceName) {
           
            DataManagers = new List<ClassDefinition>();
            // todo is constructed with validated xDoc 
            _builder = builder;
            _namespaceName = namespaceName;
            DataManagerLib = new DataManagerLibrary();
            SetUsings();
          

        }

        public void ReadDocument() {
            try {
                IEnumerable<DataManager> dataManagers = from d in _builder.CurrentDocument.Descendants(XMLBuilder.GetElementXName(DataManager, BusinessConstants.JmlfdcNamespace))
                                                        select new DataManager () {
                                                            Name = ((string)d.Attribute("name").Value),
                                                            Modifier = ((string)d.Attribute("modifier").Value),
                                                            Scope = ((string)d.Attribute("scope").Value),
                                                            Methods = (from m in d.Descendants(XMLBuilder.GetElementXName(Method, BusinessConstants.JmlfdcNamespace))
                                                                       select new Method() {
                                                                           Name = ((string)m.Attribute("name")),
                                                                           ReturnType = ((string)m.Attribute("return")),
                                                                           Modifier = Abstract,
                                                                           Scope = Protected,
                                                                           Parameters = (from p in m.Descendants(XMLBuilder.GetElementXName(Parameter, BusinessConstants.JmlfdcNamespace))
                                                                                         select new MethodParameter() {
                                                                                             Name = ((string)p.Attribute("name")),
                                                                                             ParameterType = ((string)p.Attribute("type")),
                                                                                         }).ToList()
                                                                       }).ToList(),
                                                        };
                DataManagers.AddRange(dataManagers); 
                SetDataManagerLibrary();
              
            } catch (Exception) {
                throw;
            }
        }


        private void SetDataManagerLibrary() {
            foreach (DataManager dm in DataManagers) {               

                //DataManager derivedClass = new DataManager() {
                //    Name = dm.Name,
                //    Scope = Public,
                //    Modifier = "" // todo use partial??
                //};
                //derivedClass.AddBaseClass(dm.Name + "Base");
                //CodeFile derivedCodeFile = new CodeFile(usings, _namespaceName, derivedClass);
                //DataManagerLib.Add(derivedCodeFile);

                if (dm.Modifier.Equals(Abstract)) {                    
                    dm.AddProperty(new DataProperty() {
                        InstanceStatement = new Declaration() {
                            Modifier = Static,
                            Name = _Instance,
                            ReturnType = dm.Name,
                            Scope = Private
                        },
                        AllowSet = false,
                        IsInitializer = true,
                        ReturnType = dm.Name,
                        Modifier = Static,
                        Scope = Protected,
                        Name = Instance
                    });
                    dm.Name += "Base";
                    SetPublicMethods(dm.Methods);
                    dm.AddBaseClass("BaseDataManager");
                }
                CodeFile cf = new CodeFile(usings, _namespaceName, dm);
                DataManagerLib.Add(cf);
            }
        }

        private void SetPublicMethods(List<Method> methods) {
            List<Method> newMethods = new List<Method>();
            foreach (Method m in methods) {
                Method method = new Method() {
                    Name = m.Name,
                    Parameters = m.Parameters,
                    ReturnType = m.ReturnType,
                    Modifier = Static,
                    Scope = Public
                };
                SetMethodLogStatements(method);
                newMethods.Add(method);
            }

            methods.AddRange(newMethods);
        }


        private void SetMethodLogStatements(Method method) {
            Statement s = new Statement() {
                Terminate = false,
                IsReturn = true,
                LeftSide = string.Format("LogMethod<{0}> ({1}{2}{3}, delegate {4}", method.ReturnType, '"', method.Name, '"', "{")

            };
            StringBuilder sb = new StringBuilder();
            int count = 0;
            int parameterCount = method.Parameters.Count;
            foreach (MethodParameter p in method.Parameters) {
                count++;
                if (count == parameterCount) {
                    sb.Append(p.Name);
                } else {
                    sb.Append(p.Name + ", ");
                }
            }
            Statement s1 = new Statement() {
                Terminate = true,
                IsReturn = true,
                LeftSide = string.Format("Instance._{0}({1})", method.Name, sb.ToString()),
                

            };
            Statement s2 = new Statement() {
                Terminate = true,
                IsReturn = false,
                LeftSide = "    })"

            };
            method.Statements.Add(s);
            method.Statements.Add(s1);
            method.Statements.Add(s2);
        }


        private void SetUsings() {
            if (usings == null) {
                usings = new List<UsingStatement>();
            } else {
                usings.Clear();
            }
            usings.Add(new UsingStatement() {
                Assembly = "System"
            });
            usings.Add(new UsingStatement() {
                Assembly = "System.Collections.Generic"
            });
            usings.Add(new UsingStatement() {
                Assembly = "System.Linq"
            });
            usings.Add(new UsingStatement() {
                Assembly = "DcamLocalDbDataLib"
            });
            usings.Add(new UsingStatement() {
                Assembly = "ServiceDataLib"
            });

        }

    }
}
