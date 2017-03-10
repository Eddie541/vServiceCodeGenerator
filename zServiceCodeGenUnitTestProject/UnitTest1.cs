using System;
using System.Text;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilitiesLib;
using ValiantServiceGeneratorViewModelLib;
using VSCDBusinessLib;

namespace zServiceCodeGenUnitTestProject {
    [TestClass]
    public class UnitTest1 : IDisposable {

        ServiceDataFileManager sdfm;
        DataManagerFileManager dmfm;
        DataControllerFileManager dcfm;

        [TestMethod]
        public void TestMethod1() {
            MainViewModel mvm = new MainViewModel() {
                SourceDirectory = @"C:\Projects\DataService\LocalDbDataLib",
                //SourceDirectory = @"C:\Projects\DataService\LocalDbDataLib\Blah1\Blah2\Blah3\Blah4\Blah5\Blah6\Blah7\Blah8\Blah9\Blah10\Blah5\Blah6\Blah7\Blah8\Blah9\Blah10\test.txt",
                DestinationDirectory = @"C:\testGen\DataService.ServiceInterface",
                ControllerDirectory = @"C:\testGen\DataService.Controllers",
                InterfaceDefinitions = @"C:\testGen\DataFiles\ServiceDataSchema.xml",
                SchemaDirectory = @"C:\testGen\Schemas",
                DataManagerDefinitions = @"C:\testGen\DataFiles\DataManagerLibrary.xml",
                DataManagerNamespace = "Business",
                ManagerDirectory = @"C:\testGen\DataManagerTest",
                ServiceNamespace = "DataService"

            };

            string x = StringUtilities.TrimPathFileName(mvm.SourceDirectory, 100);

            //Console.Write(x);
            sdfm = new ServiceDataFileManager(mvm.SchemaDirectory, mvm.InterfaceDefinitions, mvm.SourceDirectory,
                mvm.DestinationDirectory);
            sdfm.GenerateServiceData();


            dcfm = new DataControllerFileManager(sdfm.DataFiles, mvm.ControllerDirectory, mvm.ServiceNamespace, sdfm.ServiceInterfaces);
            dcfm.GenerateControllers();

            dmfm = new DataManagerFileManager(mvm.SchemaDirectory, mvm.DataManagerDefinitions, mvm.DataManagerNamespace, mvm.ManagerDirectory);
            dmfm.GenerateDataManagerCode();


        }





        #region IDisposable Members

        private bool disposed = false;

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposed) {
                return;
            }
            if (disposing) {
                sdfm.Dispose();
                dcfm.Dispose();
                dmfm.Dispose();
            }

            disposed = true;

        }

        #endregion
    }
}
