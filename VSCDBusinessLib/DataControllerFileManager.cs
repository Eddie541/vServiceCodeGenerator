using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSCDBusinessLib {
    public class DataControllerFileManager : IDisposable {

        private DataFileCollection _dataFiles;
        private ServiceInterfaceCollection _serviceInterfaceCollection;
        private CodeFileWriter _fileWriter;
        private DataControllerLibrary _dataControllerLib;
        private List<ClassDefinition> Controllers { get; set; }
        private List<UsingStatement> usings;

        private string _namespaceName;

        private string _destinationDirectory;

        private bool disposed = false;

        public DataControllerFileManager(DataFileCollection dataFiles, string destinationDirectory, string namespaceName, ServiceInterfaceCollection serviceInterfaces) {
            _destinationDirectory = destinationDirectory;
            _dataFiles = dataFiles;
            _namespaceName = namespaceName;
            _dataControllerLib = new DataControllerLibrary();
            _serviceInterfaceCollection = serviceInterfaces;
            Controllers = new List<ClassDefinition>();


            SetUsings();

        }

        public void GenerateControllers() {
            List<string> typeParameters = new List<string>();
            
            foreach (DataFile dataFile in _dataFiles) {
                DataController dataController = new DataController();
                typeParameters.Clear();
                dataController.Name = dataFile.ControllerName;
                dataController.Modifier = ManagerIOBase.Partial;
                dataController.Scope = ManagerIOBase.Public;
                typeParameters.Add(dataFile.ServiceDataName);
                typeParameters.Add(dataFile.ControllerKeyType);
                string dataViewName = dataFile.DataFileName.Replace(".cs", "");
                typeParameters.Add(dataViewName);
                ServiceInterface sinterface = _serviceInterfaceCollection.GetServiceInterfaceNamed(dataFile.InterfaceName);
                if (sinterface != null) {
                    CreateCloneMethod(dataController, sinterface, dataFile);
                }

                dataController.AddBaseClass("BaseApiController", typeParameters);
                Controllers.Add(dataController);
            }
            SetDataControllerLibrary();
        }

        private void SetDataControllerLibrary() {
            foreach (DataController controller in Controllers) {
                CodeFile cf = new CodeFile(usings, _namespaceName, controller);
                _dataControllerLib.Add(cf);

            }
            _fileWriter = new CodeFileWriter(_dataControllerLib, _destinationDirectory);
            _fileWriter.WriteCodeFiles();

           
        }

        private void CreateCloneMethod(DataController dataController, ServiceInterface serviceInterface, DataFile dataFile) {
            Method method = new Method();
            method.Modifier = ManagerIOBase.Override;
            method.Scope = ManagerIOBase.Public;
            method.ReturnType = serviceInterface.Name.Remove(0, 1); // interface name with out the I
            method.Name = "Clone";
            method.Parameters.Add(new MethodParameter() {
                Name = "data",
                ParameterType = dataFile.DataFileName.Replace(".cs", "")                
            });
            Statement statement = new Statement();
            statement.Tabbing = 2;
            statement.LeftSide = method.ReturnType + " service";
            statement.Operand = CodeFile.Equalsx;
            statement.RightSide = "new " + method.ReturnType + "() {";
            method.Statements.Add(statement);
            int current = 0;
            int count = serviceInterface.ServiceProperties.Count;

            foreach (Property p in serviceInterface.ServiceProperties) {
                current++;
                Statement propertyStatement = new Statement() {
                    Tabbing = 3,
                    LeftSide = p.Name,
                    Operand = CodeFile.Equalsx,
                    RightSide = "data." + p.Name + (current < count ? "," : "")
                };
                method.Statements.Add(propertyStatement);
            }
            Statement closingStatement = new Statement() {
                Tabbing = 2,
                LeftSide = "}",
                Terminate = true
            };
            method.Statements.Add(closingStatement);
            Statement returnStatement = new Statement() {
                Tabbing = 2,
                IsReturn = true,
                RightSide = "service",
                Terminate = true
            };
            method.Statements.Add(returnStatement);
            dataController.AddMethod(method);
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
                Assembly = "LocalDbDataLib"
            });
            usings.Add(new UsingStatement() {
                Assembly = "ServiceDataLib"
            });

        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposed) {
                return;
            }
            if (disposing) {           
                _fileWriter = null;
            }

            disposed = true;
        }

    }
}
