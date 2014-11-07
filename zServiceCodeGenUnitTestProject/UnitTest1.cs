using System;
using System.Text;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilitiesLib;
using ValiantServiceGeneratorViewModelLib;
using VSCDBusinessLib;

namespace zServiceCodeGenUnitTestProject {
    [TestClass]
    public class UnitTest1 {

        ServiceDataFileManager sdfm;
        DataManagerFileManager dmfm;
        DataControllerFileManager dcfm;

        [TestMethod]
        public void TestMethod1() {
            MainViewModel mvm = new MainViewModel() {
                SourceDirectory = @"C:\Projects\DcamDataService\DcamLocalDbDataLib",
                DestinationDirectory = @"C:\testGen\DcamDataService.ServiceInterface",
                ControllerDirectory = @"C:\testGen\DcamDataService.Controllers",
                InterfaceDefinitions = @"C:\testGen\DataFiles\ServiceDataSchema.xml",
                SchemaDirectory = @"C:\testGen\Schemas",
                DataManagerDefinitions = @"C:\testGen\DataFiles\DataManagerLibrary.xml",
                DataManagerNamespace = "DcamBusiness",
                ManagerDirectory = @"C:\testGen\DataManagerTest",
                ServiceNamespace = "DcamDataService"

            };

            sdfm = new ServiceDataFileManager(mvm.SchemaDirectory, mvm.InterfaceDefinitions, mvm.SourceDirectory,
                mvm.DestinationDirectory);
            sdfm.GenerateServiceData();


            dcfm = new DataControllerFileManager(sdfm.DataFiles, mvm.ControllerDirectory, mvm.ServiceNamespace, sdfm.ServiceInterfaces);
            dcfm.GenerateControllers();

            dmfm = new DataManagerFileManager(mvm.SchemaDirectory, mvm.DataManagerDefinitions, mvm.DataManagerNamespace, mvm.ManagerDirectory);
            dmfm.GenerateDataManagerCode();


        }




    }
}
